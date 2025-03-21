using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CrewMember : MonoBehaviour
{
    public string crewMemberName; // Nombre tripulante
    public string dialog; // Primer dialogo
    public string congrats; // Al entregarle el objeto
    public string dialogMission; // Dialogo de misión aceptada

    public string missionItem; // Objeto de misión
    public int crewMemberID; // ID del tripulante
    public bool isRecruited; // Esta reclutado?
    public bool missionAccepted; // Has aceptado la mision?
    public bool noEntrar; // Boolena que se activa cuando no se quiere a entrar a uno de los if.

    public GameObject missionObject;
    public GameObject crewMemberInShip;
    public Dialogue dialogue;
    public bool activateInteract;
    public GameObject interactiveBtn;
    public ItemsList itemsList;
    public RecruitedMembers recruitedMembers;
    private PlayerManager player;
    public Image faceImage;
    public string faceImageSourceName;
    public AudioClip[] dialogueClips;
    public AudioManager audioManager;
    private AudioSource audioSource; // AudioSource local para el sonido en 3D
    private Coroutine deactivatePanelCoroutine;
    private bool fadeOut;
    public int timeAfterDesactive;

    public bool isAudioPlaying = false; // Esta el audio sonando?
    public bool dayTime; // ¿Cuando aparece? True (1) = Dia / False (0) = Noche
    public bool allDay; // Si esta marcada significa que aparece de dia y noche

    private DataManager dataManager;
    private PrisonDoor prisonDoor;
    private List<int> listRecruitedCrewMembers = new List<int>();
    private List<int> acceptedMissionCrewMembers = new List<int>();
    public FadeCrewMembers fadeController; // NUEVO: Referencia al script de fade
    private bool objectCollected; // Booleana que si has recogido el objeto es true 

    /* public string[] crewMembersIgnoringPhrases = new string[] {
        "Anda, vete a molestar a otro.",
        "¿No tienes algo mejor que hacer?",
        "Mejor vete, no tengo tiempo para ti.",
        "¡Lárgate de aquí!",
        "¿Por qué no hablas con el marinero y me dejas en paz?"
    }; */

    public string[] crewMembersIgnoringPhrases = new string[] {
        "¡Largo! Habla con el hombre del muelle.",
        "No eres mi problema. Prueba suerte cerca del muelle.",
        "!No puedo ayudarte! ¡Busca en el puerto!",
        "¡Fuera! Pregunta en el muelle.",
        "¡No estoy para esto! Ve al muelle.",
        "¡Déjame en paz! Busca en el muelle."
    };

    public AudioClip[] crewMembersIgnoringDialogues;
    
    private string ignoringPhrase;

    void Start()
    {
        // Encuentra autom�ticamente al objeto PlayerManager en la escena y lo asigna a player
        if (player == null)
        {
            //player = FindObjectOfType<PlayerManager>();
            player = GameObject.FindWithTag("Player").GetComponent<PlayerManager>();
        }
        missionAccepted = false;
        isRecruited = false;

        if (dataManager == null)
        {
            dataManager = FindObjectOfType<DataManager>();
        }
        if (prisonDoor == null)
        {
            prisonDoor = FindObjectOfType<PrisonDoor>();
        }

        audioSource = GetComponent<AudioSource>(); // Obtiene el audioSource del CrewMember
        audioSource.minDistance = 5.0f; // Ajusta este valor al radio en el que quieres que el sonido se escuche alto
        audioSource.maxDistance = 20.0f; // Ajusta este valor a la distancia máxima en la que se escuche el sonido
        audioSource.rolloffMode = AudioRolloffMode.Linear; // Da mas precision al 3D y asi evitamos que si te alejes mucho se siga escuchando
        audioSource.spatialBlend = 1.0f; // Configura el audio en modo 3D

        if(dataManager != null) {
            listRecruitedCrewMembers = dataManager.playerData.collectedCrewMembers;
            acceptedMissionCrewMembers = dataManager.playerData.activeMissionCrewMembers;

            if(dataManager.loadGame == true) {
                foreach (int crewMember in listRecruitedCrewMembers)
                {
                    if (crewMember == crewMemberID)
                    {
                        Debug.Log("Reclutado tripulante: " + crewMember);
                        if(isRecruited == false) {
                            recruitedMembers.totalCrewMembers++;
                            player.objective.sumCount();
                        }
                        isRecruited = true;
                        crewMemberInShip.SetActive(true);
                        gameObject.SetActive(false);
                    }
                }
                foreach (int crewMember in acceptedMissionCrewMembers)
                {
                    if (crewMember == crewMemberID)
                    {
                        missionAccepted = true;
                        missionObject.SetActive(true);
                        //player.pistas.ActivarPista(crewMemberID);
                        Debug.Log("Tripulante con mision aceptada, activa objeto misión y missionAccepted. ID:" + crewMemberID);
                    }
                }
            }
            
        }
        
        //Debug.Log("TRIPULANTES RECLUTADOS: " + listRecruitedCrewMembers);
        
        
        // Rereferencia al script de fade-out en el mismo GameObject
        fadeController = GetComponent<FadeCrewMembers>();
    }

    void Update()
    {
        // if (isAudioPlaying && !audioSource.isPlaying)
        // {
        //     //Debug.Log("Desactivar panel y isAudioPlaying = false");
        //     isAudioPlaying = false;
        //     dialogue.panel.SetActive(false);
        //     dialogue.ClearButtons();
        // }

        if (activateInteract && (Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.JoystickButton3)))
        {
            if(noEntrar == false) { // Entra si la booleana noEntrar es false.
                if (gameObject.tag == "CrewMember")
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

                    }
                    // Comprobamos si se ha hablado con el businessman
                    if(Businessman.businessmanIsActive == false)
                    {
                        switch (crewMemberName)
                        {
                            case "William":
                                PlayIgnoringPhrasesClips(0);
                                dialogue.panel.SetActive(true);
                                dialogue.StartDialogue(crewMembersIgnoringPhrases[0], crewMemberName);
                                StartCoroutine(DeactivatePanelAfterDelay());
                            break;
                            case "Henry":
                                PlayIgnoringPhrasesClips(1);
                                dialogue.panel.SetActive(true);
                                dialogue.StartDialogue(crewMembersIgnoringPhrases[1], crewMemberName);
                                StartCoroutine(DeactivatePanelAfterDelay());
                            break;
                            case "Edward":
                                PlayIgnoringPhrasesClips(2);
                                dialogue.panel.SetActive(true);
                                dialogue.StartDialogue(crewMembersIgnoringPhrases[2], crewMemberName);
                                StartCoroutine(DeactivatePanelAfterDelay());
                            break;
                            case "Samuel":
                                PlayIgnoringPhrasesClips(3);
                                dialogue.panel.SetActive(true);
                                dialogue.StartDialogue(crewMembersIgnoringPhrases[3], crewMemberName);
                                StartCoroutine(DeactivatePanelAfterDelay());
                            break;
                            case "Joseph":
                                PlayIgnoringPhrasesClips(4);
                                dialogue.panel.SetActive(true);
                                dialogue.StartDialogue(crewMembersIgnoringPhrases[4], crewMemberName);
                                StartCoroutine(DeactivatePanelAfterDelay());
                            break;
                            case "Nathaniel":
                                PlayIgnoringPhrasesClips(5);
                                dialogue.panel.SetActive(true);
                                dialogue.StartDialogue(crewMembersIgnoringPhrases[5], crewMemberName);
                                StartCoroutine(DeactivatePanelAfterDelay());
                            break;
                        }
                        /* RandomPhrase(crewMembersIgnoringPhrases);
                        dialogue.panel.SetActive(true);
                        dialogue.StartDialogue(ignoringPhrase, crewMemberName);
                        StartCoroutine(DeactivatePanelAfterDelay()); */
                    }
                    else
                    {
                        dialogue.panel.SetActive(true);

                        Debug.Log("Panel activado");
                        if (missionAccepted == false)
                        {
                            PlayDialogueClip(0);
                            string[] opciones = { "Aceptar misión", "Ahora no" };
                            System.Action<int>[] acciones = {
                                (index) => aceptarMision(),
                                (index) => volver()
                            };
                            dialogue.ShowOptions(dialog, crewMemberName, opciones, acciones);
                        }
                        else if (missionAccepted == true) //&& itemsList.missionObjectsList.Count > 0)
                        {
                            objectCollected = itemsList.findObjectInList(missionItem); // ObjectCollected obtiene el valor de true o false en base a si ha cogido el objeto el jugador o no
                            if (objectCollected == true)
                            {  
                                if(crewMemberID == 1 && prisonDoor != null) // Si es el prisionero activa la puerta de la prision
                                {
                                    Debug.Log("Abrir puerta prisión");
                                    prisonDoor.ActivateAction();
                                }
                                player.audioManager.PlaySfx(10);
                                PlayDialogueClip(1);
                                dialogue.StartDialogue(congrats,crewMemberName);
                                if(isRecruited == false) {
                                    recruitedMembers.totalCrewMembers++;
                                    player.objective.sumCount(); // Suma 1 al contador de objetivos actuales en el HUD
                                }
                                isRecruited = true;
                                player.objective.missionComplete();
                                player.pistas.CompletarPista(crewMemberID);
                                crewMemberInShip.SetActive(true);
                                StartCoroutine(DeactivateAfterDelay()); // Llama a la funcion de desactivar el NPC con delay
                                if(dataManager != null) {
                                    dataManager.playerData.collectedCrewMembers.Add(crewMemberID);
                                    dataManager.playerData.activeMissionCrewMembers.Remove(crewMemberID);
                                    dataManager.playerData.collectedItems.Remove(crewMemberID);
                                    player.objects.EntregarObjeto(crewMemberID);
                                    dataManager.SaveGameNpcTp();
                                }
                            } else { 
                                PlayDialogueClip(0);
                                dialogue.StartDialogue(dialog,crewMemberName);
                                Debug.Log("Else del CrewMember");
                            }
                        }
                    }
                } 
            } else {
                noEntrar = false; // Vuelve esta booleana a false para que si el jugador vuelve a hablar con el tripulante repita el dialogo
            }

        }
        if (fadeOut)
        {
            dialogue.panel.SetActive(false);
        }


    }
    private void aceptarMision() 
    {        
        player.objective.missionNew();
        player.pistas.ActivarPista(crewMemberID);
        missionAccepted = true;
        noEntrar = true; // Se marca esta booleana para evitar que se repita el dialogo en el update.
        missionObject.SetActive(true);
        Debug.Log("Mision aceptada, mostrando objeto de mision. ID: " + crewMemberID);
        dialogue.ClearButtons();
        audioSource.Stop();
        
        // Dialogo y audio al aceptar la misión
        PlayDialogueClip(2);
        dialogue.panel.SetActive(true);
        dialogue.StartDialogue(dialogMission,crewMemberName);
        Dialogue.isDialogueOptionsActive = true;

        if(dataManager != null) {
            dataManager.playerData.activeMissionCrewMembers.Add(crewMemberID);
            dataManager.SaveGameNpcTp();
        }

    }

    private void volver()
    {
        Dialogue.isDialogueOptionsActive = true;
        Debug.Log("Volver isDialogueOptionsActive: " + Dialogue.isDialogueOptionsActive);
        noEntrar = true;
        dialogue.panel.SetActive(false);
        dialogue.ClearButtons();
        audioSource.Stop();
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

    private void PlayIgnoringPhrasesClips(int index)
    {
        if (index >= 0 && index < crewMembersIgnoringDialogues.Length)
        {
            audioSource.clip = crewMembersIgnoringDialogues[index];
            audioSource.Play();
            //isAudioPlaying = true;
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            interactiveBtn.SetActive(true);
            activateInteract = true;

            /* if(itemsList.findObjectInList(missionItem) == true)
            {
                GameObject infBtn = interactiveBtn.transform.GetChild(1).gameObject;
                infBtn.SetActive(true);
            }
            else 
            {
                GameObject infBtn = interactiveBtn.transform.GetChild(0).gameObject;
                infBtn.SetActive(true);
            } */

            if(/* acceptedMissionCrewMembers[crewMemberID] == crewMemberID */ acceptedMissionCrewMembers.Contains(crewMemberID))
            {
                GameObject infBtn = interactiveBtn.transform.GetChild(1).gameObject;
                infBtn.SetActive(true);
            }
            else 
            {
                GameObject infBtn = interactiveBtn.transform.GetChild(0).gameObject;
                infBtn.SetActive(true);
            }

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

            /* if(itemsList.findObjectInList(missionItem) == true)
            {
                GameObject infBtn = interactiveBtn.transform.GetChild(1).gameObject;
                infBtn.SetActive(false);
            }
            else 
            {
                GameObject infBtn = interactiveBtn.transform.GetChild(0).gameObject;
                infBtn.SetActive(false);
            } */

            if(/* acceptedMissionCrewMembers[crewMemberID] == crewMemberID */ acceptedMissionCrewMembers.Contains(crewMemberID))
            {
                GameObject infBtn = interactiveBtn.transform.GetChild(1).gameObject;
                infBtn.SetActive(false);
            }
            else 
            {
                GameObject infBtn = interactiveBtn.transform.GetChild(0).gameObject;
                infBtn.SetActive(false);
            }

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
        audioSource.Stop();
    }

    // Corrutina para esperar 5 segundos antes de desactivar el gameObject
    private IEnumerator DeactivateAfterDelay()
    {
        yield return new WaitForSeconds(timeAfterDesactive);
        fadeController.enabled = true;
        fadeOut = true;
        yield return new WaitForSeconds(6); // Espera de 5 segundos
        gameObject.SetActive(false);        // Desactiva el gameObject
        
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
}