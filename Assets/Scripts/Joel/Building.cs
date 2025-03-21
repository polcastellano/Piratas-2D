using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.TextCore.Text;

public class Building : MonoBehaviour
{
    public string buildingName; // String nombre del edificio
    public string buildingQuestion; // String de la pregunta del edificio
    public string buildingDialog; // String con el dialogo del edificio
    public string buildingCancel; // String que mostrar al cancelar
    public string noMoney; // String que mostrar si no tienes dinero
    public int price; // Precio del producto
    public int ronQuantity; // Cantidad de ron bebida
    public int HealQuantity; // Cantidad de vida a restaurar
    public Image faceImage; // Imagen de la cara del panel de dialogos
    public string tavernFaceImage; // Nombre de la imagen del tabernero
    public string hostelFaceImage; // Nombre de la imagen del hostalero
    public Dialogue dialogue; // Referencia a Dialogue
    public bool activateInteract; // Booleana de interacción
    public GameObject interactiveBtn; // Boton de interacción
    public PlayerManager player; // Referencia al PlayerManager
    public Day_Night dayNightScript;  // Referencia al script Day_Night
    public DataManager dataManager; // Referencia al DataManager
    public bool noEntrar; // Booleana para no entrar a un if si es true.

    public AudioClip[] dialogueClips;
    public AudioSource audioSource;
    public AudioSource audioSourcePay;
    public bool isAudioPlaying = false;
    public static bool buildingPanelActive;

    void Start()
    {
        if(dataManager == null) // Encuentra el DataManager de forma dinamica si no esta declarado
        {
            dataManager = FindObjectOfType<DataManager>();
        }

        audioSource = GetComponent<AudioSource>(); // Obtiene el audioSource del CrewMember
        audioSource.minDistance = 5.0f; // Ajusta este valor al radio en el que quieres que el sonido se escuche alto
        audioSource.maxDistance = 20.0f; // Ajusta este valor a la distancia máxima en la que se escuche el sonido
        audioSource.rolloffMode = AudioRolloffMode.Linear; // Da mas precision al 3D y asi evitamos que si te alejes mucho se siga escuchando
        audioSource.spatialBlend = 1.0f; // Configura el audio en modo 3D

    }

