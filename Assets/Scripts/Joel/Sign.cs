using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Sign : MonoBehaviour
{
    public string signName = "Cartel - Ubicación"; // Nombre del Cartel
    public string signText; // Texto del Cartel
    private Dialogue dialogue; // Referencia al script de dialogos
    public bool activateInteract; // Bool para activar/desactivar btnInteractivo
    public GameObject interactiveBtn; // Boton interactivo
    public Image dialogImage; // Imagen del panel de dialogos
    public string signImageSourceName; // Nombre de la imagen del cartel

    // Start is called before the first frame update
    void Start()
    {
        if (dialogue == null)
        {
            dialogue = FindObjectOfType<Dialogue>(); 
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (activateInteract && (Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.JoystickButton3)))
        {
            if (gameObject.tag == "Sign")
            {             
                Sprite loadedSprite = Resources.Load<Sprite>("Images/HUD/" + signImageSourceName);
                if (loadedSprite != null)
                {
                    dialogImage.sprite = loadedSprite;
                }
                else
                {
                    Debug.LogWarning("No se encontró la imagen en Resources/Images/HUD/" + signImageSourceName);
                }

                dialogue.panel.SetActive(true);
                dialogue.StartDialogueInstant(signText, signName);
            }
        }
    }
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            //Debug.Log("Jugador entra al cartel");
            interactiveBtn.SetActive(true);
            activateInteract = true;

            GameObject infBtn = interactiveBtn.transform.GetChild(2).gameObject;
            infBtn.SetActive(true);

        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            //Debug.Log("Jugador sale del cartel");
            interactiveBtn.SetActive(false);
            activateInteract = false;
            
            GameObject infBtn = interactiveBtn.transform.GetChild(2).gameObject;
            infBtn.SetActive(false);
            dialogue.panel.SetActive(false);
        }
    }
}
