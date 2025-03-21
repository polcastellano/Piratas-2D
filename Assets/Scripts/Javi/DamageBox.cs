using System.Collections;
using UnityEngine;

public class DamageBox : MonoBehaviour
{
    [Header("Referencias")]
    public SpriteRenderer boxSpriteRenderer; // Sprite de la caja
    public bool isBoxBroken = false; // Estado de la caja
    public Collider2D boxCollider; // Collider de la caja
    public float yOffset = 1.0f; // Posición Y del prefab instanciado

    [Header("Movimiento y Rotación")]
    public float fallSpeed = 2f; // Velocidad de caída
    public float rotationSpeed = 100f; // Velocidad de rotación en X
    private bool isFalling = true; // Controla si la caja sigue descendiendo
    private bool isRotating = true; // Controla si la caja sigue rotando

    [Header("Efectos Visuales y Sonoros")]
    public string brokenBoxRoute; // Ruta de la imagen rota
    public ParticleSystem boxParticle; // Partículas de la caja rota
    public AudioSource audioSource; // Audio Source
    public AudioClip[] boxBreakSounds; // Sonidos de rotura

    [Header("Objeto a Desactivar")]
    public GameObject objectToDisable; // Objeto que se desactivará al romper la caja

    void Update()
    {
        if (isFalling)
        {
            // **Mover la caja hacia abajo continuamente**
            transform.position += Vector3.down * fallSpeed * Time.deltaTime;
        }

        if (isRotating)
        {
            // **Rotar la caja en el eje X**
            transform.Rotate(Vector3.forward* rotationSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if ((other.CompareTag("Player") || other.CompareTag("Floor")) && !isBoxBroken)
        {
            isBoxBroken = true; // Marcar como rota para evitar múltiples activaciones
            isFalling = false;  // **Detener el movimiento**
            isRotating = false; // **Detener la rotación**
            
            Debug.Log($"🛑 Caja impactó con {other.gameObject.name} ({other.tag})");

            // **Desactivar el SpriteRenderer**
            if (boxSpriteRenderer != null)
            {
                boxSpriteRenderer.enabled = false;
            }

            // **Desactivar el Collider2D**
            if (boxCollider != null)
            {
                boxCollider.enabled = false;
            }

            // **Reproducir partículas**
            if (boxParticle != null)
            {
                boxParticle.transform.position = transform.position; // Asegurar que la partícula aparece en el impacto
                boxParticle.Play();
            }

            // **Reproducir sonido**
            PlayRandomBoxBreakSound();

            // **Desactivar otro objeto si está asignado**
            if (objectToDisable != null)
            {
                objectToDisable.SetActive(false);
                Debug.Log($"Objeto {objectToDisable.name} desactivado.");
            }

            // **Iniciar la autodestrucción después de 2 segundos**
            StartCoroutine(DestroyBoxAfterDelay(2f));
        }
    }

    private void PlayRandomBoxBreakSound()
    {
        if (audioSource != null && boxBreakSounds.Length > 0)
        {
            int randomIndex = Random.Range(0, boxBreakSounds.Length);
            audioSource.clip = boxBreakSounds[randomIndex];
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning("⚠️ No se encontró un AudioSource o no hay clips asignados.");
        }
    }

    private IEnumerator DestroyBoxAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Debug.Log($"🗑️ La caja {gameObject.name} ha sido destruida.");
        Destroy(gameObject);
    }
}
