using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class EstadosBarbossa : MonoBehaviour
{
    enum Estados { Idle, Perseguir, Attack1, Disparo, Daño, Jump, Morir }
    private Animator b_animator;
    [SerializeField] Estados b_Estados;
    public Transform objetivo;
    public Transform transformPistola;
    public CapsuleCollider2D zonaAtaque;
    public CapsuleCollider2D particulasCollider;
    public RecibeDañoBarbossa recibeDañoBarbossa;
    public ZonaDeteccion zonaDeteccion;
    public Image healthBar; // Imagen de la barra de vida (relleno)
    public float maximumHealth = 10f; // Vida maxima
    public float damageTime = 1f; // Tiempo que tarda en reducir vida al recibir da�o
    public float velocidadBala;
    Color healthColor = new Color(0.8f, 0.19f, 0.19f); // Rojo oscuro
    Color lowerHealthColor = new Color(1f, 0.4f, 0.4f); // Rojo claro
    private bool isFlashing = false; // Bandera para controlar el parpadeo
    public bool bloqueaEstado = false; // Nueva bandera para bloquear cambios de estado
    public GameObject balaPrefab;
    public GameObject balaInstanciada;

    public GameObject objectToDestroy;


    public float vidas;
    public float velocidad;
    public bool recibeAtaque = false;

    public GameObject particleNight;
    public GameObject particleDay;

    public void Perseguir()
    {
        // Mover el objeto hacia el objetivo de manera suave, usando Time.deltaTime para un movimiento gradual
        transform.position = Vector2.MoveTowards(transform.position, new Vector2(objetivo.position.x, transform.position.y), velocidad * Time.deltaTime);

        // Verificar si el objeto ha alcanzado el punto inicial o final para hacer flipX
        if (transform.position.x > objetivo.position.x)
        {
            transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
        else if (transform.position.x < objetivo.position.x)
        {
            transform.localRotation = Quaternion.Euler(0, 180, 0);
        }
    }
    // Funci�n que disminuye la barra de vida
    public void Damage(float quantity)
    {
        StartCoroutine(DecreaseHealthOverTime(quantity)); // Inicia la reducci�n gradual de la salud
    }

    // Corrutina para disminuir la salud de forma progresiva
    private IEnumerator DecreaseHealthOverTime(float quantity)
    {
        float initialHealth = vidas;
        float finalHealth = Mathf.Clamp(vidas - quantity, 0, maximumHealth);
        float elapsedTime = 0f;
        isFlashing = true; // Habilita el parpadeo

        while (elapsedTime < damageTime)
        {
            elapsedTime += Time.deltaTime;
            vidas = Mathf.Lerp(initialHealth, finalHealth, elapsedTime / damageTime); // Interpola entre la salud inicial y final
            healthBar.fillAmount = vidas / maximumHealth; // Actualiza el relleno de la barra de vida
            healthBar.color = Color.Lerp(lowerHealthColor, healthColor, Mathf.PingPong(elapsedTime * 4, 1)); // Parpadeo r�pido

            yield return null; // Espera un frame
        }

        vidas = finalHealth; // Aseg�rate de que el valor final se asigna
        isFlashing = false; // Desactiva el parpadeo
        healthBar.color = (vidas <= 3) ? lowerHealthColor : healthColor; // Establece el color de la barra de vida despu�s de recibir da�o
    }

    public void DebilitarBarbossa()
    {

        recibeAtaque = false;

        recibeDañoBarbossa.puedeRecibir = true;

    }

    private void MatarBarbossa()
    {
        PerlaNegra.barbossaIsDead = true;

        zonaDeteccion.bossHealth.SetActive(false);
        particleDay.SetActive(false);
        particleNight.SetActive(false);
        particulasCollider.enabled = false;

        gameObject.GetComponent<Rigidbody2D>().isKinematic = true;
        gameObject.GetComponent<CapsuleCollider2D>().enabled = false;

        // 🔹 **Destruir el objeto adicional si está asignado**
        if (objectToDestroy != null)
        {
            Destroy(objectToDestroy);
            Debug.Log($"🗑️ {objectToDestroy.name} ha sido destruido al morir Barbossa.");
        }
    }

    public void Disparar()
    {

        balaInstanciada = Instantiate(balaPrefab, transformPistola.position, transformPistola.rotation);
        long direccionBala = transform.localRotation.y == 0 ? -1 : 1;
        // Iniciar el movimiento del prefab
        StartCoroutine(MovePrefabToTarget(balaInstanciada, new Vector2(transformPistola.position.x + 10f * direccionBala, transformPistola.position.y)));
    }

    private IEnumerator MovePrefabToTarget(GameObject balaPrefab, Vector2 targetPosition)
    {
        while (balaPrefab != null && Vector2.Distance(balaPrefab.transform.position, targetPosition) > 0.1f)
        {
            // Verifica si la bala aún existe
            if (balaPrefab != null)
            {
                // Mueve la bala hacia la posición objetivo
                balaPrefab.transform.position = Vector2.MoveTowards(
                    balaPrefab.transform.position,
                    targetPosition,
                    velocidadBala * Time.deltaTime);
            }

            yield return null; // Esperar un frame antes de continuar
        }

        // Si la bala todavía existe después de salir del loop, destrúyela
        if (balaPrefab != null)
        {
            Destroy(balaPrefab);
        }
    }


    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Barbossa_Human_Light"))
        {
            Debug.Log("----> ME VUELVO ESQUELETO");
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Barbossa_Human_Light"))
        {
            Debug.Log("----> ME VUELVO HUMANO");
        }
    }

    private IEnumerator DesbloquearEstado(float tiempo)
    {
        Debug.Log("Esperando para desbloquear estado...");
        yield return new WaitForSeconds(tiempo);
        bloqueaEstado = false;
        Debug.Log("Estado desbloqueado.");
    }

    // Start is called before the first frame update
    void Start()
    {
        b_animator = GetComponent<Animator>();
        b_Estados = Estados.Idle;
        GetComponent<Rigidbody2D>().isKinematic = true;

    }

    // Update is called once per frame
    void Update()
    {
        if (bloqueaEstado) return; // Si está bloqueado, no cambia el estado

        if (vidas <= 0)
        {
            b_Estados = Estados.Morir;
        }
        else if (recibeAtaque)
        {
            b_Estados = Estados.Daño;
        }
        else if (ZonaAtaque.rangoAtaque)
        {
            if (b_Estados != Estados.Attack1 && b_Estados != Estados.Disparo)
            {
                System.Random random = new System.Random();
                int randomNumber = random.Next(1, 3);
                b_Estados = randomNumber == 2 ? Estados.Disparo : Estados.Attack1;
            }

        }

        else if (ZonaDeteccion.detectado)
        {
            b_Estados = Estados.Perseguir;
        }
        else
        {
            b_Estados = Estados.Idle;
        }

        // Maquina de estados
        switch (b_Estados)
        {
            case Estados.Idle:
                b_animator.SetInteger("Estado", 0);
                break;
            case Estados.Perseguir:
                b_animator.SetInteger("Estado", 1);
                Perseguir();
                break;
            case Estados.Attack1:
                b_animator.SetInteger("Estado", 2);
                break;
            case Estados.Disparo:
                bloqueaEstado = true; // Bloquear cambios de estado
                StartCoroutine(DesbloquearEstado(2.3f)); // Iniciar corrutina para desbloqueo
                b_animator.SetInteger("Estado", 3);
                break;
            case Estados.Jump:
                b_animator.SetInteger("Estado", 4);
                break;
            case Estados.Daño:
                b_animator.SetInteger("Estado", 5);
                Invoke("DebilitarBarbossa", 2);
                break;
            case Estados.Morir:
                b_animator.SetInteger("Estado", 6);
                MatarBarbossa();
                break;
        }
    }
}
