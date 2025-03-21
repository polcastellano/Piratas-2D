using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarbossaLightChanger : MonoBehaviour
{
   [Tooltip("Lista de GameObjects que se desactivarán al entrar en los colliders.")]
    public List<GameObject> objectsToActivate;

    [Tooltip("Un GameObject que tendrá un comportamiento inverso a los de la lista.")]
    public GameObject inverseObject;

    public string targetTag = "Light_Barbossa"; // Tag que deben tener los colliders estáticos

    private int triggersCount = 0; // Contador de triggers activos

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Verificar si el collider tiene el tag objetivo
        if (collision.CompareTag(targetTag))
        {
            Debug.Log($"Entró en el trigger: {collision.name} con tag {targetTag}");
            triggersCount++;
            SetObjectsState(false); // Desactivar los objetos al entrar
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Verificar si el collider tiene el tag objetivo
        if (collision.CompareTag(targetTag))
        {
            Debug.Log($"Salió del trigger: {collision.name} con tag {targetTag}");
            triggersCount--;

            // Solo activar si no quedan colliders activos
            if (triggersCount <= 0)
            {
                triggersCount = 0; // Evitar valores negativos
                SetObjectsState(true); // Activar los objetos al salir del último collider
            }
        }
    }

    private void SetObjectsState(bool isActive)
    {
        // Cambiar el estado de los objetos en la lista
        foreach (GameObject obj in objectsToActivate)
        {
            if (obj != null)
            {
                obj.SetActive(isActive);
            }
        }

        // Cambiar el estado del objeto inverso
        if (inverseObject != null)
        {
            inverseObject.SetActive(!isActive);
        }

        Debug.Log($"Objetos ahora están {(isActive ? "activados" : "desactivados")}, " +
                  $"Objeto inverso {(isActive ? "desactivado" : "activado")}.");
    }
}
