using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class ScriptMenu : MonoBehaviour
{
    public DataManager dataManager;
    public Button continueButton;
    public CustomNetworkHUD customNetworkHUD;
    private ButtonSound buttonSound;
    [SerializeField] TextMeshProUGUI textProgress;
    [SerializeField] Slider progressSlider;

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

        // Obtén el componente ButtonSound una vez
        buttonSound = continueButton.GetComponent<ButtonSound>();

        if (buttonSound == null)
        {
            Debug.LogWarning("No se encontró el componente ButtonSound en el botón de continuar.");
        }

        // Verifica si hay un archivo de guardado y configura el botón y el script en consecuencia
        if (dataManager.SaveExists())
        {
            continueButton.interactable = true;
            if (buttonSound != null)
            {
                buttonSound.enabled = true;
            }
        }
        else
        {
            continueButton.interactable = false;
            if (buttonSound != null)
            {
                buttonSound.enabled = false;
            }
        }
    }

    public void Continuar()
    {
        Debug.Log("Continuar");
        Time.timeScale = 1f;
        StartCoroutine(CargarPartida());
        if(dataManager != null) {
            dataManager.LoadGame();
        }
        
    }

    public void nuevaPartida()
    {
        dataManager.DeleteSaveFile();
        dataManager.loadGame = false;
        Debug.Log("Nueva partida");
        StartCoroutine(CargarNuevaPartida());
        Time.timeScale = 1f;
    }

    public void controles()
    {
        Debug.Log("Controles");
    }

    public void creditos()
    {
        Debug.Log("Creditos");
        SceneManager.LoadScene(3);
    }
    

    public void opciones()
    {
        Debug.Log("Opciones");
    }

    public void Salir()
    {
        Debug.Log("Salir");
        Application.Quit();
    }

    public void CreateHost() 
    {
        customNetworkHUD.StartHost();
    }
    public void JoinMP() 
    {
        customNetworkHUD.StartClient();
    }

    public IEnumerator CargarPartida()
    {
        progressSlider.gameObject.SetActive(true);
        AsyncOperation operacionCarga = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1); // Carga la siguiente escena sumando 1 a la escena actual. Menu = 0, Juego = 1

        while (operacionCarga.isDone == false)
        {
            float progress = Mathf.Clamp01(operacionCarga.progress / .09f);
            progressSlider.value = progress;
            textProgress.text = "Cargando... " + Mathf.RoundToInt(progress * 100) + "%";
            yield return null;
        }
    }
    private IEnumerator CargarNuevaPartida()
    {
        progressSlider.gameObject.SetActive(true);
        AsyncOperation operacionCarga = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1); // Carga la siguiente escena sumando 1 a la escena actual. Menu = 0, Juego = 1

        while (operacionCarga.isDone == false)
        {
            float progress = Mathf.Clamp01(operacionCarga.progress / .09f);
            progressSlider.value = progress;
            textProgress.text = "Cargando... " + Mathf.RoundToInt(progress * 100) + "%";
            yield return null;
        }
    }
}
