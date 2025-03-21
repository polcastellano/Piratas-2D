using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScriptMenuPausa : MonoBehaviour
{
    
    public GameObject menuPausaUI;  // Referencia al menu de pausa
    public GameObject menuOpcionesUI; // Referencia al menu de opciones
    public GameObject menuMuerteUI; // Referencia al menu de muerte
    public GameObject menuControlesUI; // Referencia al menu de controles
    public GameObject HUDPanel; // Referencia al panel del HUD
    public Button btnReaparecer; // Referencia al btnReaparecer del panel de muerte
    private bool juegoPausado = false; // Booleana para saber si el juego esta pausado
    
    private CustomNetworkHUD customNetworkHUD;
    public DataManager dataManager;
    
    void Start()
    {
        if (dataManager == null)
        {
            dataManager = FindObjectOfType<DataManager>();
        }
        if (customNetworkHUD == null)
        {
            customNetworkHUD = FindObjectOfType<CustomNetworkHUD>();
        }
    }


    void Update()
    {
        if(Input.GetKeyDown(KeyCode.JoystickButton1) && juegoPausado)
        {
            Reanudar();
        }
        // Detecta si se presiona la tecla ESC o el btn de tres lineas en mando Xbox
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.JoystickButton7) )
        {
            if(!menuOpcionesUI.activeInHierarchy && !menuControlesUI.activeInHierarchy) {
                if (juegoPausado)
                {
                    Reanudar();
                }
                else
                {
                    Pausar();
                }
            }  
        }
    }

    public void cargarPartida()
    {
        if(dataManager != null) {
            dataManager.cargarPartidaMuerte = true;
            dataManager.LoadGame();
        } else {
            Debug.Log("dataManager no existe o no esta referenciado");
        }
        //Time.timeScale = 1f;
        Debug.Log("Cargar partida");

    }
    // M�todo para reanudar el juego
    public void Reanudar()
    {
        menuPausaUI.SetActive(false);  // Esconde el menu de pausa
        if (HUDPanel != null) // Activa el hud si esta en escena
        {
            HUDPanel.SetActive(true);
        } 
        
        Time.timeScale = 1f;  // Restablece el tiempo del juego
        juegoPausado = false;
        Debug.Log("Reanudar partida");
    }


    public void SalirAlMenu()
    {
        menuPausaUI.SetActive(false);
        Time.timeScale = 1f;  // Restablece el tiempo del juego
        juegoPausado = false;
        SceneManager.LoadScene(0);
        if(dataManager != null) {
            dataManager.loadGame = false;
        }
        
    }

    // M�todo para pausar el juego
    void Pausar()
    {
        if (HUDPanel != null) { HUDPanel.SetActive(false); } // Desactiva el hud si esta en escena
        menuPausaUI.SetActive(true);  // Muestra el menu de pausa

        if (SceneManager.GetActiveScene().buildIndex != 4) { // Si no es la escena con índice 4 (por ejemplo, la escena multiplayer)
            Time.timeScale = 0f;  // Detiene el tiempo del juego
        }
        
        juegoPausado = true;
    }
    
    public void DeathPlayer()
    {
        menuMuerteUI.SetActive(true);
        if (dataManager.SaveExists())
        {
            btnReaparecer.interactable = true;
        } else {
            btnReaparecer.interactable = false;
        }
        Time.timeScale = 0f;
        //juegoPausado = true;
    }

    // M�todo para salir del juego
    public void SalirJuego()
    {
        if(dataManager != null) {
            dataManager.loadGame = false;
        }
        Application.Quit();
    }

    public void Desconectar() 
    {
        customNetworkHUD.StopConnection();
        SceneManager.LoadScene(0);
    }
 
}

