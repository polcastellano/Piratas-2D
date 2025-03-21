using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using UnityEngine.UI;
using System;
using static UnityEngine.Rendering.DebugUI;
using TMPro;
using Unity.VisualScripting;

public class Day_Night : MonoBehaviour
{
    public List<GameObject> objects;  // Lista de objetos de luz que se pueden configurar desde el Inspector
    public Light2D globalLight;                   // Referencia a la luz global 2D
    public Volume postProcessVolume;               // Referencia al Volume de post-procesado
    private WhiteBalance whiteBalance;             // Referencia al White Balance
    public GameObject spriteObject;                // GameObject con el Sprite Renderer
    private SpriteRenderer spriteRenderer;         // Referencia al Sprite Renderer
    public bool isDay = true;       // Estado inicial, empieza en día
    public Image fadeImage;                        // Imagen de la pantalla para el fade in/out
    private bool isTransitioning = false;          // Controla si el fade está en progreso
    public GameObject[] crewMembers;
    public GameObject sun;
    public GameObject moon;
    public DataManager dataManager;


    private List<Transform> directDayChildren = new List<Transform>();
    private List<Transform> directNightChildren = new List<Transform>();

    public GameObject dayEnemiesObject;
    public GameObject nightEnemiesObject;
    
    public Transform[] dayEnemies;
    public Transform[] nightEnemies;


