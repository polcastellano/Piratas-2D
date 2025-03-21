using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NpcRandomDices : MonoBehaviour
{
    public string crewMemberName;
    public Dialogue dialogue;
    public bool activateInteract;
    public GameObject interactiveBtn;
    public GameObject miniMap;
    public GameObject leftHudBlock;

    private PlayerManager player;
    public Image faceImage;
    public string faceImageSourceName;

    private Coroutine deactivatePanelCoroutine;
    private bool fadeOut;
    public int timeAfterDesactive;

    public bool isAudioPlaying = false;

    public FadeCrewMembers fadeController;
    public bool noEntrar; // Evita entrar al if en bucle para evitar problemas al pulsar con el boton de accion un boton

    // FRASES DEL NPC
    private string ignoringPhrase;
    public string askingGameQuestion;
    public string declinePhrase;
    public string noMoneyPhrase;
    public int diceGamePrice;

    // Panel de los dados
    public GameObject dicePanel;

    // Audio
    public AudioClip[] dialogueClips;
    public AudioManager audioManager;
    private AudioSource audioSource; // AudioSource local para el sonido en 3D

    void Start()
    {
        if (player == null)
        {
            //player = FindObjectOfType<PlayerManager>();
            player = GameObject.FindWithTag("Player").GetComponent<PlayerManager>();
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
        if (activateInteract && (Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.JoystickButton3)))
        {
            if(noEntrar == false) {
                if (gameObject.tag == "NpcDices")
                {
                    /* ------------------------------------------------- */
                    // Bloque para cargar la FaceImage en el panel de dialogos
                    Sprite loadedSprite = Resources.Load<Sprite>("Images/HUD/CarasPersonajes/" + faceImageSourceName);
                    if (loadedSprite != null)
                    {
                        faceImage.sprite = loadedSprite;
                    }
                    else
                    {
                        Debug.LogWarning("No se encontró la imagen en Resources/Images/HUD/CarasPersonajes/" + faceImageSourceName);
                    }
                    /* ------------------------------------------------- */
                    dialogue.panel.SetActive(true);
                    dialogue.StartDialogue(askingGameQuestion, crewMemberName);

                    string[] opciones = { "Jugar", "No me apetece" };
                    System.Action<int>[] acciones = {
                        (index) => acceptMission(),
                        (index) => declineMission()
                    };

                    dialogue.ShowOptions(askingGameQuestion, crewMemberName, opciones, acciones);
                    PlayDialogueClip(0);
                }
            } else {
                noEntrar = false;
            }
        }

        if (fadeOut)
        {
            dialogue.panel.SetActive(false);
        }
    }

    private void acceptMission() 
    {
        noEntrar = true;
        dialogue.ClearButtons();
        
        if (player.coins.actualCoins >= diceGamePrice)
        {
            // Si hay monedas, mostramos el tablero de los dados
            dialogue.panel.SetActive(false);
            miniMap.SetActive(false);
            leftHudBlock.SetActive(false);
            dicePanel.SetActive(true);
        }
        else
        {
            // Si no hay monedas, mandamos a la mierda
            dialogue.panel.SetActive(true);
            dialogue.StartDialogue(noMoneyPhrase, crewMemberName);
            PlayDialogueClip(2);
            StartCoroutine(DeactivatePanelAfterDelay());
        }
    }

    private void declineMission() 
    {
        noEntrar = true;
        dialogue.panel.SetActive(true);
        dialogue.StartDialogue(declinePhrase, crewMemberName);
        PlayDialogueClip(1);
        StartCoroutine(DeactivatePanelAfterDelay());
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            interactiveBtn.SetActive(true);
            activateInteract = true;

            GameObject infBtn = interactiveBtn.transform.GetChild(0).gameObject;
            infBtn.SetActive(true);
            
            if(fadeOut)
            {
                dialogue.panel.SetActive(false);
            } 
            // else if (isAudioPlaying && !fadeOut)
            // {
            //     dialogue.panel.SetActive(true);
            // }
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