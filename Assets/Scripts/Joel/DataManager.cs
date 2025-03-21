using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance { get; private set; } // Singleton instance
    
    [System.Serializable]
    public class PlayerData
    {
        public int coins;
        public bool isDay;
        public List<int> collectedItems; // Lista de objetos con ID
        public List<string> collectedItemsName; // Lista de objetos con nombre
        public List<int> collectedCrewMembers; // Lista de tripulantes reclutados
        public List<int> activeMissionCrewMembers; // Lista de tripulantes si esta la misión en curso.
        public bool bossScene;
        public int npcState;
        public bool shipIsBought;
        public bool activeTpZone; // Variable que activa la chica del tp 


        public PlayerData()
        {
            collectedItems = new List<int>();
            collectedItemsName = new List<string>();
            collectedCrewMembers = new List<int>();
            activeMissionCrewMembers = new List<int>();
            
            npcState = 0;
        }
    }

    public PlayerManager playerManager;
    public ScriptMenu menu;
    public Day_Night day_Night;
    public PlayerData playerData;
    public string saveFilePath;
    public bool loadGame = false;
    public bool cargarPartidaMuerte;
    public Businessman businessman;
    public Ship ship;
    public GameObject panelGuardado;
    private CanvasGroup panelCanvasGroup;

    void Start()
    {
        // Configura la ruta del archivo de guardado
        saveFilePath = Application.persistentDataPath + "/PlayerData.json";
        SceneManager.sceneLoaded += OnSceneLoaded; // Suscripción al evento de carga de escena
    }

    void Awake() 
{
    // Solo marca este objeto como persistente si no hay otra instancia
    if (Instance == null)
    {
        Instance = this;
        DontDestroyOnLoad(gameObject); // No destruir este objeto al cambiar de escena
    }
    else if (Instance != this)
    {
        // Si ya existe una instancia, destruye esta nueva
        Destroy(gameObject);
    }
}

    void OnDestroy()
    {
        // Desvincula el evento de carga de escena cuando el objeto sea destruido
        if (Instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) // Modificado: nuevo método para manejar la carga de escena
    {
        // Verifica si estamos regresando a la escena principal (escena 0) y evita duplicar el DataManager
        if (scene.buildIndex == 0) // Si estamos en la escena del menú (escena 0)
        {
            // Asegura que no haya más de un DataManager al entrar en la escena
            DataManager existingDataManager = FindObjectOfType<DataManager>();
            if (existingDataManager != null && existingDataManager != this)
            {
                Destroy(gameObject); // Destruye cualquier DataManager que no sea el actual
            }

            if (menu == null)
            {
                menu = FindObjectOfType<ScriptMenu>();
            }

        }
        // Verifica si no estamos en la escena de inicio
        if (scene.buildIndex == 1)
        {
            // Inicializa el PlayerManager y Day_Night si no están asignados
            if (playerManager == null)
            {
                playerManager = GameObject.FindWithTag("Player")?.GetComponent<PlayerManager>();
                
            }

            if (day_Night == null)
            {
                day_Night = FindObjectOfType<Day_Night>();
            }

            if (businessman == null)
            {
                businessman = FindObjectOfType<Businessman>();
            }

            if (ship == null)
            {
                ship = FindObjectOfType<Ship>();
                Debug.Log("SHIP ENCONTRADO!");
            }
            if (panelGuardado == null)
            {
                panelGuardado = GameObject.Find("PartidaGuardada");

                if (panelGuardado != null)
                {
                    panelCanvasGroup = panelGuardado.GetComponent<CanvasGroup>();
                    if (panelCanvasGroup == null)
                    {
                        panelCanvasGroup = panelGuardado.AddComponent<CanvasGroup>();
                    }

                    // Cambia la transparencia en lugar de activar/desactivar el objeto
                    panelCanvasGroup.alpha = 0; // Completamente invisible
                }
                else
                {
                    Debug.LogWarning("No se encontró el panel 'PartidaGuardada' en la escena.");
                }
            // Carga los datos del juego
            LoadGame(); // Modificado: llama a LoadGame() al cargar la escena
            }
        }
    }

    public void SaveGame()
    {
        if (playerManager != null) // Asegura que playerManager esté asignado
        {
            playerData.coins = playerManager.coins.actualCoins; // Establece las monedas en lugar de sumarlas
        }

        playerData.isDay = !day_Night.actualTime();
        playerData.npcState = businessman.npcState;

        if(ship != null)
        {
            playerData.shipIsBought = ship.shipIsBought;
        }
        else
        {
            Debug.Log("Objeto ship no asignado!");
        }

        string savePlayerData = JsonUtility.ToJson(playerData);
        File.WriteAllText(saveFilePath, savePlayerData);
        Debug.Log("Save file created at: " + saveFilePath);
        //Debug.Log("MONEDAS GUARDADAS:" + playerData.coins);
        panelCanvasGroup.alpha = 1; // Panel de guardado activado
        StartCoroutine(DesactivatePanelAfterDelay());
    }

    public void SaveGameNpcTp()
    {
        if (playerManager != null) // Asegura que playerManager esté asignado
        {
            playerData.coins = playerManager.coins.actualCoins; // Establece las monedas en lugar de sumarlas
        }

        playerData.isDay = day_Night.actualTime();
        playerData.npcState = businessman.npcState;

        if(ship != null)
        {
            playerData.shipIsBought = ship.shipIsBought;
        }
        else
        {
            Debug.Log("Objeto ship no asignado!");
        }

        string savePlayerData = JsonUtility.ToJson(playerData);
        File.WriteAllText(saveFilePath, savePlayerData);
        Debug.Log("Save file created at: " + saveFilePath);
        //Debug.Log("MONEDAS GUARDADAS:" + playerData.coins);
        panelCanvasGroup.alpha = 1; // Panel de guardado activado
        StartCoroutine(DesactivatePanelAfterDelay());
    }

    public void LoadGame() // Funcion para cargar la partida desde el JSON
    {
        
        if (File.Exists(saveFilePath))
        {
            loadGame = true;
            string loadPlayerData = File.ReadAllText(saveFilePath);
            playerData = JsonUtility.FromJson<PlayerData>(loadPlayerData);
            

            if (playerManager != null) // Asegura que playerManager esté asignado
            {
                playerManager.coins.actualCoins = playerData.coins; // Asigna los coins al playerManager
                //playerManager.transform.position = hostalPosition;
            }

            if(playerData.bossScene && SceneManager.GetActiveScene().buildIndex != 2) // Detecta si es true la bool bossScene y evita recargar escena 2
            {
                SceneManager.LoadScene(2); // Carga la escena del boss
                //Debug.Log("Carga boss: " + SceneManager.GetActiveScene().buildIndex != 2 + " " + playerData.bossScene);
            }
            else if (SceneManager.GetActiveScene().buildIndex != 1) // Condición para evitar recargar escena 1
            {
                SceneManager.LoadScene(1); // Carga la escena del juego
                //StartCoroutine(menu.CargarPartida());
            }
            if(cargarPartidaMuerte)
            {
                playerManager.health.actualHealth = 100;
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);// Recarga la escena del juego actual
                cargarPartidaMuerte = false;
                Time.timeScale = 1;
                ZonaDeteccion.detectado = false;
                //Debug.Log("IF Partida muerte: " + cargarPartidaMuerte);
            }
        }
        else
        {
            // En caso de que no exista un archivo JSON para cargar
            Debug.Log("No hay archivo de guardado para cargar!");
        }
    }

    public void DeleteSaveFile()
    {
        if (File.Exists(saveFilePath))
        {
            File.Delete(saveFilePath);
            Debug.Log("Save file deleted!");
        }
        else
        {
            Debug.Log("There is nothing to delete!");
        }
            playerData.collectedItemsName.Clear();
            playerData.collectedItemsName.Clear();
            playerData.collectedCrewMembers.Clear();
            playerData.activeMissionCrewMembers.Clear();
    }

    public bool SaveExists() // Devuelve true si existe un archivo de guardado y en caso contrario false
    {
        return File.Exists(saveFilePath);
    }

    private IEnumerator DesactivatePanelAfterDelay()
    {
        yield return new WaitForSeconds(3);
        if(panelCanvasGroup != null) {
           panelCanvasGroup.alpha = 0; // Desactiva el panel de guardado  
        }
    }
    
}
