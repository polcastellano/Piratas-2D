using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomVoicesBarbossa : MonoBehaviour
{
    public AudioManager audioManager;
    private bool hasPlayedDyingPhrase = false;
    /* public List<AudioClip> audioClips;
    public AudioClip currentClip;
    public AudioSource source;
    public float minWaitBetweenPlays = 1f;
    public float maxWaitBetweenPlays = 20f;
    public float waitTimeCountdown = -1f;

    void Start()
    {

    }

    void Update()
    {
        if (!source.isPlaying)
        {
            if (waitTimeCountdown < 0f)
            {
                currentClip = audioClips[Random.Range(0, audioClips.Count)];
                source.clip = currentClip;
                source.Play();
                waitTimeCountdown = Random.Range(minWaitBetweenPlays, maxWaitBetweenPlays);
            }
            else
            {
                waitTimeCountdown -= Time.deltaTime;
            }
        }
    } */

    void Start()
    {
        
    }

    void Update()
    {
        if(PerlaNegra.barbossaIsDead == false)
        {
            if(ZonaDeteccion.detectado == true)
            {
                audioManager.ReproduceBarbossasSoundEffects();
            }
        }
        else if(PerlaNegra.barbossaIsDead == true && hasPlayedDyingPhrase == false)
        {
            audioManager.ReproduceBarbossasDyingPhraseSoundEffects();
            hasPlayedDyingPhrase = true;
        }
    }
}
