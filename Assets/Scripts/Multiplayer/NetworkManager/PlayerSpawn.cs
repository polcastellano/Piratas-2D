using UnityEngine;
using Mirror;

public class PlayerSpawn : NetworkBehaviour
{
    // Posiciones donde aparecerán los jugadores
    public Vector3 player1Position = new Vector3(46, 10, 0); // Posición del jugador 1
    public Vector3 player2Position = new Vector3(49, 10, 0); // Posición del jugador 2

    public override void OnStartLocalPlayer()
    {
        // Solo el jugador local se posiciona
        if (isLocalPlayer)
        {
            Debug.Log("is local player");
            // Obtener el número de jugadores conectados, lo cual está en NetworkManager
            int playerCount = NetworkServer.connections.Count;

            // Asignar la posición según el número de jugadores
            if (playerCount == 1)
            {
                transform.position = player1Position; // Asigna la posición para el primer jugador
            }
            else if (playerCount == 2)
            {
                transform.position = player2Position; // Asigna la posición para el segundo jugador
            }
        }
    }
}
