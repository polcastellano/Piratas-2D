using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour
{
    private PlayerMovement player;

    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerMovement>();
    }

    private void OnTriggerStay2D(Collider2D other)
{
    if (other.CompareTag("Player") || other.CompareTag("FloorDetector"))
    {
        //Rigidbody2D rb = other.GetComponent<Rigidbody2D>();

        // Desactivar completamente la gravedad y el movimiento físico
        player.GetComponent<Rigidbody2D>().gravityScale = 0;
        player.GetComponent<Rigidbody2D>().velocity = Vector2.zero; // Evita que la velocidad residual afecte

        // Obtener referencia al transform del jugador
        Transform playerTransform = player.transform;

        // Manejar el movimiento vertical usando Translate
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            player.GetComponent<Rigidbody2D>().isKinematic = true;
            playerTransform.Translate(Vector3.up * 4f * Time.deltaTime); // Subir
        }
        else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            player.GetComponent<Rigidbody2D>().isKinematic = true;
            playerTransform.Translate(Vector3.down * 4f * Time.deltaTime); // Bajar
        }
    }
}

private void OnTriggerExit2D(Collider2D other)
{
    if (other.CompareTag("Player") || other.CompareTag("FloorDetector"))
    {
        //Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
        player.GetComponent<Rigidbody2D>().isKinematic = false;
        // Restaurar la gravedad cuando el jugador sale de la escalera
        //rb.gravityScale = 1; // Usa el valor original de la gravedad
    }
}


}