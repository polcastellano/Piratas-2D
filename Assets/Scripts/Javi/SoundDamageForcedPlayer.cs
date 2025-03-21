using UnityEngine;

public class SwordHitSound2D : MonoBehaviour
{
    [Header("Configuración de Detección")]
    public CapsuleCollider2D detectionCollider; // Collider 2D que detectará la espada

    [Header("Configuración de Audio")]
    public AudioSource audioSource; // Fuente de audio
    public AudioClip soundClip; // Sonido a reproducir cuando detecte la espada

    private void Start()
    {
        if (detectionCollider == null)
        {
            Debug.LogWarning("⚠️ No se ha asignado un CapsuleCollider2D en " + gameObject.name);
        }
        
        if (audioSource == null || soundClip == null)
        {
            Debug.LogWarning("⚠️ No se ha asignado un AudioSource o AudioClip en " + gameObject.name);
        }

        // Asegurar que el CapsuleCollider2D está en modo trigger
        if (detectionCollider != null)
        {
            detectionCollider.isTrigger = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D other) // Cambiado para detección 2D
    {
        if (other.CompareTag("Sword")) // Detectar colisión con un objeto con el tag "Sword"
        {
            if (audioSource != null && soundClip != null)
            {
                audioSource.PlayOneShot(soundClip); // Reproducir sonido
                Debug.Log("🔊 Sonido de golpe de espada reproducido en " + gameObject.name);
            }
            else
            {
                Debug.LogWarning("⚠️ No hay AudioSource o SoundClip asignado en " + gameObject.name);
            }
        }
    }
}
