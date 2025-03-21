using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckGround : MonoBehaviour
{

    public static bool isGrounded;
    public JackStatesMachine jackStateMachine;

    public GameObject effectObject; // Asignar el GameObject que deseas activar


    void Start()
    {
        if(jackStateMachine == null)
        {
            jackStateMachine = FindObjectOfType<JackStatesMachine>();
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Floor")) {
            isGrounded = true;
                 // Instanciar el prefab en la posición del jugador
            // Instanciar el prefab en la posición del jugador
        if (effectObject != null)
        {
            // Crear la posición ajustada
            Vector3 adjustedPosition = transform.position + new Vector3(0, -1.1f, 0);

            // Crear la rotación ajustada
            Quaternion adjustedRotation = Quaternion.Euler(-180f, 90f, 0f);

            // Instanciar el objeto con posición y rotación ajustadas
            GameObject instantiatedEffect = Instantiate(effectObject, adjustedPosition, adjustedRotation);

            // Destruir el prefab después de 1 segundo
            Destroy(instantiatedEffect, 1f);
        }
        }
    }

    void OnTriggerStay2D(Collider2D other) {
        if (other.gameObject.CompareTag("Floor")) {
            isGrounded = true;
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject.CompareTag("Floor")) {
            isGrounded = false;
        }
    }
}
