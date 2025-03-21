using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnoreLayerCollision : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Ignorar la colisión con el jugador
            Physics2D.IgnoreCollision(collision.collider, GetComponent<Collider2D>());
            Debug.Log("Colisión ignorada con el jugador.");
        }
    }
}
