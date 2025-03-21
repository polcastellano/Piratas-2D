using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ChangeColorPlayer2PrefabMp : NetworkBehaviour
{
    [Tooltip("Lista de SpriteRenderers de los objetos hijos que se deben modificar")]
    public SpriteRenderer[] childRenderers; // Lista de SpriteRenderers a modificar

    [SyncVar(hook = nameof(OnColorChanged))]
    private Color playerColor = Color.white; // Color sincronizado entre el host y los clientes

    public Color hostPlayerColor = Color.blue; // Color específico para el host

    public override void OnStartServer()
    {
        base.OnStartServer();

        // Cambia el color del host y sincroniza
        if (connectionToClient.connectionId == 0) // Si es el host
        {
            playerColor = hostPlayerColor; // Asigna el color del host
        }
    }

    private void OnColorChanged(Color oldColor, Color newColor)
    {
        ApplyColorToChildRenderers(newColor);
    }

    private void ApplyColorToChildRenderers(Color color)
    {
        // Cambiar el color de cada SpriteRenderer en la lista
        foreach (SpriteRenderer renderer in childRenderers)
        {
            if (renderer != null)
            {
                renderer.color = color;
            }
        }
    }
}
