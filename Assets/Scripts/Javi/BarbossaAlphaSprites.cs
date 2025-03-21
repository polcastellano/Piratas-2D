using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarbossaAlphaSprites : MonoBehaviour
{private SpriteRenderer spriteRenderer;
    private Coroutine fadeCoroutine;
    public float fadeDuration = 1f; // Duración del fade en segundos

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("No se encontró un SpriteRenderer en el objeto.");
            return;
        }

        // Asegurarse de que el alpha esté en 0 al inicio
        SetAlpha(0f);
    }

    private void OnEnable()
    {
        if (spriteRenderer != null)
        {
            // Al activarse, hacer fade al alpha máximo
            StartFade(1f);
        }
    }

    private void OnDisable()
    {
        if (spriteRenderer != null)
        {
            // Al desactivarse, directamente ajustar el alpha a 0
            StopCurrentFade();
            SetAlpha(0f);
        }
    }

    private void StartFade(float targetAlpha)
    {
        // Detener cualquier fade en curso
        StopCurrentFade();

        // Iniciar nuevo fade solo si el GameObject está activo
        if (gameObject.activeInHierarchy)
        {
            fadeCoroutine = StartCoroutine(FadeToAlpha(targetAlpha));
        }
    }

    private void StopCurrentFade()
    {
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
            fadeCoroutine = null;
        }
    }

    private System.Collections.IEnumerator FadeToAlpha(float targetAlpha)
    {
        float startAlpha = spriteRenderer.color.a;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / fadeDuration);
            SetAlpha(newAlpha);
            yield return null;
        }

        // Asegurar que el alpha final sea exactamente el objetivo
        SetAlpha(targetAlpha);
        fadeCoroutine = null;
    }

    private void SetAlpha(float alpha)
    {
        if (spriteRenderer != null)
        {
            Color color = spriteRenderer.color;
            color.a = alpha;
            spriteRenderer.color = color;
        }
    }

    private void Update()
    {
        // Si el objeto padre o el propio se desactiva, forzar fade out
        if (!gameObject.activeInHierarchy && spriteRenderer != null)
        {
            SetAlpha(0f); // Asegurar que el alpha esté a 0 si no está activo
        }
    }
}
