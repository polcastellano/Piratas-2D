using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NpcInfo : MonoBehaviour
{
    public string nameNpc; // Nombre del npc
    public bool activateInteract; // Boton interactuar
    private Dialogue dialogue; // Referencia a script dialogue
    public GameObject interactiveBtn; // Referencia al Boton de interacción
    private PlayerManager player; // Referencia al PlayerManager
    public Image faceImage; // Panel  donde va la imagen en dialogos
    public string faceImageSourceName; // Nombre de la imagen que mostrar en el panel de dialogo
    private Coroutine deactivatePanelCoroutine;
    public bool noEntrar; // Boolena que se activa cuando no se quiere a entrar a uno de los if.

    // AUDIO
    public bool isAudioPlaying = false;
    public AudioClip[] dialogueClips; // Array donde se almacenan las voces de los diferentes dialogos
    private AudioManager audioManager; // Referencia al audioManager
    private AudioSource audioSource; // AudioSource local para el sonido 3D del npc

    // FRASES DEL NPC
    public string dialog; // Frase normal
    public string askingQuestion1; // Respuesta pregunta 1
    public string askingQuestion2; // Respuesta pregunta 2
    public string askingQuestion3; // Respuesta pregunta 3
    public string dialogCancel; // Frase si no quieres ninguna respuesta

    void Start()
    {
        // Encuentra automaticamente al objeto PlayerManager en la escena y lo asigna a player
        if (player == null)
        {
            player = GameObject.FindWithTag("Player").GetComponent<PlayerManager>();
        }
        if(dialogue == null) // Encuentra el DataManager de forma dinamica si no esta declarado
        {
            dialogue = FindObjectOfType<Dialogue>();
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
    
    }

    void Update()
    {
        //  if (isAudioPlaying && !audioSource.isPlaying)
        //  {
        //     Debug.Log("Desactivar panel y isAudioPlaying = false");
        //     isAudioPlaying = false;
        //     dialogue.panel.SetActive(false);
        //     dialogue.ClearButtons();
        //  } 

        if (activateInteract && (Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.JoystickButton3)))
        {

            if (gameObject != null)
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
                    string[] opciones = { "¿Como busco tripulantes?", "¿Que puedo hacer aqui?", "¿La Maldición?", "No tengo más preguntas" };
                    System.Action<int>[] acciones = {
                        (index) => question1(),
                        (index) => question2(),
                        (index) => question3(),
                        (index) => noQuestion()
                    };
                    dialogue.ShowOptions(dialog, nameNpc, opciones, acciones);
                    PlayDialogueClip(0);
                } else {
                    noEntrar = false;
                }
            } 
        }
    }

    private void question1() 
    {
        //PlayDialogueClip(1);
        dialogue.panel.SetActive(true);
        string[] opciones = { "¿Como busco tripulantes?", "¿Que puedo hacer aqui?", "¿La Maldición?", "No tengo más preguntas" };
        System.Action<int>[] acciones = {
            (index) => question1(),
            (index) => question2(),
            (index) => question3(),
            (index) => noQuestion()
        };
        dialogue.ShowOptions(askingQuestion1, nameNpc, opciones, acciones);
        PlayDialogueClip(1);
        noEntrar = true;
    }
    private void question2()
    {
        //PlayDialogueClip(2);
        dialogue.panel.SetActive(true);
        string[] opciones = { "¿Como busco tripulantes?", "¿Que puedo hacer aqui?", "¿La Maldición?", "No tengo más preguntas" };
        System.Action<int>[] acciones = {
            (index) => question1(),
            (index) => question2(),
            (index) => question3(),
            (index) => noQuestion()
        };
        dialogue.ShowOptions(askingQuestion2, nameNpc, opciones, acciones);
        PlayDialogueClip(2);
        noEntrar = true;
    }
    private void question3() 
    {
        //PlayDialogueClip(3);
        dialogue.panel.SetActive(true);
        string[] opciones = { "¿Como busco tripulantes?", "¿Que puedo hacer aqui?", "¿La Maldición?", "No tengo más preguntas" };
        System.Action<int>[] acciones = {
            (index) => question1(),
            (index) => question2(),
            (index) => question3(),
            (index) => noQuestion()
        };
        dialogue.ShowOptions(askingQuestion3, nameNpc, opciones, acciones);
        PlayDialogueClip(3);
        noEntrar = true;
    }
    private void noQuestion() 
    {
        //PlayDialogueClip(4);
        dialogue.panel.SetActive(true);
        dialogue.StartDialogue(dialogCancel, nameNpc);
        noEntrar = true;
        //Dialogue.isDialogueOptionsActive = true;
        StartCoroutine(DeactivatePanelAfterDelay());
        PlayDialogueClip(4);
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

    private void PlayDialogueClip(int index)
    {
        if (index >= 0 && index < dialogueClips.Length)
        {
            audioSource.clip = dialogueClips[index];
            audioSource.Play();
            //isAudioPlaying = true;
        }
    }
}