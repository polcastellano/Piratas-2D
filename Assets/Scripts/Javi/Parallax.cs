using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
     // Asignar la cámara y las capas desde el Inspector
    public Transform cameraTransform;
    public Vector2 parallaxEffectMultiplier; // Controla el efecto en X e Y
    private Vector3 lastCameraPosition;
    
    void Start()
    {
        // Almacena la última posición de la cámara
        lastCameraPosition = cameraTransform.position;
    }

    void LateUpdate()
    {
        // Calcula el movimiento de la cámara
        Vector3 deltaMovement = cameraTransform.position - lastCameraPosition;
        
        // Aplica el efecto de parallax (multiplicando el movimiento de la cámara por el multiplicador)
        transform.position += new Vector3(deltaMovement.x * parallaxEffectMultiplier.x, deltaMovement.y * parallaxEffectMultiplier.y, 0);

        // Actualiza la última posición de la cámara
        lastCameraPosition = cameraTransform.position;
    }
}
