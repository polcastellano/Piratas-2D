using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    
    public AudioSource backgroundMusic;       
    public AudioSource sfxSource;         
    public AudioSource dialogueSource;
    public AudioSource ambientSource;
    public AudioSource uiSource;
    public AudioClip[] audioClips;
    public AudioClip[] audioUI;


    /* CREAR UN NUEVO ARRAY SOLO PARA LOS CLIPS DE LOS DIALOGOS. EL OTRO SOLO PARA EFECTOS DE INTERACCION, ETC */

    private AudioClip backgroundClip;
    private AudioClip sfxClip;
    private AudioClip uiClip;


    // VARIABLES SONIDO BARBOSSA
    public List<AudioClip> audioClipsBarbossa;
    private AudioClip currentClip;
    public AudioSource source;
    public float minWaitBetweenPlays = 1f;
    public float maxWaitBetweenPlays = 30f;
    public float waitTimeCountdown = -1f;



    public AudioSource audioSourceEnemigo;
    public AudioClip dañoEnemigo;

    public AudioSource jackEmpezarSourceSent;
    public AudioClip jackEmpezarSound;
    
    public AudioClip barbossasFinalPhrase;

    public AudioClip finalBossFightClip;
    public Health health;

    // Variable para controlar el tiempo entre reproducciones
    private Dictionary<int, float> audioCooldowns = new Dictionary<int, float>();
    public float cooldownTime = 0f; // Tiempo mínimo entre reproducciones en segundos

    public void ReproduceBarbossasSoundEffects()
    {
        if (!source.isPlaying)
        {
            if (waitTimeCountdown < 0f)
            {
                currentClip = audioClipsBarbossa[Random.Range(0, audioClipsBarbossa.Count)];
                source.clip = currentClip;
                source.Play();
                waitTimeCountdown = Random.Range(minWaitBetweenPlays, maxWaitBetweenPlays);
            }
            else
            {
                waitTimeCountdown -= Time.deltaTime;
            }
        }
    }

    public void ReproduceBarbossasDyingPhraseSoundEffects()
    {
        source.clip = barbossasFinalPhrase;
        source.Play();
    }

    public void ReproduceEnemigoDaño(AudioSource enemySourceSent)
    {
        enemySourceSent.clip = dañoEnemigo;
        enemySourceSent.Play();
        //Debug.Log("REPRODUCIDO!");
    }

    public void ReproduceFinalFightSoundEffect(AudioSource finalFightSourceSent)
    {
        finalFightSourceSent.clip = finalBossFightClip;
        finalFightSourceSent.Play();
        //Debug.Log("MUSICA BATALLA FINAL!");
    }

    public void ReproduceSonidoJackEmpezar()
    {
        if(jackEmpezarSourceSent != null) {
            jackEmpezarSourceSent.clip = jackEmpezarSound;
            jackEmpezarSourceSent.Play();
        }
        
        //Debug.Log("REPRODUCIDO!");
    }

    // ------------------------


    /* public Dialogue dialogue;
    public PlayerMovement playerMovement; */
    //private void Awake()
    //{
    //    // Configuración del Singleton
    //    if (instance == null)
    //    {
    //        instance = this;
    //        DontDestroyOnLoad(gameObject);  // Mantiene el AudioManager entre escenas
    //    }
    //    else
    //    {
    //        Destroy(gameObject);
    //    }
    //}

    void Start()
    {
        //PlayBackgroundMusic();
         // Inicializa el diccionario de cooldowns
        for (int i = 0; i < audioClips.Length; i++)
        {
            audioCooldowns[i] = 0f;
        }
    }

    // Musica de fondo
    public void PlayBackgroundMusic()
    {
        foreach (var audio in audioClips)
        {
            if (audio.name == "backgroundClip")
            {
                backgroundClip = audio;
                backgroundMusic.clip = backgroundClip;
                backgroundMusic.loop = true;
                backgroundMusic.Play();
                break; 
            }
        }
    }

    // Efectos de sonido (monedas, ron, manzana, etc)
    public void PlaySfx(int index)
    {
        if (index >= 0 && index < audioClips.Length)
        {
           // Verifica si hay un cooldown activo para este clip
        float currentCooldown = cooldownTime > 0 ? cooldownTime : 0f;

        if (Time.time >= audioCooldowns[index] || currentCooldown == 0f)
        {
            // Forzar la interrupción del AudioSource si ya está reproduciendo
            if (sfxSource.isPlaying)
            {
                sfxSource.Stop();
            }

            sfxClip = audioClips[index];
            sfxSource.PlayOneShot(sfxClip);

            // Actualiza el tiempo del próximo cooldown solo si el cooldown es mayor a 0
            if (currentCooldown > 0)
            {
                audioCooldowns[index] = Time.time + currentCooldown;
            }
        }
        else
        {
            Debug.LogWarning("Índice fuera de rango: " + index);
        }
        
        }
    }

    // Efectos de sonido de la UI (Interfaz)
    public void PlayUI(int index)
    {
        if (index >= 0 && index < audioUI.Length)
        {
            uiClip = audioUI[index];
            uiSource.PlayOneShot(uiClip);
        }
        else
        {
            Debug.LogWarning("Índice fuera de rango: " + index);
        }
    }

    // Dialogo de los tripulantes
    public void PlayDialogue(AudioClip[] list, int index)
    {
        AudioSource dialogueSfx = gameObject.GetComponent<AudioSource>();
        
        if (index >= 0 && index < list.Length) 
        {
            if(index == 0)
            {
                dialogueSfx.PlayOneShot(list[index]);
            }
            else if(index == 1)
            {
                if(list[0] != null)
                {
                    dialogueSfx.Stop();
                }
                dialogueSfx.PlayOneShot(list[index]);
            }
        }
        else
        {
            Debug.LogWarning("Índice fuera de rango: " + index);
        }
    }

    /* public void StopCrewMemberDialogue()
    {
        AudioSource dialogueSfx = gameObject.GetComponent<AudioSource>();
        if(dialogueSfx.isPlaying)
        {
            dialogueSfx.Stop();
            dialogue.panel.SetActive(false);
            playerMovement.enabled = !playerMovement.enabled;
        }
    } */

    public void PlayBusinessmanDialogue(AudioClip[] list, int index)
    {
        AudioSource dialogueSfx = gameObject.GetComponent<AudioSource>();
        Debug.Log("NOMBRE DEL OBJETO: " + gameObject.name);   
        if (index >= 0 && index < list.Length) 
        {
            for (int i = 0; i < index; i++)
            {
                if(list[i] != null)
                {
                    dialogueSfx.Stop();
                }
            }
            dialogueSfx.PlayOneShot(list[index]);
        }
        else
        {
            Debug.LogWarning("Índice fuera de rango: " + index);
        }
    }
}
