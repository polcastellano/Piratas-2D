using UnityEngine;

public class ObjectActivatorBoxes : MonoBehaviour
{
    [Header("Objeto a Activar")]
    public GameObject objectToActivate; // Objeto que se activará

    private void OnTriggerEnter2D(Collider2D other)
{
    if (other.CompareTag("Player")) 
    {
        if (objectToActivate != null)
        {
            objectToActivate.SetActive(true);
            Debug.Log($"✅ {objectToActivate.name} ACTIVADO por {other.name}");
        }
        else
        {
            Debug.LogWarning("⚠️ No se ha asignado ningún objeto para activar.");
        }
    }
}

}
