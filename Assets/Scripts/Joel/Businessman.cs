using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Businessman : MonoBehaviour
{
    public string businessmanName;
    public Objects objects;
    public Dialogue dialogue;
    public bool activateInteract;
    public string objectiveMission = "Busca tripulantes para embarcar"; // Objetivo de mision actual
    public GameObject interactiveBtn;
    public Ship ship;
    public ItemsList itemsList;
    public PlayerManager player;
    public Image faceImage;
    public string faceImageSourceName;
    public GameObject missionObject;
    public RecruitedMembers recruitedMembers;
    public bool hasCompass;
    public int shipPrice = 20;
    public bool shipObtained;
    public AudioClip[] dialogueClips;
    public AudioManager audioManager;
    public AudioSource audioSource; // AudioSource local para el sonido en 3D
    public DataManager dataManager;
    public string[] businessmanPhrases = new string[] {
        "Como? Que quieres este barco, dices? Cuesta 20 monedas de oro y necesitarás 6 tripulantes!",
        "Arrghh! Te falta una brújula para zarpar!",
        "No hay suficiente dinero!",
        "Excelente! Todo tuyo!"
    };
    public int npcState;
    public bool isAudioPlaying = false;
    public bool activarCharla = false;
    private Coroutine deactivatePanelCoroutine;
    private List<int> listItems = new List<int>();
    // Esta var se tendra que guardar en el DataManager
    public static bool businessmanIsActive = false;
    void Start()
    {
        hasCompass = false;
        //missionObject.SetActive(false);
        if (dataManager == null)
        {
            dataManager = FindObjectOfType<DataManager>();
        }
        if (dataManager != null) {
            npcState = dataManager.playerData.npcState;
            listItems = dataManager.playerData.collectedItems;
            if(dataManager.loadGame == false)
            {
                businessmanIsActive = false;
                npcState = 0;
            }
            if (npcState > 0) {
                businessmanIsActive = true;
            }

            
        }
        
        // Comprueba si tiene la brujula
        foreach (int item in listItems)
        {
            if (item == 6) // 6 = Brujula
            {
                Debug.Log("Brujula cargada");
                hasCompass = true;
            }
        }

        audioSource.minDistance = 5.0f; // Ajusta este valor al radio en el que quieres que el sonido se escuche alto
        audioSource.maxDistance = 20.0f; // Ajusta este valor a la distancia máxima en la que se escuche el sonido
        audioSource.rolloffMode = AudioRolloffMode.Linear; // Da mas precision al 3D y asi evitamos que si te alejes mucho se siga escuchando
        audioSource.spatialBlend = 1.0f; // Configura el audio en modo 3D

    }

    void Update()
    {
        if (isAudioPlaying && !audioSource.isPlaying)
        {
            isAudioPlaying = false;
            dialogue.panel.SetActive(false);
        }
        if (activateInteract && (Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.JoystickButton3)))
        {
            Sprite loadedSprite = Resources.Load<Sprite>("Images/HUD/CarasPersonajes/" + faceImageSourceName);
            faceImage.sprite = loadedSprite;

            switch (npcState)
            {
                case 0:
                    if (recruitedMembers.totalCrewMembers < 6)
                    {
                        activarCharla = true;
                        businessmanIsActive = true;
                        PlayDialogueClip(0);
                        dialogue.StartDialogue(businessmanPhrases[0],businessmanName);
                        dialogue.panel.SetActive(true);
                        player.objective.missionNew();
                        npcState = 1;
                        player.pistas.ActivarPista(6);
                    }
                break;
                case 1:
                    if(recruitedMembers.totalCrewMembers >= 6 && hasCompass && player.coins.actualCoins >= shipPrice)
                    {
                        PlayDialogueClip(3);
                        dialogue.StartDialogue(businessmanPhrases[3],businessmanName);
                        dialogue.panel.SetActive(true);
                        player.coins.spendCoins(shipPrice);
                        shipObtained = true;
                        player.objective.missionUpdate();
                        ship.shipIsBought = true;
                        npcState = 3;
                        player.pistas.ActivarPista(8);
                        player.audioManager.PlaySfx(10);

                    } else if (recruitedMembers.totalCrewMembers >= 6 && hasCompass && player.coins.actualCoins < shipPrice)
                    {
                        PlayDialogueClip(2);
                        dialogue.StartDialogue(businessmanPhrases[2],businessmanName);
                        dialogue.panel.SetActive(true);
                        player.objective.missionNew();
                        npcState = 2;
                        player.pistas.CompletarPista(7);
                        player.pistas.ActivarPista(8);

                    } else if (recruitedMembers.totalCrewMembers >= 6 && !hasCompass)
                    {
                        missionObject.SetActive(true);
                        PlayDialogueClip(1);
                        dialogue.StartDialogue(businessmanPhrases[1],businessmanName);
                        dialogue.panel.SetActive(true);
                        player.objective.missionUpdate();
                        player.pistas.CompletarPista(6);
                        player.pistas.ActivarPista(7);

                    } else
                    {
                        PlayDialogueClip(0);
                        dialogue.StartDialogue(businessmanPhrases[0],businessmanName);
                        dialogue.panel.SetActive(true);
                        player.objective.missionNew();
                    }
                    break;
                case 2: 
                    if (recruitedMembers.totalCrewMembers >= 6 && hasCompass && player.coins.actualCoins < shipPrice)
                    {
                        PlayDialogueClip(2);
                        dialogue.StartDialogue(businessmanPhrases[2],businessmanName);
                        dialogue.panel.SetActive(true);
                    }
                    else if ((recruitedMembers.totalCrewMembers >= 6) && (hasCompass == true) && (player.coins.actualCoins >= shipPrice))
                    {
                        PlayDialogueClip(3);
                        dialogue.StartDialogue(businessmanPhrases[3],businessmanName);
                        dialogue.panel.SetActive(true);
                        player.coins.spendCoins(shipPrice);
                        shipObtained = true;
                        player.objective.missionUpdate();
                        ship.shipIsBought = true;
                        npcState = 3;
                        player.pistas.ActivarPista(8);
                        player.audioManager.PlaySfx(10);
                    }
                    break;
                default:
                    Debug.Log("No hay más diálogos para este NPC.");
                break;
            }
        }
    }

    private void PlayDialogueClip(int index)
    {
        if (audioSource == null)
        {
            Debug.LogError("AudioSource no asignado en el objeto.");
            return; // Sale del método si no se ha asignado el AudioSource.
        }

        if (dialogueClips == null || dialogueClips.Length == 0)
        {
            Debug.LogError("Los diálogos no están asignados.");
            return; // Sale del método si no se han asignado los clips de audio.
        }

        if (index >= 0 && index < dialogueClips.Length)
        {
            audioSource.clip = dialogueClips[index];
            audioSource.Play();
            isAudioPlaying = true;
        }
        else
        {
            Debug.LogError("Índice fuera de rango en los clips de diálogo.");
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

            if (isAudioPlaying)
            {
                dialogue.panel.SetActive(true);
            }
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

            // Si ya hay una corrutina ejecutándose, la detenemos para evitar múltiples corrutinas simultáneas
            if (deactivatePanelCoroutine != null)
            {
                StopCoroutine(deactivatePanelCoroutine);
            }
            // Iniciamos la corrutina para desactivar el panel después de 3 segundos
            deactivatePanelCoroutine = StartCoroutine(DeactivatePanelAfterDelay());

        }
    }
    // Corrutina para esperar 2 segundos antes de desactivar el panel de diálogo
    private IEnumerator DeactivatePanelAfterDelay()
    {
        yield return new WaitForSeconds(2);

        if(activarCharla == false)
        {
            dialogue.panel.SetActive(false); // Desactiva el panel de diálogo a los dos segundos de salir del collider del CrewMember

        } else
        {
            activarCharla = false;
        }
    }
}

