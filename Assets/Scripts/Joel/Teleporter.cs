using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Teleporter : MonoBehaviour
{
    public GameObject interactiveBtn;       // Referencia al Boton de interacción
    public bool activateInteract;           // Booleana de si esta activada el boton de interaccion.
    private GameObject currenTeleporter;    // Teleport actual con el interactuas
    public Transform destination;           // Referencia al teleport de destino
    public GameObject player;               // Jugador
    
    public Image fadeImage;                 // Imagen de la pantalla para el fade in/out
    public float coolDown = 1.5f;           // Cooldown FadeInOut del teleport
    public Day_Night dayNightScript; 
    public AudioManager audioManager;       // Referencia al script de Day_Night

    // Update is called once per frame
    void Update()
    {
        // Si el jugador pulsa "E" comienza el teleport, ejecuta el FadeInOut y justo cuando la pantalla esta en negro hace el teleport.
        if (activateInteract && (Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.JoystickButton3)))
        {
            StartCoroutine(dayNightScript.FadeInOut());
            
            Invoke ("Destination",coolDown);
            audioManager.PlaySfx(4);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            interactiveBtn.SetActive(true);
            activateInteract = true;

            GameObject infBtn = interactiveBtn.transform.GetChild(2).gameObject;
            infBtn.SetActive(true);

            currenTeleporter = other.gameObject;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.gameObject == currenTeleporter)
            {
                interactiveBtn.SetActive(false);
                activateInteract = false;

                GameObject infBtn = interactiveBtn.transform.GetChild(2).gameObject;
                infBtn.SetActive(false);

                currenTeleporter = null;
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