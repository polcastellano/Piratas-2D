using System.Collections;
using UnityEngine;

public class AmbientSound : MonoBehaviour
{
    private AudioSource audioSource;
    private float fadeDuration = 2.0f; // Duraci�n del desvanecimiento del sonido
    public bool soundStart; // Suena al iniciar partida? True = Si, False = No
    public float maxVolume; // Volumen m�ximo del sonido

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        // Si soundStart est� activado, el sonido comenzar� al iniciar la partida
        if (soundStart)
        {
            audioSource.volume = maxVolume;
            audioSource.Play();
        }
        else
        {
            audioSource.volume = 0;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Reinicia el sonido si ya estaba pausado o en volumen 0
            if (!audioSource.isPlaying || audioSource.volume == 0)
            {
                audioSource.Stop();  // Aseg�rate de que el sonido se detiene por completo
                audioSource.volume = 0; // Reinicia el volumen a 0
                audioSource.Play();     // Comienza a reproducir desde el inicio
                StartCoroutine(FadeInSound());
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (gameObject.activeInHierarchy)
            {
                StartCoroutine(FadeOutSound());
            }
        }
    }

    IEnumerator FadeInSound()
    {
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            audioSource.volume = Mathf.Lerp(0, maxVolume, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        audioSource.volume = maxVolume;
    }

    IEnumerator FadeOutSound()
    {
        float startVolume = audioSource.volume;
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            audioSource.volume = Mathf.Lerp(startVolume, 0, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        audioSource.volume = 0;
        audioSource.Stop();
    }
}
