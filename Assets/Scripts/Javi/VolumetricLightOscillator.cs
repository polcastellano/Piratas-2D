using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class VolumetricLightOscillator : MonoBehaviour
{
    [Tooltip("Velocidad de cambio de la intensidad (en segundos entre cambios).")]
    public float changeInterval = 1f;

    [Tooltip("Rango mínimo de la intensidad.")]
    public float minIntensity = 0f;

    [Tooltip("Rango máximo de la intensidad.")]
    public float maxIntensity = 1f;

    private Light2D light2D;
    private float nextChangeTime;

    private void Awake()
    {
        // Intentar obtener la Light2D del objeto
        light2D = GetComponent<Light2D>();

        if (light2D == null)
        {
            Debug.LogError("Este script necesita estar en un objeto con una Light2D.");
        }
    }

    private void Update()
    {
        if (light2D == null || Time.time < nextChangeTime)
            return;

        // Cambiar intensidad de forma abrupta
        light2D.intensity = (light2D.intensity == minIntensity) ? maxIntensity : minIntensity;

        // Establecer el tiempo para el próximo cambio
        nextChangeTime = Time.time + changeInterval;
    }
}

