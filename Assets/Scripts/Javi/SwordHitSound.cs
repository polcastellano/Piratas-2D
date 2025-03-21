using UnityEngine;

public class SwordHitSound : MonoBehaviour
{
    [Header("Configuración de Detección")]
    public Collider2D detectionCollider; // Collider que detectará al jugador (debe ser Trigger)

    [Header("Configuración de Audio")]
    public AudioSource audioSource; // Fuente de audio

    private void Start()
    {
        // Verificar si las referencias están asignadas correctamente
        if (detectionCollider == null)
        {
            Debug.LogWarning("⚠️ No se ha asignado un Collider2D en " + gameObject.name);
        }
        
        if (audioSource == null)
        {
            Debug.LogWarning("⚠️ No se ha asignado un AudioSource en " + gameObject.name);
        }

        // Asegurar que el collider está en modo Trigger
        if (detectionCollider != null)
        {
            detectionCollider.isTrigger = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D other) // Detecta cuando entra el Player
    {
        if (other.CompareTag("Player")) // Detecta solo al Player
        {
            if (audioSource != null)
            {
                audioSource.Play(); // Reproducir sonido
                Debug.Log("🔊 Sonido activado al detectar al jugador en " + gameObject.name);
            }
            else
            {
                Debug.LogWarning("⚠️ No hay AudioSource asignado en " + gameObject.name);
            }
        }
    }
}
