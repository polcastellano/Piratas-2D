using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockRotationSon : MonoBehaviour
{
private Quaternion fixedRotation;

    void Start()
    {
        // Establece la rotación fija hacia adelante
        fixedRotation = Quaternion.identity;  // O cualquier otra rotación fija que prefieras
    }

    void LateUpdate()
    {
        // Bloquea la rotación del hijo en el espacio mundial
        transform.rotation = fixedRotation;
    }
}
