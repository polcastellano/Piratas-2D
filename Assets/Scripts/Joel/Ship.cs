using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class Ship : MonoBehaviour
{
    public GameObject interactiveBtn;
    public bool activateInteract;
    public bool shipIsBought;
    public DataManager dataManager;
    [SerializeField] TextMeshProUGUI textProgress;
    [SerializeField] Slider progressSlider;

    // Start is called before the first frame update
    void Start()
    {
        if(dataManager == null)
        {
            dataManager = FindObjectOfType<DataManager>();
        }
        if(dataManager != null) { 
            if(dataManager.playerData.shipIsBought == false)
            {
                shipIsBought = false;
            }
            else if(dataManager.playerData.shipIsBought == true)
            {
                shipIsBought = true;
            }
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (activateInteract && (Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.JoystickButton3)) && shipIsBought)
        {
            
            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); // Carga la siguiente escena sumando 1 a la escena actual. Menu = 0, Juego = 1, Boss = 2, Creditos = 3
            if(dataManager != null) {
                dataManager.playerData.bossScene = true;
                dataManager.SaveGame();
            }
            StartCoroutine(CargarBossScene());
        }
    }
    
    public void cargarBossAtajoTeclado() {
        StartCoroutine(CargarBossScene());
    }
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && shipIsBought == true)
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

    public IEnumerator CargarBossScene()
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
