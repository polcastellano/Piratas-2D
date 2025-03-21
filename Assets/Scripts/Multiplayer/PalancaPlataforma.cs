using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class PalancaPlataforma : NetworkBehaviour
{
    public GameObject plataforma;
    public GameObject interactiveBtn;
    private bool activateInteract;
    enum Estados { Normal, Activada };
    private Animator p_animator;
    [SyncVar] public bool isOpened = false; // Estado de la puerta
    [SyncVar] public bool isActive = false; // Estado de la palanca

    private GameObject interactingPlayer; // Referencia al jugador que interactúa

    void Start()
    {
        p_animator = GetComponentInChildren<Animator>();
    }
    void Update()
    {
        // Solo el jugador que está interactuando puede activar la palanca
        if (activateInteract && interactingPlayer != null && interactingPlayer.GetComponent<NetworkIdentity>().isLocalPlayer)
        {
            if (Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.JoystickButton3))
            {
                CmdToggleDoor();
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            activateInteract = true;
            interactingPlayer = other.gameObject; // Establece al jugador que está interactuando
            interactiveBtn.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            activateInteract = false;
            interactingPlayer = null; // El jugador deja de interactuar
            interactiveBtn.SetActive(false);
        }
    }

    [Command(requiresAuthority = false)]
    void CmdToggleDoor()
    {
        isActive = !isActive; // Alterna el estado de la palanca
        RpcUpdateDoor(isActive); // Actualiza en todos los clientes
    }

    [ClientRpc]
    void RpcUpdateDoor(bool state)
    {
        if (plataforma != null)
        {
            p_animator.SetBool("Activada", state);
            BoxCollider2D collider = gameObject.GetComponent<BoxCollider2D>();
            collider.enabled = !state;
            interactiveBtn.SetActive(!state);
            plataforma.SetActive(state); // Activa o desactiva la puerta
        }
    }
}
