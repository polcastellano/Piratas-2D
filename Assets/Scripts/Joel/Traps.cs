using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Traps : MonoBehaviour
{
    public float trapDamage = 10f; // Da�o que aplica la trampa
    public float damageInterval = 0.5f; // Intervalo de tiempo entre cada da�o
    private PlayerMovement playerMovement;
    private PlayerManager player;
    public bool isWaterTrap;
    public AudioClip trapDamageSound; // Sonido espec�fico de esta trampa

    private bool isPlayerInTrap = false;
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
            if (isPlayerInTrap)
            {
                // Cambia el clip del audio y luego reproduce el sonido
                if (playerMovement.damageAudioSource.clip != trapDamageSound)
                {
                    playerMovement.damageAudioSource.clip = trapDamageSound;
                }
                if (!playerMovement.damageAudioSource.isPlaying)
                {
                    playerMovement.damageAudioSource.Play();
                }

            }
        }
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {   
            Debug.Log("--------> TOCO TRAMPA");
            isPlayerInTrap = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("--------> NO TOCO TRAMPA");
            isPlayerInTrap = false;
            playerMovement.damageAudioSource.Stop(); // Detener el sonido cuando el jugador sale de la trampa
        }
    }
}
