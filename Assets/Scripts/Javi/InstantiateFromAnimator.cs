using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateFromAnimator : MonoBehaviour
{
    public GameObject defaultPrefab; // Prefab a instanciar por defecto
    public GameObject defaultReferencePoint; // Punto de referencia por defecto

    public void InstantiateObject()
    {
        if (defaultPrefab == null || defaultReferencePoint == null)
        {
            Debug.LogError("Prefab o referencia no asignados.");
            return;
        }

        // Definir la rotación personalizada
        Quaternion customRotation = Quaternion.Euler(0, -90, 90);

        // Instanciar el prefab en la posición del referencePoint con la rotación personalizada
        GameObject instantiatedObject = Instantiate(defaultPrefab, defaultReferencePoint.transform.position, customRotation);

        // Destruir el objeto después de 1 segundo
        Destroy(instantiatedObject, 1f);
    }
}
