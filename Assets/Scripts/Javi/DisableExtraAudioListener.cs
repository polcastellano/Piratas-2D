using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class DisableExtraAudioListener : NetworkBehaviour
{
    private void Start()
    {
        // Encuentra el AudioListener en el objeto del jugador
        AudioListener audioListener = GetComponentInChildren<AudioListener>();

        if (audioListener != null)
        {
            // Desactiva el AudioListener si no es el jugador local
            if (!isLocalPlayer)
            {
                audioListener.enabled = false;
            }
        }
        else
        {
            Debug.LogWarning("No se encontró un AudioListener en el objeto del jugador.");
        }
    }
}
