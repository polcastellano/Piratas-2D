using UnityEngine;
using Mirror;

public class CustomNetworkManager : NetworkManager
{
    private int currentPlayerIndex = 0; // Controla el índice del jugador actual

    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        // Obtener el punto de spawn para el jugador actual
        Transform spawnPoint = SpawnManager.Instance.GetSpawnPoint(currentPlayerIndex);

        if (spawnPoint != null)
        {
            // Instanciar al jugador en el punto de spawn
            GameObject player = Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
            NetworkServer.AddPlayerForConnection(conn, player);
        }
        else
        {
            Debug.LogError("No se pudo encontrar un punto de spawn válido.");
        }

        // Incrementar el índice para el próximo jugador
        currentPlayerIndex++;
    }
}