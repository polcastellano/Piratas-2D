using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class Alcohol_Effect : MonoBehaviour
{
      public Volume volume; // Asigna el Volume desde el Inspector
    private LensDistortion lensDistortion;
    private Bloom bloom;
    private bool isEffectActive;

    private float targetXMultiplier;
    private float targetYMultiplier;
    private Vector2 targetCenter;

    private float intensityLerpSpeed = 0.2f; // Velocidad de interpolación
    private float multiplierLerpSpeed = 0.3f; // Velocidad de interpolación
    private float centerLerpSpeed = 2f; // Velocidad de interpolación
    private float intensityPhaseDuration = 5f; // Duración de cada fase de intensidad (ascenso y descenso)
    private float resetTransitionDuration = 2f; // Duración de la transición suave de vuelta a los valores originales

    void Start()
    {
        if (volume != null)
        {
            if (volume.profile.TryGet(out lensDistortion) && volume.profile.TryGet(out bloom))
            {
                isEffectActive = false;
            }
            else
            {
                Debug.LogWarning("Lens Distortion o Bloom no encontrados en el perfil de Volume.");
            }
        }
        else
        {
            Debug.LogWarning("Volume no asignado.");
        }
    }


    public void Alcohol_Effect_Ya()
    {
        isEffectActive = true;
        StartCoroutine(ActivateLensDistortionEffect());
    }

    private IEnumerator ActivateLensDistortionEffect()
    {
        float elapsedTime = 0f;

        // Fase 1: Activación del efecto durante 15 segundos
        while (elapsedTime < 15f)
        {
            if (lensDistortion != null && bloom != null)
            {
                // Control de Lens Distortion y Bloom con un ciclo de PingPong
                float cycleProgress = Mathf.PingPong(elapsedTime / intensityPhaseDuration, 1f);
                float targetIntensity = Mathf.Lerp(-0.2f, 0.4f, cycleProgress);
                lensDistortion.intensity.Override(targetIntensity);

                float bloomIntensity = Mathf.Lerp(0f, 0.5f, cycleProgress);
                bloom.intensity.Override(bloomIntensity);

                if (Mathf.Abs(lensDistortion.xMultiplier.value - targetXMultiplier) < 0.02f ||
                    Mathf.Abs(lensDistortion.yMultiplier.value - targetYMultiplier) < 0.02f ||
                    Vector2.Distance(lensDistortion.center.value, targetCenter) < 0.02f)
                {
                    SetNewTargetValues();
                }

                lensDistortion.xMultiplier.Override(Mathf.Lerp(lensDistortion.xMultiplier.value, targetXMultiplier, Time.deltaTime * multiplierLerpSpeed));
                lensDistortion.yMultiplier.Override(Mathf.Lerp(lensDistortion.yMultiplier.value, targetYMultiplier, Time.deltaTime * multiplierLerpSpeed));
                lensDistortion.center.Override(Vector2.Lerp(lensDistortion.center.value, targetCenter, Time.deltaTime * centerLerpSpeed));
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Fase 2: Transición suave de vuelta a los valores originales
        float resetElapsedTime = 0f;
        float currentIntensity = lensDistortion.intensity.value;
        float currentXMultiplier = lensDistortion.xMultiplier.value;
        float currentYMultiplier = lensDistortion.yMultiplier.value;
        Vector2 currentCenter = lensDistortion.center.value;
        float currentBloomIntensity = bloom.intensity.value;

        while (resetElapsedTime < resetTransitionDuration)
        {
            float t = resetElapsedTime / resetTransitionDuration;

            lensDistortion.intensity.Override(Mathf.Lerp(currentIntensity, 0f, t));
            lensDistortion.xMultiplier.Override(Mathf.Lerp(currentXMultiplier, 1f, t));
            lensDistortion.yMultiplier.Override(Mathf.Lerp(currentYMultiplier, 1f, t));
            lensDistortion.center.Override(Vector2.Lerp(currentCenter, new Vector2(0.5f, 0.5f), t));
            bloom.intensity.Override(Mathf.Lerp(currentBloomIntensity, 0f, t));

            resetElapsedTime += Time.deltaTime;
            yield return null;
        }

        // Asegurarse de que los valores se restablecen completamente
        lensDistortion.intensity.Override(0f);
        lensDistortion.xMultiplier.Override(1f);
        lensDistortion.yMultiplier.Override(1f);
        lensDistortion.center.Override(new Vector2(0.5f, 0.5f));
        bloom.intensity.Override(0f);

        isEffectActive = false;
    }

    // Configura nuevos valores objetivo dentro de los rangos deseados
    private void SetNewTargetValues()
    {
        targetXMultiplier = Random.Range(0.7f, 1f);
        targetYMultiplier = Random.Range(0.7f, 1f);
        targetCenter = new Vector2(Random.Range(0.3f, 0.7f), Random.Range(0.3f, 0.7f));
    }
}
