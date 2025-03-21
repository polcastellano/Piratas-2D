using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Cinemachine;

public class SeguimientoCamara :  NetworkBehaviour
{
   public GameObject player1CameraPrefab; // Prefab para la cámara del jugador 1
    public GameObject player2CameraPrefab; // Prefab para la cámara del jugador 2

    private CinemachineVirtualCamera virtualCameraInstance; // Instancia de la cámara virtual

    private void Start()
    {
        if (isLocalPlayer)
        {
            // Seleccionar el prefab adecuado según si es el primer jugador o no
            GameObject cameraPrefabToUse = isServer ? player1CameraPrefab : player2CameraPrefab;

            // Instanciar la cámara virtual
            virtualCameraInstance = Instantiate(cameraPrefabToUse).GetComponent<CinemachineVirtualCamera>();

            // Configurar la cámara para que siga al jugador local
            virtualCameraInstance.Follow = transform;

            // Mantener la cámara entre escenas
            DontDestroyOnLoad(virtualCameraInstance.gameObject);
        }
    }

   
}
