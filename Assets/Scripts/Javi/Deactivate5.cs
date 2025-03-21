using UnityEngine;

public class AutoDeactivate : MonoBehaviour
{
    public float deactivateTime = 5f; // Tiempo antes de desactivarse

    void OnEnable() // Se ejecuta cuando el objeto se activa
    {
        Invoke("Deactivate", deactivateTime);
    }

    void Deactivate()
    {
        gameObject.SetActive(false); // Desactiva el objeto
    }
}
