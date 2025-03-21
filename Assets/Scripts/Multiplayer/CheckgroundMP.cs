using UnityEngine;
using Mirror;

public class CheckGroundMP : NetworkBehaviour
{
    [SyncVar] public bool isGrounded; // Sincroniza el estado entre servidor y clientes

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!isServer) return; // Solo el servidor evalúa las colisiones

        if (other.gameObject.CompareTag("Floor"))
        {
            isGrounded = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (!isServer) return; // Solo el servidor evalúa las colisiones

        if (other.gameObject.CompareTag("Floor"))
        {
            isGrounded = false;
        }
    }
}
