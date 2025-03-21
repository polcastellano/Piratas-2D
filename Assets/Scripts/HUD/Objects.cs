using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Objects : MonoBehaviour
{
    public GameObject[] objetos; // Array de objetos para los objetos
    public GameObject[] objetosMenu; // Array de objetos para los objetos del menu info
    public Image panelFillImage; // Imagen de tipo Filled para el panel
    public CanvasGroup panelCanvasGroup; // CanvasGroup para el panel de objetos
    private float fadeDuration = 0.5f; // Duraci�n del desvanecimiento
    private float displayDuration = 4f; // Tiempo que se mantiene visible
    private bool isTabPressed = false; // Variable booleana para controlar la pulsaci�n de Tab
    public PlayerManager player;
    private bool isPanelVisible = false; // Estado del panel
    public List<int> listItems = new List<int>();
    private DataManager dataManager;

    void Start()
    {
        if (player == null)
        {
            player = GameObject.FindWithTag("Player").GetComponent<PlayerManager>();
        }
        if (dataManager == null)
        {
            dataManager = FindObjectOfType<DataManager>();
        }
        if(dataManager != null) {
            listItems = dataManager.playerData.collectedItems;
            Debug.Log("Objetos cargados: " + listItems);

            if(dataManager.loadGame == false) {
                listItems.Clear();
                Debug.Log("Limpia la lista de objetos");
            }
        }
        
        // Inicializar todos los objetos como no recogidos (color negro)
        for (int i = 0; i < objetos.Length; i++)
        {
            objetosMenu[i].SetActive(false);
        }
        

    }

    void Update()
    {
        // Actualizar el estado de las im�genes de los objetos seg�n los IDs en listItems
        ActualizarObjetosRecogidos();
    }

    // Metodo para actualizar las im�genes de los objetos recogidos
    public void ActualizarObjetosRecogidos()
    {
        // Recorrer la lista de IDs de objetos recogidos
        foreach (int id in listItems)
        {
            if (id >= 0 && id < objetos.Length)
            {
                objetosMenu[id].SetActive(true);
            }
        }
    }

    // M�todo para marcar un objeto como recogido
    public void ObtenerObjeto(int index)
    {
        if (index >= 0 && index < objetos.Length)
        {
            player.audioManager.PlaySfx(1);
            objetosMenu[index].SetActive(true);
        }
    }

    // M�todo para marcar un objeto como entregado
    public void EntregarObjeto(int index)
    {
        if (index >= 0 && index < objetos.Length)
        {
            //player.audioManager.PlaySfx(1);
            objetosMenu[index].SetActive(false);
        }
    }


}
