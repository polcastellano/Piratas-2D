using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotarBarraVida : MonoBehaviour
{
    public Transform player; // Asigna el transform del jugador en el Inspector
    public GameObject barraVida;

void Start(){
    player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
}
    void Update()
    {
        if (player != null)
        {
            // Si el jugador está a la derecha, gira el sprite a la derecha
            if (player.position.x > transform.position.x)
            {
                transform.localRotation = Quaternion.Euler(0, 0, 0);
                if (barraVida != null)
                {
                    barraVida.transform.localRotation = Quaternion.Euler(0, 0, 0);
                }
            }
            // Si el jugador está a la izquierda, gira el sprite a la izquierda
            else
            {
                transform.localRotation = Quaternion.Euler(0, 180, 0);
                if (barraVida != null)
                {
                    barraVida.transform.localRotation = Quaternion.Euler(0, 180, 0);
                }
            }
        }
    }
}
