using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSmooth : MonoBehaviour
{
     public Transform playerTransform;
    public float smoothFactor = 0.1f;
    private Vector3 velocity = Vector3.zero;

    void LateUpdate()
    {
        // Suaviza el seguimiento del jugador
        Vector3 targetPosition = new Vector3(playerTransform.position.x, playerTransform.position.y, transform.position.z);
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothFactor);
    }
}
