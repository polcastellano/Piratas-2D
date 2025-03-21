using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TavernDoor : NetworkBehaviour
{
    // public bool activateInteract;
    public int playersInTrigger = 0;
    public bool isLocalPlayerInTrigger = false;
    public GameObject interactiveBtn;
    public GameObject[] gorros;
    private int contador = -1;


    // Update is called once per frame
    void Update()
    {
        if (playersInTrigger >= 2 && isLocalPlayerInTrigger && (Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.JoystickButton3)))
        {
            CmdMenuScene();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playersInTrigger++;
            contador++;
            CmdActualizarGorros(true, gorros[contador]);
            if (other.gameObject.GetComponent<Mirror.NetworkIdentity>().isLocalPlayer)
            {
                isLocalPlayerInTrigger = true;
                if (playersInTrigger >= 2)
                {
                    CmdActivarBtn(true);
                }
            }

        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playersInTrigger--;
            if (other.gameObject.GetComponent<Mirror.NetworkIdentity>().isLocalPlayer)
            {
                isLocalPlayerInTrigger = false;
                CmdActivarBtn(false);
                CmdActualizarGorros(false, gorros[contador]);
            }
            contador--;
        }
    }

    [Command(requiresAuthority = false)]
    void CmdMenuScene()
    {
        RpcMenuScene(); // Actualiza en todos los clientes
    }

    [ClientRpc]
    void RpcMenuScene()
    {
        BoxCollider2D collider = gameObject.GetComponent<BoxCollider2D>();
        collider.enabled = false;
        interactiveBtn.SetActive(false);
        SceneManager.LoadScene("MenuPrincipal");
    }

    [Command(requiresAuthority = false)]
    void CmdActivarBtn(bool state)
    {
        RpcActivarBtn(state); // Actualiza en todos los clientes
    }

    [ClientRpc]
    void RpcActivarBtn(bool state)
    {
        interactiveBtn.SetActive(state);
    }

    [Command(requiresAuthority = false)]
    void CmdActualizarGorros(bool state, GameObject gorro)
    {
        RpcActualizarGorros(state, gorro); // Actualiza en todos los clientes
    }

    [ClientRpc]
    void RpcActualizarGorros(bool state, GameObject gorro)
    {
        gorro.GetComponent<Animator>().enabled = state;
    }

}
