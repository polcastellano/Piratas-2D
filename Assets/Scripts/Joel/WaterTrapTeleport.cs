using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaterTrapTeleport : MonoBehaviour
{
    //public GameObject interactiveBtn;       // Referencia al Boton de interacción
    //public bool activateInteract;           // Booleana de si esta activada el boton de interaccion.
    private GameObject currenTeleporter;    // Teleport actual con el interactuas
    public Transform destination;           // Referencia al teleport de destino
    public GameObject player;     
    public float waterTrapDamage = 10f;          // Jugador
    public AudioClip trapDamageSound; // Sonido específico de esta trampa

    //public Image fadeImage;                 // Imagen de la pantalla para el fade in/out
    public float coolDown = 1.5f;           // Cooldown FadeInOut del teleport
    //public Day_Night dayNightScript; 
    //public AudioManager audioManager;       // Referencia al script de Day_Night

    private PlayerManager playerManager;
    private PlayerMovement playerMovement;

    void Start()
    {
        if (playerManager == null)
        {
            playerManager = GameObject.FindWithTag("Player").GetComponent<PlayerManager>();
        }
        if (playerMovement == null)
        {
            playerMovement = GameObject.FindWithTag("Player").GetComponent<PlayerMovement>();
        }
        playerMovement.damageAudioSource.clip = trapDamageSound;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerMovement.damageAudioSource.clip = trapDamageSound;
            //interactiveBtn.SetActive(true);
            //activateInteract = true;
            playerManager.health.Damage(waterTrapDamage);
            currenTeleporter = other.gameObject;
            //StartCoroutine(dayNightScript.FadeInOut());
            Invoke ("Destination",coolDown);
            //Debug.Log("Ha entrado");
            playerMovement.damageAudioSource.Play();

        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.gameObject == currenTeleporter)
            {
                //interactiveBtn.SetActive(false);
                //activateInteract = false;
                currenTeleporter = null;
                //Debug.Log("Ha salido");
                
            }
        }
    }

    public Transform GetDestination()
    {
        return destination;
    }

    public void Destination ()
    {
        player.transform.position = GetDestination().position;
    }
}
