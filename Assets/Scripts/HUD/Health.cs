using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour
{
    public Image HealthBar; // Imagen de la barra de vida (relleno)
    public Image HealthBarMenu; // Imagen de la barra de vida del menu de info
    public float maximumHealth = 100f; // Vida maxima
    public float actualHealth; // Vida actual
    public float decreaseSpeed = 20f; 
    public float healingTime = 1f; // Tiempo de curacion
    Color healthColor = new Color(0.44f, 0.66f, 0.02f); // Verde amarillento
    Color lowerHealthColor = new Color(0.75f, 0.85f, 0.4f); // Verde amarillento claro
    private bool isFlashing = false; // Bandera para controlar el parpadeo
    public GameObject deathPanelUI;
    private PlayerManager player;

    private ScriptMenuPausa scriptMenuPausa;
    private DataManager dataManager;
    public bool isActivated = false;
    void Start()
    {
        if(dataManager == null)
        {
            dataManager = FindObjectOfType<DataManager>();
        }
        if(SceneManager.GetActiveScene().buildIndex != 2) //&& dataManager.loadGame == false)
        {
            if(dataManager != null && dataManager.loadGame == false) 
            {
                actualHealth = maximumHealth / 2; // La vida actual por defecto sera la mitad de la vida maxima.
            } else {
                actualHealth = maximumHealth;
            }
            
        } else {
            actualHealth = maximumHealth;
        }
        
        if (player == null)
        {
            player = GameObject.FindWithTag("Player").GetComponent<PlayerManager>();
        }
         if (scriptMenuPausa == null)
        {
            scriptMenuPausa = FindObjectOfType<ScriptMenuPausa>();
        }
    }

    private void Update()
    {
        // Si la vida actual es inferior o igual al 20%, parpadea
        if (actualHealth <= 20 && actualHealth > 0)
        {
            HealthBar.color = Color.Lerp(healthColor, lowerHealthColor, Mathf.PingPong(Time.time * 2, 1)); // Parpadeo entre el color normal de la vida y el color de vida baja
            if(HealthBarMenu != null) {   
                HealthBarMenu.color = Color.Lerp(healthColor, lowerHealthColor, Mathf.PingPong(Time.time * 2, 1));
            }

        }
        else if (isFlashing) // Si est� parpadeando, sigue el efecto
        {
            HealthBar.color = Color.Lerp(healthColor, lowerHealthColor, Mathf.PingPong(Time.time * 2, 1));
            if(HealthBarMenu != null) {   
                HealthBarMenu.color = Color.Lerp(healthColor, lowerHealthColor, Mathf.PingPong(Time.time * 2, 1));
            }
        }
        else
        {
            HealthBar.color = healthColor; // Color normal de la barra de vida
            if(HealthBarMenu != null) {   
                HealthBarMenu.color = healthColor;
            }
        }
        
    }

    // Función que disminuye la vida y llama a corrutina para bajar barra de vida de forma progresiva
    public void Damage(float quantity)
    {
        actualHealth = actualHealth - quantity; 
        StartCoroutine(ParpadeoDamage());
        StartCoroutine(DecreaseHealthBarOverTime());
    }

    private IEnumerator DecreaseHealthBarOverTime()
    {
        float targetFillAmount = actualHealth / maximumHealth; // Calculamos el fillAmount objetivo

        // Bajar la barra progresivamente
        while (HealthBar.fillAmount > targetFillAmount)
        {
            HealthBar.fillAmount -= decreaseSpeed * Time.deltaTime / maximumHealth; // Reducimos la barra progresivamente
            if(HealthBarMenu != null) {   
                HealthBarMenu.fillAmount -= decreaseSpeed * Time.deltaTime / maximumHealth;
            }
            yield return null; // Esperamos al siguiente frame
        }

        // Aseguramos que la barra de vida se queda en el valor correcto
        if(HealthBarMenu != null) {   
            HealthBarMenu.fillAmount = targetFillAmount;
            HealthBarMenu.color = (actualHealth <= 20) ? lowerHealthColor : healthColor;
        }

        HealthBar.fillAmount = targetFillAmount;
        HealthBar.color = (actualHealth <= 20) ? lowerHealthColor : healthColor; // Establece el color de la barra después de recibir daño

        // Muerte
        if (actualHealth <= 0 && !isActivated)
        {
            HealthBar.fillAmount = 0;
            if(HealthBarMenu != null) {   
                HealthBarMenu.fillAmount = 0;
            }
            scriptMenuPausa.DeathPlayer();
            player.audioManager.PlaySfx(5);
            isActivated = true;
        }
    }

    private IEnumerator ParpadeoDamage() 
    {
        isFlashing = true;
        yield return new WaitForSeconds(2);
        isFlashing = false;
    }

    // Funci�n que aumenta la barra de vida de forma progresiva
    public void Heal(float quantity)
    {
        StartCoroutine(AumentarVidaProgresivamente(quantity));
    }

    // Corrutina para aumentar la cantidad de vida de forma gradual
    private IEnumerator AumentarVidaProgresivamente(float quantity)
    {
        float startIncrease = actualHealth;
        float finalIncrese = Mathf.Clamp(actualHealth + quantity, 0, maximumHealth);
        float elapsedTime = 0f;

        while (elapsedTime < healingTime)
        {
            elapsedTime += Time.deltaTime;
            actualHealth = Mathf.Lerp(startIncrease, finalIncrese, elapsedTime / healingTime);
            HealthBar.fillAmount = actualHealth / maximumHealth;
            if(HealthBarMenu != null) {   
                HealthBarMenu.fillAmount = actualHealth / maximumHealth;
            }
        }   
            yield return null;

        actualHealth = finalIncrese; // Asegurarse de que el valor final se asigna
    }
}