    void Start()
    {
        // Guardar enemigos de dia
        if (dayEnemiesObject != null)
        {
            Transform parentTransform = dayEnemiesObject.transform;
            
            for (int i = 0; i < parentTransform.childCount; i++)
            {
                directDayChildren.Add(parentTransform.GetChild(i));
            }

            dayEnemies = directDayChildren.ToArray(); 
            // foreach (Transform child in dayEnemies)
            // {
            //     Debug.Log("ENEMIGO: " + child.name);
            // }
        }
        else
        {
            Debug.Log("***** DAY ENEMIES OBJECT ES NULO *****");
        }

        // Guardar enemigos de noche
        if (nightEnemiesObject != null)
        {
            Transform parentTransform = nightEnemiesObject.transform;

            for (int i = 0; i < parentTransform.childCount; i++)
            {
                directNightChildren.Add(parentTransform.GetChild(i));
            }

            nightEnemies = directNightChildren.ToArray(); 
            // foreach (Transform child in nightEnemies)
            // {
            //     Debug.Log("ENEMIGO: " + child.name);
            // }
        }
        else
        {
            Debug.Log("***** NIGHT ENEMIES OBJECT ES NULO *****");
        }

        // Enemigos se activan dependiendo del ciclo 
        if (isDay)
        {
            foreach (Transform enemy in directDayChildren)
            {
                enemy.gameObject.SetActive(true);
            }
            foreach (Transform enemy in directNightChildren)
            {
                enemy.gameObject.SetActive(false);
            }
        }
        else
        {
            foreach (Transform enemy in directDayChildren)
            {
                enemy.gameObject.SetActive(false);
            }
            foreach (Transform enemy in directNightChildren)
            {
                enemy.gameObject.SetActive(true);
            }
        }




        if (dataManager == null)
        {
            dataManager = FindObjectOfType<DataManager>();
        }

        // Configurar el White Balance del Volume si está presente
        if (postProcessVolume != null && postProcessVolume.profile.TryGet(out whiteBalance))
        {
            whiteBalance.temperature.value = 40f;  // Configuración inicial para el día
        }
        else
        {
            Debug.LogWarning("White Balance no encontrado en el Volume de post-procesado.");
        }

        // Configuración inicial de la luz global
        if (globalLight != null)
        {
            globalLight.intensity = 0.8f;  // Intensidad inicial para el día
        }
        // Obtener el Sprite Renderer del objeto
        if (spriteObject != null)
        {
            spriteRenderer = spriteObject.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                spriteRenderer.color = new Color(160 / 255f, 160 / 255f, 160 / 255f);  // Color inicial para el día
            }
        }
        if (dataManager != null && dataManager.loadGame)
        {
            Debug.Log("LOADGAME: " + dataManager.loadGame);
            if (dataManager.playerData.isDay == false)
            {
                ToggleDayNight();
            }
        }
        //if(dataManager.loadGame == true) {
            StartCoroutine(activeCrewMembers());
        //}
        
    }
    void Update()
    {
        // DEBUG: Detectar la tecla N para iniciar el ciclo de transición de día/noche
        // if (Input.GetKeyDown(KeyCode.N) && !isTransitioning)
        // {
        //     StartCoroutine(DayNightTransition());
        // }
    }
    public IEnumerator DayNightTransition()
    {
        isTransitioning = true;

        // Fade In (pantalla a negro)
        yield return StartCoroutine(Fade(1f, 1f)); // Desvanecerse a negro en 1 segundo

        // Esperar 0.5 segundos cuando la pantalla esté completamente en negro
        yield return new WaitForSeconds(0.5f);

        // Realizar el cambio de día/noche después del fade in
        ToggleDayNight();

        // Fade Out (pantalla de negro a transparente)
        yield return StartCoroutine(Fade(0f, 1.5f)); // Desvanecerse a transparente en 1.5 segundo

        isTransitioning = false;
    }

    // Script para hacer el FadeInOut
    public IEnumerator FadeInOut()
    {
        isTransitioning = true;

        // Fade In (pantalla a negro)
        yield return StartCoroutine(Fade(1f, 1f)); // Desvanecerse a negro en 1 segundo

        // Esperar 0.5 segundos cuando la pantalla esté completamente en negro
        yield return new WaitForSeconds(1.5f);

        // Fade Out (pantalla de negro a transparente)
        yield return StartCoroutine(Fade(0f, 1.5f)); // Desvanecerse a transparente en 1.5 segundo

        isTransitioning = false;
    }
    public IEnumerator activeCrewMembers()
    {
        yield return new WaitForSeconds(1f);
        activeCrewMember();
    }

    private IEnumerator Fade(float targetAlpha, float duration)
    {
        Color color = fadeImage.color;
        float startAlpha = color.a;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            color.a = Mathf.Lerp(startAlpha, targetAlpha, elapsed / duration);
            fadeImage.color = color;
            yield return null;
        }

        color.a = targetAlpha;
        fadeImage.color = color;
    }

    public void ToggleDayNight()
    {
        // Cambiar el estado entre día y noche
        isDay = !isDay;

        if (isDay)
        {
            foreach (Transform enemy in directDayChildren)
            {
                enemy.gameObject.SetActive(true);
            }
            foreach (Transform enemy in directNightChildren)
            {
                enemy.gameObject.SetActive(false);
            }
        }
        else
        {
            foreach (Transform enemy in directDayChildren)
            {
                enemy.gameObject.SetActive(false);
            }
            foreach (Transform enemy in directNightChildren)
            {
                enemy.gameObject.SetActive(true);
            }
        }


        // Encender o apagar las luces según el estado
        foreach (GameObject light in objects)
        {
            if (light != null)
            {
                light.SetActive(!isDay);  // Si es de día, luces activadas; si es de noche, luces desactivadas
            }
        }

        // Cambiar la intensidad de la luz global según el estado
        if (globalLight != null)
        {
            globalLight.intensity = isDay ? 0.8f : 0.3f;  // 0.8 para día, 0.3 para noche
        }
        // Cambiar la temperatura del White Balance según el estado
        if (whiteBalance != null)
        {
            whiteBalance.temperature.value = isDay ? 40f : -65f;  // 40 para día, -65 para noche
        }
        // Cambiar el color del Sprite Renderer según el estado
        if (spriteRenderer != null)
        {
            spriteRenderer.color = isDay ? new Color(160 / 255f, 160 / 255f, 160 / 255f)
                                         : new Color(105 / 255f, 105 / 255f, 105 / 255f);  // 160 para día, 105 para noche
        }

        sun.SetActive(isDay);
        moon.SetActive(!isDay);

        // Mensaje de depuración opcional
        Debug.Log(isDay ? "Cambio a Día" : "Cambio a Noche");

        StartCoroutine(activeCrewMembers());
    }

    // Llamar desde objetos para saber si es de dia o noche
    public bool actualTime()
    {
        return isDay; // True = Dia / False = Noche
    }

    // Activa o desactiva los crewMember en base a la booleana de actualTime que indica si es de dia o noche
    public void activeCrewMember()
    {
        //Debug.Log("Activando Crew Members DAY_NIGHT");
        foreach (GameObject crewMember in crewMembers)
        {

            bool cicleDay = crewMember.GetComponent<CrewMember>().dayTime;
            bool isAllDay = crewMember.GetComponent<CrewMember>().allDay;
            bool isRecruited = crewMember.GetComponent<CrewMember>().isRecruited;

            if (isRecruited == false)
            {
                if (isAllDay)
                {
                    crewMember.SetActive(true);
                }
                else if (cicleDay == isDay)
                {
                    crewMember.SetActive(true);
                }
                else
                {
                    crewMember.SetActive(false);
                }
            }
        }
    }

}
