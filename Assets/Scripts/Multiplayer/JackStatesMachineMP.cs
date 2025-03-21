using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using Unity.VisualScripting;
using UnityEngine;

public class JackStatesMachineMP : NetworkBehaviour
{
    enum Estados { Idle, Jump, Run };
    public Animator e_animator;
    [SerializeField] Estados e_Estados;
    public CheckGroundMP checkGroundMP;

    // ESTADOS NORMALES
    public bool idle = false;
    public bool jump = false;
    public bool run = false;

    //public bool touched = false;

    Rigidbody2D rb;
    public PlayerMovementMP playerMovement;

    public PlayerManager player;
    // Contador que llevará el tiempo transcurrido
    public GameObject actualJack;
    //private float timer = 0f;

    void Start()
    {

        e_Estados = Estados.Idle;
        rb = gameObject.GetComponentInParent<Rigidbody2D>();

        if (player == null)
        {
            //player = FindObjectOfType<PlayerManager>();
            player = GameObject.FindWithTag("Player").GetComponent<PlayerManager>();
        }
    }

    void Update()
    {
        if (!isLocalPlayer) { return; }
        Estados estadoAnterior = e_Estados; // Guardamos el estado actual

        switch (e_Estados)
        {
            case Estados.Idle:
                // Activar el salto cuando se presiona "space" y el personaje está en el suelo
                if (rb.velocity.y != 0.0f && !checkGroundMP.isGrounded)
                {
                    //Debug.Log("ESTOY VOLANDO");
                    e_Estados = Estados.Jump;
                }
                else if ((rb.velocity.y == 0.0f) && (rb.velocity.x != 0.0f) && checkGroundMP.isGrounded)
                {
                    e_Estados = Estados.Run;
                }
                break;
            case Estados.Jump:
                if ((rb.velocity.y == 0.0f) && checkGroundMP.isGrounded)
                {
                    e_Estados = Estados.Idle;
                }
                else if (rb.velocity.x != 0.0f)
                {
                    e_Estados = Estados.Run;
                }
                break;
            case Estados.Run:
                if ((rb.velocity.y == 0.0f) && (rb.velocity.x == 0.0f) && checkGroundMP.isGrounded)
                {
                    e_Estados = Estados.Idle;
                }
                else if (rb.velocity.y != 0.0f && !checkGroundMP.isGrounded)
                {
                    e_Estados = Estados.Jump;
                }
                break;
        }

        switch (e_Estados)
        {
            case Estados.Idle:
                e_animator.SetInteger("Estado", 0);
                if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.JoystickButton0)) && checkGroundMP.isGrounded)
                {
                    playerMovement.Jump();
                }
                break;
            case Estados.Jump:
                e_animator.SetInteger("Estado", 3);
                break;
            case Estados.Run:
                e_animator.SetInteger("Estado", 7);
                if (playerMovement.moveHorizontal != 0)
                {
                    rb.velocity = new Vector2(playerMovement.moveHorizontal * playerMovement.speed, rb.velocity.y);
                }
                if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.JoystickButton0)) && checkGroundMP.isGrounded)
                {
                    playerMovement.Jump();
                }
                break;
        }
    }

    void FixedUpdate()
    {
        if (!isLocalPlayer) { return; }
        if (playerMovement.moveHorizontal > 0)
        {
            CmdFlipJack(true);
        }
        else if (playerMovement.moveHorizontal < 0)
        {
            CmdFlipJack(false);
        }
    }

    void ReproduceJumpSoundEffect()
    {
        player.audioManager.PlaySfx(9);
    }

    [Command(requiresAuthority = false)]
    void CmdFlipJack(bool state)
    {
        RpcFlipJack(state); // Actualiza en todos los clientes
    }

    [ClientRpc]
    void RpcFlipJack(bool state)
    {
        if (state)
        {
            actualJack.transform.localScale = new Vector3(Mathf.Abs(actualJack.transform.localScale.x), actualJack.transform.localScale.y);
        }
        else
        {
            actualJack.transform.localScale = new Vector3(-Mathf.Abs(actualJack.transform.localScale.x), actualJack.transform.localScale.y);

        }
    }
}