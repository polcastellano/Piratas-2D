using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class EstadosEnemigo : MonoBehaviour
{
    enum Estados { Perseguir, Patrulla, Volver, Aturdido, Atacar, Debilitado };
    private Animator e_animator;
    [SerializeField] Estados e_Estados;
    public Transform objetivo;
    public Vector2 posicionInicial;
    public Vector2 posicionFinal;
    public GameObject piesJack;


    //Definicion de varaibles del estado del enemigo
    public bool zonaDeteccion = false;
    public bool zonaAturdido = false;
    public bool zonaAtaque = false;
    public bool patrullando = true;
    public bool aturdido = false;
    public bool debilitarEjecutado = false;


    //Definicion de varaibles para los taimings del enemigo
    public float vidas;
    public float vidasMax;
    public float damageTime = 1f; // Tiempo que tarda en reducir vida al recibir dano
    public float velocidadMax;
    private float distanciaTotal;
    public float distanciaMinima;
    public float tiempoAturdido = 2.0f;
    public float sumaPuntoFinal;
    private float tiempoLocal = 0f; // Temporizador local
    public GameObject objetoDebilitado; // Objeto que se activará al entrar en estado debilitado
    public RecibeDaño recibeDaño;
    public Transform[] hijosADesactivar;



    public GameObject monedaPrefab;
    private Vector3 desplazamientoMoneda = new Vector3(1, 1, 0);
    public Canvas barraVida;
    public Image healthBar; // Imagen de la barra de vida (relleno)
    Color healthColor = new Color(0.8f, 0.19f, 0.19f); // Rojo oscuro
    Color lowerHealthColor = new Color(1f, 0.4f, 0.4f); // Rojo claro


    public void Patrullar()
    {

        velocidadMax = 2.5f;
        // Incrementar el temporizador local con el tiempo delta
        tiempoLocal += Time.deltaTime;
        // Calcular el nuevo valor de la posición utilizando Mathf.PingPong para el patrullaje
        float pingPong = Mathf.PingPong(tiempoLocal * velocidadMax, distanciaTotal);

        // Mover el objeto a lo largo de la línea entre inicio y final
        transform.position = Vector2.Lerp(posicionInicial, posicionFinal, pingPong / distanciaTotal);

        // El enemigo camina en la dirección adecuada
        if (pingPong <= 0.05f)
        {
            transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
        else if (pingPong >= distanciaTotal - 0.05f)
        {
            transform.localRotation = Quaternion.Euler(0, 180, 0);
        }

    }


    public void Perseguir()
    {
        velocidadMax = 3.0f;

        // Calcula la distancia actual al objetivo
        float distanciaActual = Vector2.Distance(transform.position, objetivo.position);

        if (distanciaActual > distanciaMinima)
        {
            // Mover el objeto hacia el objetivo de manera suave, usando Time.deltaTime para un movimiento gradual
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(objetivo.position.x, transform.position.y), velocidadMax * Time.deltaTime);
        }
        // Verificar si el objeto ha alcanzado el punto inicial o final para hacer flipX
        if (transform.position.x < objetivo.position.x)
        {
            transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
        else if (transform.position.x > objetivo.position.x)
        {
            transform.localRotation = Quaternion.Euler(0, 180, 0);
        }
    }

    void Volver()
    {
        velocidadMax = 2.5f;
        transform.position = Vector2.MoveTowards(transform.position, posicionInicial, velocidadMax * Time.deltaTime);

        // Verificar si el objeto ha alcanzado el punto inicial o final para hacer flipX
        if (transform.position.x < posicionInicial.x)
        {
            transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
        else if (transform.position.x > posicionInicial.x)
        {
            transform.localRotation = Quaternion.Euler(0, 180, 0);
        }
    }

    // Función para bloquear el Transform por un tiempo en segundos
    public void AturdirEnemigo()
    {
        if (!aturdido)
        {
            zonaAtaque = false;
            aturdido = true;

            piesJack.GetComponent<BoxCollider2D>().enabled = false;

            gameObject.GetComponent<BoxCollider2D>().enabled = false;
            gameObject.GetComponent<Rigidbody2D>().isKinematic = true;
            gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX;
            gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionY;

            StartCoroutine(AturdirEnemigoCoroutine(tiempoAturdido));
        }
    }

    private IEnumerator AturdirEnemigoCoroutine(float tiempoAturdido)
    {
        // Espera por el tiempo especificado
        yield return new WaitForSeconds(tiempoAturdido);

        gameObject.GetComponent<BoxCollider2D>().enabled = true;
        gameObject.GetComponent<Rigidbody2D>().isKinematic = false;
        gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;

        zonaAturdido = false; // Desactiva el bloqueo
        aturdido = false;
        Invoke("ActivarPiesJack", 1);

    }

    public void ActivarPiesJack()
    {
        piesJack.GetComponent<BoxCollider2D>().enabled = true;

    }

    public void DebilitarEnemigo()
    {
        // Verificar si ya se ha ejecutado
        if (debilitarEjecutado) return;

        debilitarEjecutado = true; // Marcar como ejecutado

        foreach (Transform hijo in hijosADesactivar)
        {
            hijo.gameObject.SetActive(false);
        }
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        gameObject.GetComponent<Rigidbody2D>().isKinematic = true;
        gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX;
        gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionY;
        healthBar.fillAmount = 0f;

        GenerateCoin();
        Invoke("DebilitarEnemigoCorutina", 10);

    }

    void GenerateCoin()
    {
        // Instancia la moneda en la posición y rotación del enemigo
        GameObject moneda = Instantiate(monedaPrefab, transform.position + desplazamientoMoneda, Quaternion.identity);

        // Asigna el enemigo (este objeto) como el padre de la moneda
        moneda.transform.SetParent(transform);
        moneda.transform.SetParent(null);
    }

    public void DebilitarEnemigoCorutina()
    {
        foreach (Transform hijo in hijosADesactivar)
        {
            hijo.gameObject.SetActive(true);
        }

        if (gameObject.name.Equals("Esqueleto"))
        {
            vidas = 6;
        }
        else if (gameObject.name.Equals("Enemigo"))
        {
            vidas = 2;
        }
        recibeDaño.puedeRecibir = true;
        debilitarEjecutado = false;
        gameObject.GetComponent<BoxCollider2D>().enabled = true;
        gameObject.GetComponent<Rigidbody2D>().isKinematic = false;
        gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
        healthBar.fillAmount = vidasMax;



    }

    // Funci n que disminuye la barra de vida
    public void Damage(float quantity)
    {
        StartCoroutine(DecreaseHealthOverTime(quantity)); // Inicia la reducci n gradual de la salud
    }

    // Corrutina para disminuir la salud de forma progresiva
    private IEnumerator DecreaseHealthOverTime(float quantity)
    {
        float initialHealth = vidas;
        float finalHealth = Mathf.Clamp(vidas - quantity, 0, vidasMax);
        float elapsedTime = 0f;

        while (elapsedTime < damageTime)
        {
            elapsedTime += Time.deltaTime;
            vidas = Mathf.Lerp(initialHealth, finalHealth, elapsedTime / damageTime); // Interpola entre la salud inicial y final
            healthBar.fillAmount = vidas / vidasMax; // Actualiza el relleno de la barra de vida
            healthBar.color = Color.Lerp(lowerHealthColor, healthColor, Mathf.PingPong(elapsedTime * 4, 1)); // Parpadeo r pido

            yield return null; // Espera un frame
        }

        vidas = finalHealth; // Aseg rate de que el valor final se asigna
        healthBar.color = (vidas <= 3) ? lowerHealthColor : healthColor; // Establece el color de la barra de vida despu s de recibir da o
    }


    // Start is called before the first frame update
    void Start()
    {
        vidasMax = vidas;
        e_animator = GetComponent<Animator>();
        e_Estados = Estados.Patrulla;
        piesJack = GameObject.Find("PiesJack");

        posicionInicial = gameObject.transform.position;
        posicionFinal = new Vector2(posicionInicial.x + sumaPuntoFinal, posicionInicial.y);

        // Calcular la distancia total entre los dos puntos
        distanciaTotal = Vector2.Distance(posicionInicial, posicionFinal);

        Transform hijoEspada = gameObject.GetComponentInChildren<Transform>();
        Transform hijoRadar = gameObject.GetComponentInChildren<Transform>();
        Transform hijoAtaque = gameObject.GetComponentInChildren<Transform>();
        Transform hijoRecibeDaño = gameObject.GetComponentInChildren<Transform>();

        hijoEspada = hijoEspada.Find("Espada");
        hijoRadar = hijoRadar.Find("Radar");
        hijoAtaque = hijoAtaque.Find("Ataque");
        hijoRecibeDaño = hijoRecibeDaño.Find("RecibeDaño");


        hijosADesactivar = new Transform[] { hijoEspada, hijoRadar, hijoAtaque, hijoRecibeDaño };

    }

    // Update is called once per frame
    void Update()
    {

        if (vidas <= 0)
        {
            e_Estados = Estados.Debilitado;
        }
        else if (zonaAturdido && !aturdido)
        {
            e_Estados = Estados.Aturdido;
        }
        else if (!zonaAturdido)
        {
            if (zonaDeteccion)
            {
                if (zonaAtaque)
                {
                    e_Estados = Estados.Atacar;
                }
                else
                {
                    e_Estados = Estados.Perseguir;
                }
            }
            else if (!zonaDeteccion)
            {
                if (patrullando)
                {
                    e_Estados = Estados.Patrulla;
                }
                else if (!patrullando)
                {
                    if (Vector2.Distance(posicionInicial, transform.position) >= 0.09f)
                    {
                        e_Estados = Estados.Volver;
                    }
                    else if (Vector2.Distance(posicionInicial, transform.position) < 1f)
                    {
                        e_Estados = Estados.Patrulla;
                    }

                }
            }
        }

        if (e_Estados != Estados.Patrulla)
        {
            tiempoLocal = 0;
        }

        switch (e_Estados)
        {
            case Estados.Patrulla:
                e_animator.SetInteger("Estado", 0);
                patrullando = true;
                Patrullar();
                break;
            case Estados.Perseguir:

                e_animator.SetInteger("Estado", 1);
                Perseguir();
                break;
            case Estados.Volver:
                e_animator.SetInteger("Estado", 2);
                Volver();
                break;
            case Estados.Aturdido:
                e_animator.SetInteger("Estado", 3);
                AturdirEnemigo();
                break;
            case Estados.Atacar:
                e_animator.SetInteger("Estado", 4);
                break;
            case Estados.Debilitado:
                e_animator.SetInteger("Estado", 5);
                DebilitarEnemigo();
                break;
        }
    }
}
