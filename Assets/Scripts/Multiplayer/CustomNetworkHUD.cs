using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Mirror;

public class CustomNetworkHUD : MonoBehaviour
{
    // Referencias de UI
    public TMP_InputField ipInputField; // Para ingresar la IP
    public TMP_Text statusText; // Texto para mostrar el estado de conexión

    void Start()
    {
        // Asignar eventos a los InputField
        if (ipInputField != null)
        {
            ipInputField.onEndEdit.AddListener(UpdateIP);
            ipInputField.text = NetworkManager.singleton.networkAddress; // Valor inicial
        }

        UpdateStatus("Preparado...");
    }

    // Métodos para los botones
    public void StartHost()
    {
        NetworkManager.singleton.StartHost();
        UpdateStatus("Iniciando host...");
        Debug.Log("Creando host: " + ipInputField + ":7777");
    }

    public void StartClient()
    {
        NetworkManager.singleton.networkAddress = ipInputField.text;
        NetworkManager.singleton.StartClient();
        UpdateStatus($"Conectando a {ipInputField.text}:7777");
        Debug.Log("Conectando a: " + ipInputField + ":7777");
        
    }

    public void StopConnection()
    {
        if (NetworkServer.active && NetworkClient.isConnected)
        {
            NetworkManager.singleton.StopHost();
            UpdateStatus("Host parado.");
            Debug.Log("Host parado");
        }
        else if (NetworkClient.isConnected)
        {
            NetworkManager.singleton.StopClient();
            UpdateStatus("Cliente desconectado.");
            Debug.Log("Cliente desconectado");
        }
        else if (NetworkServer.active)
        {
            NetworkManager.singleton.StopServer();
            UpdateStatus("Servidor detenido.");
            Debug.Log("Servidor detenido");
        }
    }

    // Métodos para los InputField
    public void UpdateIP(string ip)
    {
        NetworkManager.singleton.networkAddress = ip;
        Debug.Log($"IP actualizada a: {ip}");
    }

    // Método para actualizar el estado mostrado
    private void UpdateStatus(string message)
    {
        if (statusText != null)
        {
            statusText.text = message;
        }
    }
}
