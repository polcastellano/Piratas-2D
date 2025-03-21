using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NpcTeleport : MonoBehaviour
{
    public string crewMemberName;
    public bool isTalked;   
    public Dialogue dialogue;
    public bool activateInteract;
    public GameObject interactiveBtn;
    private PlayerManager player;
    public Image faceImage;
    public string faceImageSourceName;
    public AudioClip[] dialogueClips;
    public AudioManager audioManager;
    private AudioSource audioSource; // AudioSource local para el sonido en 3D
    private Coroutine deactivatePanelCoroutine;
    private bool fadeOut;
    public int timeAfterDesactive;
    public DataManager dataManager;

    public bool isAudioPlaying = false;

    public FadeCrewMembers fadeController; // NUEVO: Referencia al script de fade

    // FRASES DEL NPC
    private string ignoringPhrase;
    public string askingTpQuestion;
    public string declinePhrase;
    public string noMoneyPhrase;
    public int teleportPrice;

    // VARIABLES PARA EL FUNCIONAMIENTO DEL TP
    private Transform destination; // Desinto del tp
    public Transform ubi_1;
    public Transform ubi_2;
    public string ubiName_1;
    public string ubiName_2;
    public GameObject playerObject;  // Referencia al jugador
    public Image fadeImage; // Imagen de la pantalla para el fade in/out
    public float coolDown = 1.5f;
    public Day_Night dayNightScript; 
    public bool noEntrar; // Boolena que se activa cuando no se quiere a entrar a uno de los if.




    void Start()
    {
        // Encuentra automaticamente al objeto PlayerManager en la escena y lo asigna a player
        if (player == null)
        {
            player = GameObject.FindWithTag("Player").GetComponent<PlayerManager>();
        }
        isTalked = false;

        if(dataManager == null) // Encuentra el DataManager de forma dinamica si no esta declarado
        {
            dataManager = FindObjectOfType<DataManager>();
        }
        if(audioManager == null) // Encuentra el DataManager de forma dinamica si no esta declarado
        {
            audioManager = FindObjectOfType<AudioManager>();
        }
        audioSource = GetComponent<AudioSource>(); // Obtiene el audioSource del CrewMember
        audioSource.minDistance = 5.0f; // Ajusta este valor al radio en el que quieres que el sonido se escuche alto
        audioSource.maxDistance = 20.0f; // Ajusta este valor a la distancia máxima en la que se escuche el sonido
        audioSource.rolloffMode = AudioRolloffMode.Linear; // Da mas precision al 3D y asi evitamos que si te alejes mucho se siga escuchando
        audioSource.spatialBlend = 1.0f; // Configura el audio en modo 3D

        fadeController = GetComponent<FadeCrewMembers>();

    }

    void Update()
    {
        /* if (isAudioPlaying && !audioSource.isPlaying)
        {
            Debug.Log("Desactivar panel y isAudioPlaying = false");
            isAudioPlaying = false;
            dialogue.panel.SetActive(false);
            dialogue.ClearButtons();
        } */

        if (activateInteract && (Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.JoystickButton3)))
        {
            if (gameObject.tag == "NpcTeleport")
            {
                // Cargar el Sprite desde Resources y asignarlo a faceImage
                Sprite loadedSprite = Resources.Load<Sprite>("Images/HUD/CarasPersonajes/" + faceImageSourceName);
                if (loadedSprite != null)
                {
                    faceImage.sprite = loadedSprite;
                }
                else
                {
                    Debug.LogWarning("No se encontró la imagen en Resources/Images/HUD/CarasPersonajes/" + faceImageSourceName);
                }

                if(noEntrar == false)
                {
                    dialogue.panel.SetActive(true);
                    dialogue.StartDialogue(askingTpQuestion, crewMemberName);

                    string[] opciones = { $"{ubiName_1}", $"{ubiName_2}", "Ahora no" };
                    System.Action<int>[] acciones = {
                        (index) => location1(),
                        (index) => location2(),
                        (index) => declineMission()
                    };
                    dialogue.ShowOptions(askingTpQuestion, crewMemberName, opciones, acciones);
                    PlayDialogueClip(0);
                }   
                else
                {
                    noEntrar = false;
                }
            }

        }
        if (fadeOut)
        {
            dialogue.panel.SetActive(false);
        }

    }

    private void location1() 
    {
        destination = ubi_1;
        if (player.coins.actualCoins >= teleportPrice)
        {
            player.coins.spendCoins(teleportPrice);
            StartCoroutine(DeactivatePanelAfterDelay());
            Invoke ("Destination", coolDown);
            StartCoroutine(dayNightScript.FadeInOut()); 
            PlayDialogueClip(3);
            if(dataManager != null) {
                dataManager.SaveGameNpcTp();
            }   
        }
        else
        {
            dialogue.panel.SetActive(true);
            dialogue.StartDialogue(noMoneyPhrase, crewMemberName);
            StartCoroutine(DeactivatePanelAfterDelay());
            PlayDialogueClip(2);
        }
        noEntrar = true;
    }

    private void location2() 
    {
        destination = ubi_2;
        if (player.coins.actualCoins >= teleportPrice)
        {
            player.coins.spendCoins(teleportPrice);
            StartCoroutine(DeactivatePanelAfterDelay());
            Invoke ("Destination", coolDown);
            StartCoroutine(dayNightScript.FadeInOut()); 
            PlayDialogueClip(3);
            if(dataManager != null) {
                dataManager.SaveGameNpcTp();
            }   
            noEntrar = true;
        }
        else
        {
            dialogue.panel.SetActive(true);
            dialogue.StartDialogue(noMoneyPhrase, crewMemberName);
            StartCoroutine(DeactivatePanelAfterDelay());
            PlayDialogueClip(2);
        }
        noEntrar = true;
    }

    private void declineMission() 
    {
        dialogue.panel.SetActive(true);
        dialogue.StartDialogue(declinePhrase, crewMemberName);
        StartCoroutine(DeactivatePanelAfterDelay());
        PlayDialogueClip(1);
        noEntrar = true;
    }

    private void travel()
    {
        Debug.Log("VIAJANDO");
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
            StartCoroutine(DeactivatePanelAfterDelay());
            // Si ya hay una corrutina ejecutándose, la detenemos para evitar múltiples corrutinas simultáneas
            if (deactivatePanelCoroutine != null)
            {
                StopCoroutine(deactivatePanelCoroutine);
            }
            // Iniciamos la corrutina para desactivar el panel después de 3 segundos
            if(gameObject == null)
            {
                deactivatePanelCoroutine = StartCoroutine(DeactivatePanelAfterDelay());
            }
            

        }
    }

    // Corrutina para esperar 2 segundos antes de desactivar el panel de diálogo
    private IEnumerator DeactivatePanelAfterDelay()
    {
        yield return new WaitForSeconds(2);
        dialogue.panel.SetActive(false); // Desactiva el panel de diálogo a los dos segundos de salir del collider del CrewMember
        dialogue.ClearButtons();
    }

    public void RandomPhrase(string[] phrases)
    {
        int num = Random.Range(0, 5);
        for (int i = 0; i < phrases.Length; i++)
        {
            if(i == num)
            {
                ignoringPhrase = phrases[i];
            }
        }
    }

    public Transform GetDestination()
    {
        return destination;
    }

    public void Destination ()
    {
        playerObject.transform.position = GetDestination().position;
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