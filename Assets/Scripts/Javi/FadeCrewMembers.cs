using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeCrewMembers : MonoBehaviour
{
    [SerializeField] private float fadeDuration = 1f; // Duración del fade-out en segundos
    [SerializeField] private Material sharedMaterial; // Material compartido asignado en el editor
    private float initialFadeValue; // Valor inicial de Fade

    private void Start()
    {
        if (sharedMaterial == null)
        {
            Debug.LogError("Por favor, asigna el material compartido en el inspector.");
            return;
        }

        // Guardamos el valor inicial de _Fade
        initialFadeValue = sharedMaterial.GetFloat("_Fade");
        StartFadeOut();
    }

    public void StartFadeOut()
    {
        if (sharedMaterial != null)
        {
            StartCoroutine(FadeOutRoutine());
        }
    }

    private IEnumerator FadeOutRoutine()
    {
        float startFade = sharedMaterial.GetFloat("_Fade");
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            float fadeValue = Mathf.Lerp(startFade, 0f, elapsedTime / fadeDuration);
            sharedMaterial.SetFloat("_Fade", fadeValue);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Aseguramos que el valor de fade sea exactamente 0 al final
        sharedMaterial.SetFloat("_Fade", 0f);

        // Opcional: desactiva el personaje completo al terminar el fade-out
        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        // Restauramos el valor inicial de _Fade cuando se detiene el modo de juego
        if (sharedMaterial != null)
        {
            sharedMaterial.SetFloat("_Fade", initialFadeValue);
        }
    }
}
