using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ChestLight2D : MonoBehaviour
{
public UnityEngine.Rendering.Universal.Light2D light2D; // La luz 2D que vamos a controlar

    [Header("Intensity Settings")]
    public float minIntensity = 0.8f;
    public float maxIntensity = 1.0f;
    public float intensityChangeSpeed = 1f; // Velocidad de cambio en intensidad

    [Header("Radius Settings")]
    public float minOuterRadius = 0.5f;
    public float maxOuterRadius = 0.7f;
    public float outerRadiusChangeSpeed = 0.5f; // Velocidad de cambio en radio

    private bool isInitialRadiusSet = false;

    void Start()
    {
        if (light2D == null)
        {
            light2D = GetComponent<UnityEngine.Rendering.Universal.Light2D>();
        }

        // Asegurarse de que el radio inicial es 0
        light2D.pointLightOuterRadius = 0;
        StartCoroutine(OuterRadiusCycle());
    }

    void Update()
    {
        if (isInitialRadiusSet)
        {
            // Variar la intensidad progresivamente
            light2D.intensity = Mathf.Lerp(light2D.intensity, Random.Range(minIntensity, maxIntensity), intensityChangeSpeed * Time.deltaTime);
        }
    }

    private IEnumerator OuterRadiusCycle()
    {
        while (true)
        {
            // Fase de incremento
            yield return StartCoroutine(AdjustOuterRadius(0f, minOuterRadius, 0.5f));

            // Mantener el radio por 1 segundo
            yield return new WaitForSeconds(0.5f);

            // Fase de decremento
            yield return StartCoroutine(AdjustOuterRadius(minOuterRadius, 0f, 0.1f));
        }
    }

    private IEnumerator AdjustOuterRadius(float startRadius, float targetRadius, float duration)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            light2D.pointLightOuterRadius = Mathf.Lerp(startRadius, targetRadius, elapsedTime / duration);
            yield return null;
        }

        light2D.pointLightOuterRadius = targetRadius; // Asegurarse de que alcanza el valor final

        // Establecer el indicador de inicialización si es la primera vez
        if (!isInitialRadiusSet && targetRadius == minOuterRadius)
        {
            isInitialRadiusSet = true;
        }
    }
}
