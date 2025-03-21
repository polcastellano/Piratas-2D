using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Light_Flickering : MonoBehaviour
{
 // Referencia a la Light 2D en el mismo GameObject
    private Light2D light2D;

    [Header("Flicker Settings")]
    public float minIntensity = 0.5f; // Intensidad mínima
    public float maxIntensity = 1.5f; // Intensidad máxima
    public float flickerSpeed = 0.1f; // Velocidad de parpadeo

    private float time;

    void Start()
    {
        
        light2D = GetComponent<Light2D>();

        if (light2D == null)
        {
            Debug.LogError("No se encontró un componente Light2D en este GameObject.");
        }
    }

    void Update()
    {
        if (light2D != null)
        {
            time += Time.deltaTime * flickerSpeed;
            light2D.intensity = Mathf.Lerp(minIntensity, maxIntensity, Mathf.PerlinNoise(time, 0));
        }
    }
}
