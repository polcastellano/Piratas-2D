using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FinalBossFightSound : MonoBehaviour
{
    //public AudioManager audioManager;
    public AudioSource finalFightSource;
    public bool onFinalZone = false;
    public bool once = false;
    void Start()
    {
        /* if(audioManager == null)
        {
            audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        } */

        if(finalFightSource == null)
        {
            finalFightSource = gameObject.GetComponent<AudioSource>();
        }
    }

    void Update()
    {
        if (PerlaNegra.barbossaIsDead == true && finalFightSource.isPlaying)
        {
            StartCoroutine(FadeOutAndStop(finalFightSource, 2f)); // 2 segundos para desvanecer
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            onFinalZone = true;
            once = true;
        }
    }

    private IEnumerator FadeOutAndStop(AudioSource audioSource, float fadeDuration)
    {
        float startVolume = audioSource.volume;

        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / fadeDuration;
            yield return null; // Espera un frame antes de continuar
        }

        audioSource.Stop();
        audioSource.volume = startVolume; // Restablece el volumen para posibles usos futuros
    }
}
