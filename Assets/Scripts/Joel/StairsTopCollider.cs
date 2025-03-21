using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// SE ENCARGA DE DESACTIVAR EL COLLIDER SUPERIOR DE LAS ESCALERAS SI ESTAS PULSANDO EL GETAXIS VERTICAL
// SE UTILIZA PARA EVITAR CAERSE DESDE LA PARTE ALTA DE LA ESCALERA AL NIVEL INFERIOR
public class StairsTopCollider : MonoBehaviour
{
    private BoxCollider2D boxCollider;
    private PlayerMovement playerMovement;

    void Start()
    {
        // Obt�n el componente Collider que quieres controlar
        boxCollider = GetComponent<BoxCollider2D>();

        // Aseg�rate de que el collider est� activado al inicio
        if (boxCollider != null)
        {
            boxCollider.enabled = true;
        }

        if(playerMovement == null)
        {
            playerMovement = GameObject.FindWithTag("Player").GetComponent<PlayerMovement>();
        }
    }

    void Update()
    {
        // Detecta si hay entrada en el eje vertical (arriba/abajo) y desactiva el collider
        if (Input.GetAxis("Vertical") < 0 || playerMovement.isOnStair)
        {
            if (boxCollider != null)
            {
                boxCollider.enabled = false;
            }
        }
        else
        {
            // Si no hay entrada en el eje vertical, activa el collider
            if (boxCollider != null)
            {
                boxCollider.enabled = true;
            }
        }
    }
}