    // Update is called once per frame
    void Update()
    {
        if (activateInteract && (Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.JoystickButton3)))
        {
            if(noEntrar == false) {
                initialDialog();
            } else {
                noEntrar = false;
            }
        }

    }
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            interactiveBtn.SetActive(true);
            activateInteract = true;

            GameObject infBtn = interactiveBtn.transform.GetChild(0).gameObject;
            infBtn.SetActive(true);
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            interactiveBtn.SetActive(false);
            activateInteract = false;

            GameObject infBtn = interactiveBtn.transform.GetChild(0).gameObject;
            infBtn.SetActive(false);
            
            dialogue.panel.SetActive(false);
            buildingPanelActive = false;
            dialogue.ClearButtons();
        }
    }

    // Primer dialogo del edificio al interactuar
    private void initialDialog() {
        if (gameObject.tag == "Building")
        {
            dialogue.panel.SetActive(true);
            buildingPanelActive = true;
            if (buildingName == "Patty")
            {
                Sprite loadedSprite = Resources.Load<Sprite>("Images/HUD/CarasPersonajes/" + tavernFaceImage);
                faceImage.sprite = loadedSprite;

                string[] opciones = { "¿Que sirves?", "Comprar Ron por " + price + "$ oro", "Ahora no" };
                    System.Action<int>[] acciones = {
                        (index) => askQuestion(),
                        (index) => PurchaseRon(),
                        (index) => cancelOption() //Debug.Log($"Seleccionaste la opción {index}: Cancelar")
                    };
                dialogue.ShowOptions(buildingDialog, buildingName, opciones, acciones);

                PlayDialogueClip(0);
            }
            else if (buildingName == "Terry")
            {
                Sprite loadedSprite = Resources.Load<Sprite>("Images/HUD/CarasPersonajes/" + hostelFaceImage);
                faceImage.sprite = loadedSprite;

                string[] opciones = { "¿Puedo dormir aqui?", "Dormir (" + price + " oro)", "Aún no" };
                    System.Action<int>[] acciones = {
                        (index) => askQuestion(),
                        (index) => sleepHostal(),
                        (index) => cancelOption()
                    };
                dialogue.ShowOptions(buildingDialog, buildingName, opciones, acciones);

                PlayDialogueClip(0);
            }
        }
    }

    // Dialogo de la pregunta
    private void askQuestion()
    {
        noEntrar = true;
        buildingPanelActive = true;
        dialogue.panel.SetActive(true);

        if (buildingName == "Patty")
        {
            Sprite loadedSprite = Resources.Load<Sprite>("Images/HUD/CarasPersonajes/" + tavernFaceImage);
            faceImage.sprite = loadedSprite;

            string[] opciones = {"Beber Ron (" + price + " oro)", "Ahora no" };
                System.Action<int>[] acciones = {
                    (index) => PurchaseRon(),
                    (index) => cancelOption() //Debug.Log($"Seleccionaste la opción {index}: Cancelar")
                };
            dialogue.ShowOptions(buildingQuestion, buildingName, opciones, acciones);
            PlayDialogueClip(1);
        }
        else if(buildingName == "Terry")
        {   
            Sprite loadedSprite = Resources.Load<Sprite>("Images/HUD/CarasPersonajes/" + hostelFaceImage);
            faceImage.sprite = loadedSprite;

            string[] opciones = {"Dormir (" + price + " oro)", "Aún no" };
                System.Action<int>[] acciones = {
                    (index) => sleepHostal(),
                    (index) => cancelOption() //Debug.Log($"Seleccionaste la opción {index}: Cancelar")
                };
            dialogue.ShowOptions(buildingQuestion, buildingName, opciones, acciones);
            PlayDialogueClip(1);            
        } 
        else 
        {
            Debug.Log("El BuildingName no esta bien definido y no contiene un AskQuestion para este Building");
            dialogue.panel.SetActive(false);
            dialogue.ClearButtons();
            buildingPanelActive = false;
        }
    } 
    // Dialogo de cancelar
    private void cancelOption()
    {
        noEntrar = true;
        if (buildingName == "Patty")
        {
            dialogue.StartDialogue(buildingCancel, buildingName);
            dialogue.panel.SetActive(true);
            buildingPanelActive = true;
            PlayDialogueClip(2);
        }
        else if(buildingName == "Terry")
        {   
            dialogue.StartDialogue(buildingCancel, buildingName);
            dialogue.panel.SetActive(true);
            buildingPanelActive = true;
            PlayDialogueClip(2);
        } 
        else 
        {
            Debug.Log("El BuildingName no esta bien definido y no contiene un stringCancel para este Building");
            dialogue.panel.SetActive(false);
            dialogue.ClearButtons();
            buildingPanelActive = false;
        }
    } 
    // Dialogo de comprar ron
    private void PurchaseRon()
    {
        noEntrar = true;
        if (buildingName == "Patty")
        {
            if (player.coins.actualCoins >= price)
            {
                player.coins.spendCoins(price); // Coste de monedas por beber
                player.ron.DrinkRon(ronQuantity); // Beber la cantidad especificada de ron
                audioSourcePay.Play();
                PlayDialogueClip(4);
            }
            else
            {
                dialogue.StartDialogue(noMoney, buildingName);
                dialogue.panel.SetActive(true);
                buildingPanelActive = true;
                Sprite loadedSprite = Resources.Load<Sprite>("Images/HUD/CarasPersonajes/" + tavernFaceImage);
                faceImage.sprite = loadedSprite;
                PlayDialogueClip(3);
            }
        }
    }

// Funcion de dormir y pasar la noche
    private void sleepHostal()
    {
        noEntrar = true;
        if (buildingName == "Terry")
        {
            if (player.coins.actualCoins >= price)
            {
                player.coins.spendCoins(price); // Coste de monedas por dormir
                player.health.Heal(HealQuantity); // Restaurar salud completamente
                player.ron.DrinkRon(-100); // Bajar cantidad de ron a 0

                audioSourcePay.Play();
                StartCoroutine(dayNightScript.DayNightTransition()); // Llama a la corutina en Day_Night
                if(dataManager != null) {
                    dataManager.SaveGame();
                }
                PlayDialogueClip(4);
            }
            else
            {
                dialogue.StartDialogue(noMoney, buildingName);
                dialogue.panel.SetActive(true);
                buildingPanelActive = true;
                Sprite loadedSprite = Resources.Load<Sprite>("Images/HUD/CarasPersonajes/" + hostelFaceImage);
                faceImage.sprite = loadedSprite;
                PlayDialogueClip(3);
            }
        }
    }
    // Funcion para guardar partida
    private void saveGame() {
        StartCoroutine(dayNightScript.DayNightTransition()); // Llama a la corutina en Day_Night
        if(dataManager != null) {
            dataManager.SaveGame();
        }
    }
    private void PlayDialogueClip(int index)
    {
        if (index >= 0 && index < dialogueClips.Length)
        {
            audioSource.clip = dialogueClips[index];
            audioSource.Play();
            isAudioPlaying = true;
        }
    }
}
