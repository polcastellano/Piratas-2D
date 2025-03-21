using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public string itemName;
    public int itemId;
    public bool isCollected;
    public bool activateInteract;
    public string activeMission;
    public GameObject interactiveBtn;
    public GameObject fishObject;
    public ItemsList itemsList;
    public Objects HUDitemsList;
    public Businessman businessman;
    public AudioManager audioManager;
    private List<int> listItems = new List<int>();
    private DataManager dataManager;
    private Objects objects;

    private PlayerManager player;

    public GameObject prefabToInstantiate;
    private float particleDuration = 0.6f;

    void Start()
    {
        //gameObject.SetActive(false);

        if(dataManager == null)
        {
            dataManager = FindObjectOfType<DataManager>();
        }
        if (player == null)
        {
            player = GameObject.FindWithTag("Player").GetComponent<PlayerManager>();
        }
        if(objects == null)
        {
            objects = FindObjectOfType<Objects>();
        }
        if (dataManager != null) {
            listItems = dataManager.playerData.collectedItems;
            foreach (int item in listItems)
            {
                if(item == itemId)
                {
                    Debug.Log("Collected item: " + item);
                    isCollected = true;
                }
            }
        }
        
    }

    void Update()
    {
        if (activateInteract && (Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.JoystickButton3)))
        {
            if (gameObject.tag == "item")
            {
                // Llamamos a la funci�n en ObjectList y le pasamos el nombre del objeto
                if (gameObject != null)
                {
                    Debug.Log("NOMBRE REAL DEL OBJETO: " + itemName);
                    itemsList.keepObjects(itemName);
                    isCollected = true;
                    gameObject.SetActive(false);
                    if (prefabToInstantiate != null)
                    {
                        GameObject particleInstance = Instantiate(prefabToInstantiate, transform.position, Quaternion.identity);
                        // Destruye el prefab después de 0.5 segundos
                        Destroy(particleInstance, particleDuration);
                    }
                    audioManager.PlaySfx(1);
                    player.objective.missionNewObject();
                    //player.objective.changeText(activeMission);
                    if (itemName == "Fish" && fishObject != null)
                    {
                        fishObject.SetActive(false);
                    }
                    HUDitemsList.ObtenerObjeto(itemId);
                    if (itemName == "Compass")
                    {
                        businessman.hasCompass = true;
                    }
                    if(dataManager != null) {
                        dataManager.playerData.collectedItems.Add(itemId);
                    }
                }
            }

        }
    }
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
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
            interactiveBtn.SetActive(false);
            activateInteract = false;

            GameObject infBtn = interactiveBtn.transform.GetChild(2).gameObject;
            infBtn.SetActive(false);
        }
    }

}

