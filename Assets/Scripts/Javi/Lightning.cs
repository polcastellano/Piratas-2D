using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Lightning : MonoBehaviour
{
    public ParticleSystem particleSystem; // Referencia al sistema de partículas
    public Light2D light2D;               // Referencia a la luz 2D
    public AudioSource audioSource;       // Fuente de audio para el efecto de sonido
    
    public float lightDuration = 0.2f;    // Duración de la luz encendida (en segundos)

    public AudioClip[] lightSounds; // Array de efectos de sonido de truenos para los rayos

    private ParticleSystem.EmissionModule emissionModule; // Para acceder al Emission Module
    private float nextEmissionTime = 8f;       // Tiempo para la próxima emisión
    private bool start;
     void Start()
    {
        // Validación de referencias
        if (particleSystem == null || light2D == null || audioSource == null || lightSounds.Length == 0)
        {
            Debug.LogError("Faltan referencias: Asegúrate de asignar el Particle System, Light 2D, AudioSource y los AudioClips.");
            return;
        }

        // Asegúrate de que la luz esté apagada al inicio
        light2D.gameObject.SetActive(false);
        particleSystem.Stop();
    }

    void Update()
    {
        // Cuenta regresiva para la próxima emisión de luz
        nextEmissionTime -= Time.deltaTime;

        if (nextEmissionTime <= 0 || !start)
        {
            // Reinicia y activa el sistema de partículas
            particleSystem.Play();

            // Activa la luz
            light2D.gameObject.SetActive(true);

            // Selecciona un sonido aleatorio y lo reproduce
            AudioClip randomLightSound = lightSounds[Random.Range(0, lightSounds.Length)];
            audioSource.PlayOneShot(randomLightSound);

            // Lanza la corrutina para apagar la luz y el sistema de partículas después de la duración especificada
            StartCoroutine(TurnOffLight());

            // Configura el tiempo para la próxima emisión entre 3 y 7 segundos
            nextEmissionTime = Random.Range(3f, 7f);

            start = true;
        }
    }

    private System.Collections.IEnumerator TurnOffLight()
    {
        // Espera la duración especificada y apaga la luz y el sistema de partículas
        yield return new WaitForSeconds(lightDuration);
        light2D.gameObject.SetActive(false);
        //yield return new WaitForSeconds(0.8f);
        particleSystem.Stop();
    }
}
