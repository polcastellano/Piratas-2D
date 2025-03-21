using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Objective : MonoBehaviour
{
    public TextMeshProUGUI ObjectiveTitle; // Referencia al titulo del panel de objetivo 
    public TextMeshProUGUI ObjectiveText; // Referencia al texto del panel de objetivo
    public TextMeshProUGUI countCrewMembers; // Referencia al TMP del contador de tripulantes
    public TextMeshProUGUI countCrewMembersMenu; // Referencia al TMP del contador de tripulantes del menu info
    public RecruitedMembers recruitedMembers; // Referencia a recruitedMembers

    public Image objetivoActualImage; // La imagen de "ObjetivoActual"
    public CanvasGroup objetivoCanvasGroup; // CanvasGroup para la imagen
    private float fadeDuration = 0.5f; // Duraci�n del fade in/out
    private float displayDuration = 8f; // Tiempo que se mantiene visible
    private int maxMembers = 6; // Numero maximo de tripulantes
    private bool isTabPressed = false; // Variable booleana para controlar la pulsaci�n de Tab

    void Start()
    {
            ObjectiveTitle.text = "* Objetivo Actual *";
            ObjectiveText.text = "Habla con el pirata del embarcadero"; // Objetivo inicial (opcional)
            objetivoActualImage.gameObject.SetActive(false); // Aseg�rate de que el panel est� oculto al inicio
    }

    void Update()
    {
        // Mostrar el panel si se mantiene pulsado Tab
        // if (Input.GetKey(KeyCode.Tab) || Input.GetKey(KeyCode.JoystickButton6))
        // {
            
        //     // Mostrar el panel si no est� activo
        //     if (!objetivoActualImage.gameObject.activeSelf)
        //     {
        //         isTabPressed = true;
        //         StartCoroutine(ShowObjective());
        //     }

        // }
    }

    // Nueva mision
    public void missionNew()
    {
        ObjectiveTitle.text = "Nueva Misión"; // Cambia el titulo al nuevo titulo
        ObjectiveText.text = "Añadida al menu de misiónes"; // Cambia el texto actual por el nuevo texto obtenido
        StartCoroutine(ShowObjective()); // Mostrar el panel
    }
    public void missionUpdate()
    {
        ObjectiveTitle.text = "Misión actualizada"; // Cambia el titulo al nuevo titulo
        ObjectiveText.text = "Ver en el menu de misiónes"; // Cambia el texto actual por el nuevo texto obtenido
        StartCoroutine(ShowObjective()); // Mostrar el panel
    }
    public void missionComplete()
    {
        ObjectiveTitle.text = "Misión completada"; // Cambia el titulo al nuevo titulo
        ObjectiveText.text = "Ver en el menu tus misiónes"; // Cambia el texto actual por el nuevo texto obtenido
        StartCoroutine(ShowObjective()); // Mostrar el panel
    }
    public void missionNewObject()
    {
        ObjectiveTitle.text = "Objeto añadido"; // Cambia el titulo al nuevo titulo
        ObjectiveText.text = "Añadido al menu de misiónes"; // Cambia el texto actual por el nuevo texto obtenido
        StartCoroutine(ShowObjective()); // Mostrar el panel
    }

    public void missionChangeText(string newText)
    {
        //ObjectiveText.text = newText; // Cambia el texto al nuevo texto proporcionado
        StartCoroutine(ShowObjective()); // Siempre mostrar el panel al cambiar el texto
    }

    // Funcion para cambiar el titulo y descripcion del panel de objetivos
    public void missionChangeText(string newTitle, string newText)
    {
        ObjectiveTitle.text = newTitle; // Cambia el titulo al nuevo titulo
        ObjectiveText.text = newText; // Cambia el texto actual por el nuevo texto obtenido
        StartCoroutine(ShowObjective()); // Siempre mostrar el panel al cambiar el texto
    }

    // Actualiza el contador de tripulantes reclutados
    public void sumCount()
    {
        countCrewMembers.text = "Tripulantes " + recruitedMembers.totalCrewMembers + "/" + maxMembers + "";
        if(countCrewMembersMenu != null) {
            countCrewMembersMenu.text = "" + recruitedMembers.totalCrewMembers + "/" + maxMembers + "";
        }
        StartCoroutine(ShowObjective()); // Mostrar el panel al sumar tripulantes
    }

    IEnumerator ShowObjective()
    {
        // Aseg�rate de que el panel est� activo
        objetivoActualImage.gameObject.SetActive(true);

        // Fade in
        yield return StartCoroutine(FadeObjective(1f, fadeDuration));

            yield return new WaitForSeconds(displayDuration);
            
            yield return StartCoroutine(HideObjective());
    }

    IEnumerator HideObjective()
    {
        // Fade out
        yield return StartCoroutine(FadeObjective(0f, fadeDuration));

        // Desactiva el panel al final
        isTabPressed = false;
        objetivoActualImage.gameObject.SetActive(false);
    }

    IEnumerator FadeObjective(float targetAlpha, float duration)
    {
        float startAlpha = objetivoCanvasGroup.alpha;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startAlpha, targetAlpha, elapsed / duration);
            objetivoCanvasGroup.alpha = newAlpha;
            objetivoActualImage.fillAmount = newAlpha; // Ajusta el fill de la imagen progresivamente
            yield return null;
        }
        objetivoCanvasGroup.alpha = targetAlpha;
        objetivoActualImage.fillAmount = targetAlpha;
    }
}
