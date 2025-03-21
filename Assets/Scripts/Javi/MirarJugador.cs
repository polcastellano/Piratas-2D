using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MirarJugador : MonoBehaviour
{
    public Transform player; // Asigna el transform del jugador en el Inspector
    public GameObject interactionButton;

    void Update()
    {
        if (player != null)
        {
            // Si el jugador está a la derecha, gira el sprite a la derecha
            if (player.position.x > transform.position.x)
            {
                transform.localRotation = Quaternion.Euler(0, 0, 0);
                if(interactionButton != null)
                {
                    interactionButton.transform.localRotation = Quaternion.Euler(0, 0, 0);
                }
            }
            // Si el jugador está a la izquierda, gira el sprite a la izquierda
            else
            {
                transform.localRotation = Quaternion.Euler(0, 180, 0);
                if(interactionButton != null)
                {
                    interactionButton.transform.localRotation = Quaternion.Euler(0, 180, 0);
                }
            }
        }
    }
}
