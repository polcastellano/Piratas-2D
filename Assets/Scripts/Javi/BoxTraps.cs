using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxTraps : MonoBehaviour
{
    public float trapDamage = 10f; // Daño que aplica la trampa
    public float damageInterval = 0.5f; // Intervalo de tiempo entre cada daño
    private PlayerMovement playerMovement;
    private PlayerManager player;
    public bool isWaterTrap;
    public AudioClip trapDamageSound; // Sonido específico de esta trampa

    private bool isPlayerInTrap = false;
    private bool hasPlayedSound = false; // Nueva variable para evitar repetir el sonido
    private float nextDamageTime = 0f;

    void Start()
    {
        if (player == null)
        {
            player = GameObject.FindWithTag("Player").GetComponent<PlayerManager>();
        }
        if (playerMovement == null)
        {
            playerMovement = GameObject.FindWithTag("Player").GetComponent<PlayerMovement>();
        }
    }

    void Update()
    {
        if (Time.timeScale != 0f)
        {
            if (isPlayerInTrap && Time.time >= nextDamageTime)
            {
                player.health.Damage(trapDamage);
                nextDamageTime = Time.time + damageInterval;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {   
            Debug.Log("--------> TOCO TRAMPA");
            isPlayerInTrap = true;

            // Reproducir sonido solo si no ha sonado antes
            if (!hasPlayedSound)
            {
                if (playerMovement.damageAudioSource != null && trapDamageSound != null)
                {
                    playerMovement.damageAudioSource.clip = trapDamageSound;
                    playerMovement.damageAudioSource.Play();
                }
                hasPlayedSound = true; // Marcar que ya se reprodujo el sonido
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("--------> NO TOCO TRAMPA");
            isPlayerInTrap = false;

            // Reiniciar el estado para que el sonido pueda volver a reproducirse la próxima vez
            hasPlayedSound = false;

            // Detener el sonido si estaba reproduciéndose
            if (playerMovement.damageAudioSource.isPlaying)
            {
                playerMovement.damageAudioSource.Stop();
            }
        }
    }
}
