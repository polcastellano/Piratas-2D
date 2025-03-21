using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeInitial : MonoBehaviour
{
    public Image fadeImage;               // Referencia a la imagen del canvas
    public float fadeDuration = 1f;     // Duración del fade in en segundos

    private void Start()
    {
        // Establece la imagen en opaco al inicio
        Color startColor = fadeImage.color;
        startColor.a = 1f;
        fadeImage.color = startColor;

        // Inicia la corrutina de fade in
        StartCoroutine(FadeIn());
    }

    private System.Collections.IEnumerator FadeIn()
    {
        float elapsedTime = 0f;
        Color color = fadeImage.color;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            fadeImage.color = color;
            yield return null;
        }

        // Asegúrate de que la imagen sea completamente transparente al finalizar
        color.a = 0f;
        fadeImage.color = color;
    }
}
