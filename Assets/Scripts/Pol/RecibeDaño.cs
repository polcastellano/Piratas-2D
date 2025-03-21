using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecibeDaño : MonoBehaviour
{
    public int damage;
    public float empuje;
    public bool recibeEmpuje = false;
    public Rigidbody2D rb;
    private float fuerzaEmpuje;
    public float empujeVertical;
    public float coolDownDaño;
    public EstadosEnemigo estadosEnemigo;
    public bool puedeRecibir = true;
    public AudioManager audioManager;
    [SerializeField] private Material sharedMaterial;

    public AudioSource enemySource;

    private IEnumerator CooldownDaño()
    {
        puedeRecibir = false;
        // Espera por el tiempo especificado
        yield return new WaitForSeconds(coolDownDaño);
        puedeRecibir = true;
    }

    public void Quitarcolor()
    {
        sharedMaterial.SetColor("_Color", Color.white);
    }


    // Start is called before the first frame update
    void Start()
    {
        estadosEnemigo = gameObject.GetComponentInParent<EstadosEnemigo>();
        if (audioManager == null)
        {
            audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (recibeEmpuje)
        {
            rb.AddForce(new Vector2(fuerzaEmpuje, empujeVertical), ForceMode2D.Impulse);
            recibeEmpuje = false;
        }
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other != null && other.CompareTag("SwordJack") && puedeRecibir/*  && CheckGround.isGrounded */)
        {
            if (Ron.bebido)
            {
                estadosEnemigo.Damage(2f);
            }
            else
            {
                estadosEnemigo.Damage(1f);
            }

            sharedMaterial.SetColor("_Color", Color.red);
            Invoke("Quitarcolor", 0.3f);

            audioManager.ReproduceEnemigoDaño(enemySource);
            rb = gameObject.GetComponentInParent<Rigidbody2D>();

            if (rb != null)
            {
                // Calcula la dirección del empuje basado en la posición relativa del jugador
                float direccionEmpuje = other.transform.position.x > transform.position.x ? -1 : 1;

                // Aumenta el valor de empuje para pruebas
                fuerzaEmpuje = direccionEmpuje * empuje; // Multiplica por un valor grande para probar

                // Aplica la fuerza en la dirección calculada
                recibeEmpuje = true;

                StartCoroutine(CooldownDaño());
            }
        }
    }
}
