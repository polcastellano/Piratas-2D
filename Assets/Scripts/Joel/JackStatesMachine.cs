using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class JackStatesMachine : MonoBehaviour
{
    enum Estados { Idle, Jump, Run, Dodge, EatAndDrink, Damage, Attack_1, Drunk_Idle, Drunk_Jump, Drunk_Run, Drunk_Dodge, Drunk_EatAndDrink, Drunk_Damage, Drunk_Attack_1 };
    public Animator e_animator;
    [SerializeField] Estados e_Estados;
    public GameObject effectObject; // Asignar el GameObject que deseas activar

    // ESTADOS NORMALES
    public bool idle = false;
    public bool jump = false;
    public bool run = false;
    public bool drunk = false;
    public bool dodge = false;
    public bool attack_1 = false;
    public bool eatAndDrink = false;
    public bool damage = false;

    // ESTADOS BORRACHOS
    public bool drunk_idle = false;
    public bool drunk_jump = false;
    public bool drunk_run = false;
    public bool drunk_dodge = false;
    public bool drunk_attack_1 = false;
    public bool drunk_eatAndDrink = false;
    public bool drunk_damage = false;

    public bool touched = false;

    Vector2 vectorimpulso;
    Rigidbody2D rb;
    public PlayerMovement playerMovement;
    public StepsSound stepsSound;
    public Ron ron;
    public PlayerManager player;
    public GameObject actualJack;
    private float timer = 0f;


    // Maxima variacion del eje Y
    private float tolerance = 0.2f;
    private bool littleDodge = false;

    void Start()
    {
        if (ron == null)
        {
            ron = FindAnyObjectByType<Ron>();
        }
        e_Estados = Estados.Idle;

        rb = gameObject.GetComponentInParent<Rigidbody2D>();

        if (player == null)
        {
            player = GameObject.FindWithTag("Player").GetComponent<PlayerManager>();
        }
        if(stepsSound == null) // Encuentra el DataManager de forma dinamica si no esta declarado
        {
            stepsSound = FindObjectOfType<StepsSound>();
        }
        
        // Recogemos el valor de Y inicial
        PlayerMovement.baseY = transform.position.y;
    }

    void Update()
    {
        float currentY = transform.position.y;
        if(ron != null) {
            if (ron.actualRon >= 50.0f) //ron.actualRon < 100 && ron.actualRon >= 50.0f)
            {
                drunk = true;
                //Debug.Log("--> ESTADO DEL DRUNK: " + drunk);
            }
            else
            {
                drunk = false;
                //Debug.Log("-----> ESTADO DEL DRUNK: " + drunk);
            }
        }
        
        Estados estadoAnterior = e_Estados; // Guardamos el estado actual

        switch (e_Estados)
        {
            case Estados.Idle:
                if(drunk == false)
                {
                    if (dodge)
                    {
                        e_Estados = Estados.Dodge;
                    }
                    else if (CheckGround.isGrounded == false && Mathf.Abs(currentY - PlayerMovement.baseY) > tolerance && dodge == false)
                    {
                        e_Estados = Estados.Jump;
                    }
                    else if (damage)
                    {
                        e_Estados = Estados.Damage;
                    }
                    else if (attack_1)
                    {
                        e_Estados = Estados.Attack_1;
                    }
                    else if (Mathf.Abs(currentY - PlayerMovement.baseY) > tolerance && playerMovement.moveHorizontal == 0 && (Apple.eatingApple || Rum.drinkingRum))
                    {
                        e_Estados = Estados.EatAndDrink;
                    }

                    else if (playerMovement.moveHorizontal != 0 && CheckGround.isGrounded)
                    {
                        e_Estados = Estados.Run;
                    }
                }
                else
                {
                    e_Estados = Estados.Drunk_Idle;
                }
            break;
            case Estados.Drunk_Idle:
                if(drunk == false)
                {
                    e_Estados = Estados.Idle;
                }
                else if(drunk == true)
                {
                    if (dodge)
                    {
                        e_Estados = Estados.Drunk_Dodge;
                    }
                    else if (CheckGround.isGrounded == false && Mathf.Abs(currentY - PlayerMovement.baseY) > tolerance)
                    {
                        e_Estados = Estados.Drunk_Jump;
                    }
                    else if (drunk_damage)
                    {
                        e_Estados = Estados.Drunk_Damage;
                    }
                    else if (attack_1)
                    {
                        e_Estados = Estados.Drunk_Attack_1;
                    }
                    else if(Mathf.Abs(currentY - PlayerMovement.baseY) > tolerance && playerMovement.moveHorizontal == 0 && (Apple.eatingApple || Rum.drinkingRum))
                    {
                        e_Estados = Estados.Drunk_EatAndDrink;
                    }
                    else if (playerMovement.moveHorizontal != 0 && CheckGround.isGrounded)
                    {
                        Debug.Log("DrunkRun");
                        e_Estados = Estados.Drunk_Run;
                    }
                }
            break;
            case Estados.Jump:
                if (playerMovement.moveHorizontal == 0 && CheckGround.isGrounded)
                {
                    e_Estados = Estados.Idle;
                }
                else if (playerMovement.moveHorizontal != 0 && CheckGround.isGrounded)
                {
                    e_Estados = Estados.Run;
                }
            break;
            case Estados.Drunk_Jump:
                if (drunk == false)
                {
                    e_Estados = Estados.Jump;   
                }
                else if(drunk == true)
                {
                    if (playerMovement.moveHorizontal == 0 && CheckGround.isGrounded)
                    {
                        e_Estados = Estados.Drunk_Idle;
                    }
                    else if (playerMovement.moveHorizontal != 0 && CheckGround.isGrounded)
                    {
                        e_Estados = Estados.Drunk_Run;
                    }
                }
            break;
            case Estados.Run:
                if (playerMovement.moveHorizontal == 0 && CheckGround.isGrounded)
                {
                    e_Estados = Estados.Idle;
                }
                else if (dodge)
                {
                    e_Estados = Estados.Dodge;
                }
                else if (attack_1)
                {
                    e_Estados = Estados.Attack_1;
                }
                else if (CheckGround.isGrounded == false && Mathf.Abs(currentY - PlayerMovement.baseY) > tolerance)
                {
                    e_Estados = Estados.Jump;
                }
            break;
            case Estados.Damage:
                if (CheckGround.isGrounded && Mathf.Abs(currentY - PlayerMovement.baseY) > tolerance)
                {
                    e_Estados = Estados.Jump;
                }
                else if (playerMovement.moveHorizontal != 0 && CheckGround.isGrounded)
                {
                    e_Estados = Estados.Run;
                }
                else if (playerMovement.moveHorizontal == 0 && CheckGround.isGrounded)
                {
                    e_Estados = Estados.Idle;
                }
            break;
            case Estados.Drunk_Damage:
                if(drunk == false)
                {
                    e_Estados = Estados.Damage;
                }
                else if(drunk == true)
                {
                    if (CheckGround.isGrounded == false && Mathf.Abs(currentY - PlayerMovement.baseY) > tolerance)
                    {
                        e_Estados = Estados.Drunk_Jump;
                    }
                    else if (playerMovement.moveHorizontal != 0 && CheckGround.isGrounded)
                    {
                        e_Estados = Estados.Drunk_Run;
                    }
                    else if (playerMovement.moveHorizontal == 0 && CheckGround.isGrounded)
                    {
                        e_Estados = Estados.Drunk_Idle;
                    }
                }
            break;
            case Estados.Attack_1:
                if (CheckGround.isGrounded == false && Mathf.Abs(currentY - PlayerMovement.baseY) > tolerance)
                {
                    e_Estados = Estados.Jump;
                }
                else if (playerMovement.moveHorizontal == 0 && CheckGround.isGrounded)
                {
                    e_Estados = Estados.Idle;
                }
                else if (playerMovement.moveHorizontal != 0 && CheckGround.isGrounded)
                {
                    e_Estados = Estados.Run;
                }
            break;
            case Estados.Drunk_Attack_1:
                if(drunk == false)
                {
                    e_Estados = Estados.Attack_1;
                }
                else if(drunk == true)
                {
                    if (CheckGround.isGrounded == false && Mathf.Abs(currentY - PlayerMovement.baseY) > tolerance)
                    {
                        e_Estados = Estados.Drunk_Jump;
                    }
                    else if (playerMovement.moveHorizontal == 0 && CheckGround.isGrounded)
                    {
                        e_Estados = Estados.Drunk_Idle;
                    }
                    else if (playerMovement.moveHorizontal != 0 && CheckGround.isGrounded)
                    {
                        e_Estados = Estados.Drunk_Run;
                    }
                }
            break;
            case Estados.Drunk_Run:
                if(drunk == false)
                {
                    if (playerMovement.moveHorizontal == 0 && CheckGround.isGrounded)
                    {
                        e_Estados = Estados.Run;
                    }
                }else if(drunk == true)
                {
                    if (playerMovement.moveHorizontal == 0 && CheckGround.isGrounded)
                    {
                        e_Estados = Estados.Drunk_Idle;
                    }
                    else if (attack_1)
                    {
                        e_Estados = Estados.Drunk_Attack_1;
                    }
                    else if (CheckGround.isGrounded == false && Mathf.Abs(currentY - PlayerMovement.baseY) > tolerance)
                    {
                        e_Estados = Estados.Drunk_Jump;
                    } 
                }
            break;
            case Estados.Dodge:
                if(drunk == false)
                {
                    if(Mathf.Abs(currentY - PlayerMovement.baseY) > tolerance && playerMovement.moveHorizontal == 0 && (Apple.eatingApple || Rum.drinkingRum))
                    {
                        e_Estados = Estados.EatAndDrink;
                    }
                    else if (attack_1)
                    {
                        e_Estados = Estados.Attack_1;
                    }
                    else if (playerMovement.moveHorizontal != 0 && CheckGround.isGrounded)
                    {
                        e_Estados = Estados.Run;
                    }
                    else if (playerMovement.moveHorizontal == 0 && CheckGround.isGrounded && (Apple.eatingApple == false || Rum.drinkingRum == false))
                    {
                        e_Estados = Estados.Idle;
                    }
                }
                else if(drunk == true)
                {
                    e_Estados = Estados.Drunk_Dodge;
                }
            break;
            case Estados.Drunk_Dodge:
                if(drunk == false)
                {
                    e_Estados = Estados.Dodge;
                }
                else
                {
                    if(Mathf.Abs(currentY - PlayerMovement.baseY) > tolerance && playerMovement.moveHorizontal == 0 && (Apple.eatingApple || Rum.drinkingRum))
                    {
                        e_Estados = Estados.Drunk_EatAndDrink;
                    }
                    else if (attack_1)
                    {
                        e_Estados = Estados.Drunk_Attack_1;
                    }
                    else if (playerMovement.moveHorizontal != 0 && CheckGround.isGrounded)
                    {
                        e_Estados = Estados.Drunk_Run;
                    }
                    else if (playerMovement.moveHorizontal == 0 && CheckGround.isGrounded && (Apple.eatingApple == false || Rum.drinkingRum == false))
                    {
                        e_Estados = Estados.Drunk_Idle;
                    }
                }
            break;
            case Estados.EatAndDrink:
                if(drunk == true)
                {
                    if (CheckGround.isGrounded == false && Mathf.Abs(currentY - PlayerMovement.baseY) > tolerance)
                    {
                        e_Estados = Estados.Drunk_Jump;
                    }
                    else if (playerMovement.moveHorizontal != 0 && CheckGround.isGrounded)
                    {
                        e_Estados = Estados.Drunk_Run;
                    }
                    else if (playerMovement.moveHorizontal == 0 && CheckGround.isGrounded && (Apple.eatingApple == false || Rum.drinkingRum == false))
                    {
                        e_Estados = Estados.Drunk_Idle;
                    }
                }
                else if(drunk == false)
                {
                    if (CheckGround.isGrounded == false && Mathf.Abs(currentY - PlayerMovement.baseY) > tolerance)
                    {
                        e_Estados = Estados.Jump;
                    }
                    else if (playerMovement.moveHorizontal != 0 && CheckGround.isGrounded)
                    {
                        e_Estados = Estados.Run;
                    }
                    else if (playerMovement.moveHorizontal == 0 && CheckGround.isGrounded && (Apple.eatingApple == false || Rum.drinkingRum == false))
                    {
                        e_Estados = Estados.Idle;
                    }
                }
            break;
            case Estados.Drunk_EatAndDrink:
                if(drunk == false)
                {
                    if (CheckGround.isGrounded == false && Mathf.Abs(currentY - PlayerMovement.baseY) > tolerance)
                    {
                        e_Estados = Estados.Jump;
                    }
                    else if (playerMovement.moveHorizontal != 0 && CheckGround.isGrounded)
                    {
                        e_Estados = Estados.Run;
                    }
                    else if (playerMovement.moveHorizontal == 0 && CheckGround.isGrounded && (Apple.eatingApple == false || Rum.drinkingRum == false))
                    {
                        e_Estados = Estados.Idle;
                    }
                }
                else if(drunk == true)
                {
                    if (CheckGround.isGrounded == false && Mathf.Abs(currentY - PlayerMovement.baseY) > tolerance)
                    {
                        e_Estados = Estados.Drunk_Jump;
                    }
                    else if (playerMovement.moveHorizontal != 0 && CheckGround.isGrounded)
                    {
                        e_Estados = Estados.Drunk_Run;
                    }
                    else if (playerMovement.moveHorizontal == 0 && CheckGround.isGrounded && (Apple.eatingApple == false || Rum.drinkingRum == false))
                    {
                        e_Estados = Estados.Drunk_Idle;
                    }
                }
            break;
        }

        switch (e_Estados)
        {
            case Estados.Idle:
                e_animator.SetInteger("Estado", 0);
                if (Dialogue.isDialogueOptionsActive == false) { 
                    if ((Input.GetKeyDown(KeyCode.Space) || (Input.GetKeyDown(KeyCode.Z)) || Input.GetKeyDown(KeyCode.JoystickButton0)) && CheckGround.isGrounded && Building.buildingPanelActive == false)
                    {
                        playerMovement.Jump();
                    }
                }
                if ((Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.JoystickButton2)) && CheckGround.isGrounded)
                {
                    attack_1 = true;
                }
                if (Input.GetKeyDown(KeyCode.C) || Input.GetKeyDown(KeyCode.JoystickButton1) && CheckGround.isGrounded)
                {
                    dodge = true;
                }
                if (playerMovement.affected)
                {
                    damage = true;
                }
            break;
            case Estados.Dodge:
                e_animator.SetInteger("Estado", 1);
                StartCoroutine(HandleDodge());
            break;
            case Estados.Drunk_Dodge:
                e_animator.SetInteger("Estado", 11);
                StartCoroutine(HandleDodge());
            break;
            case Estados.EatAndDrink:
                e_animator.SetInteger("Estado", 2);
                StartCoroutine(HandleEatAndDrink());
            break;
            case Estados.Jump:
                e_animator.SetInteger("Estado", 3);
            break;
            case Estados.Damage:
                e_animator.SetInteger("Estado", 4);
                StartCoroutine(HandleDamage());
            break;
            
            case Estados.Drunk_Damage:
                e_animator.SetInteger("Estado", 14);
                StartCoroutine(HandleDamage());
            break;
           
            case Estados.Attack_1:
                e_animator.SetInteger("Estado", 6);
                StartCoroutine(HandleAttack());
            break;
            case Estados.Run:
                e_animator.SetInteger("Estado", 7);
                if (Input.GetKeyDown(KeyCode.C) || Input.GetKeyDown(KeyCode.JoystickButton1) && CheckGround.isGrounded)
                {
                    dodge = true;
                }
                if (playerMovement.moveHorizontal != 0)
                {
                    rb.velocity = new Vector2(playerMovement.moveHorizontal * playerMovement.speed, rb.velocity.y);
                }
                if ((Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.JoystickButton2)) && CheckGround.isGrounded)
                {
                    attack_1 = true;
                }
                if ((Input.GetKeyDown(KeyCode.Space) || (Input.GetKeyDown(KeyCode.Z)) ||  Input.GetKeyDown(KeyCode.JoystickButton0)) && CheckGround.isGrounded)
                {
                    playerMovement.Jump();
                }
            break;
            case Estados.Drunk_Idle:
                e_animator.SetInteger("Estado", 10);
                if ((Input.GetKeyDown(KeyCode.Space) || (Input.GetKeyDown(KeyCode.Z)) || Input.GetKeyDown(KeyCode.JoystickButton0)) && CheckGround.isGrounded && Building.buildingPanelActive == false)
                {
                    playerMovement.Jump();
                }
                if ((Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.JoystickButton2)) && CheckGround.isGrounded)
                {
                    attack_1 = true;
                }
                if (Input.GetKeyDown(KeyCode.C) || Input.GetKeyDown(KeyCode.JoystickButton1) && CheckGround.isGrounded)
                {
                    dodge = true;
                }
                if (playerMovement.affected)
                {
                    drunk_damage = true;
                }
            break;
            case Estados.Drunk_Jump:
                e_animator.SetInteger("Estado", 13);
            break;
            case Estados.Drunk_Run:
                e_animator.SetInteger("Estado", 17);
                if ((Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.JoystickButton2)) && CheckGround.isGrounded)
                {
                    attack_1 = true;
                }
                if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Z) ||Input.GetKeyDown(KeyCode.JoystickButton0)) && CheckGround.isGrounded)
                {
                    playerMovement.Jump();
                }
            break;
            case Estados.Drunk_EatAndDrink:
                e_animator.SetInteger("Estado", 12);
                StartCoroutine(HandleDrunkEatAndDrink());
            break;
            case Estados.Drunk_Attack_1:
                e_animator.SetInteger("Estado", 16);
                StartCoroutine(HandleAttack());
            break;
        }
    }
    IEnumerator HandleEatAndDrink()
    {
        
        yield return new WaitForSeconds(1.15f); // Duración de la animación
        //e_Estados = Estados.Idle; // Vuelve al estado Idle
        Apple.eatingApple = false; // Evita reingresos constantes al estado
        Rum.drinkingRum = false;
    }

    IEnumerator HandleDrunkEatAndDrink()
    {
        yield return new WaitForSeconds(1.15f); // Duración de la animación
        //e_Estados = Estados.Drunk_Idle; // Vuelve al estado Idle
        Apple.eatingApple = false; // Evita reingresos constantes al estado
        Rum.drinkingRum = false;
    }
    IEnumerator HandleDamage()
    {
        yield return new WaitForSeconds(0.4f); // Duración de la animación
        //e_Estados = Estados.Idle; // Vuelve al estado Idle
        damage = false;
        drunk_damage = false;
    }

    IEnumerator HandleDrunkDamage()
    {
        yield return new WaitForSeconds(0.4f);
        damage = false;
    }

    IEnumerator HandleAttack()
    {
        attack_1 = false;
        yield return new WaitForSeconds(0.4f); // Duración de la animación
    }

    IEnumerator HandleDodge()
    {
            // Instanciar el prefab en la posición del jugador
        if (effectObject != null)
        {
            GameObject instantiatedEffect = Instantiate(effectObject, transform.position, Quaternion.identity);
            
            // Destruir el prefab después de 0.15 segundos (duración del dodge)
            Destroy(instantiatedEffect, 1f);
        }

        yield return new WaitForSeconds(0.15f); // Duración de la animación
        dodge = false;
    }

    void FixedUpdate()
    {
        if (dodge == true)
        {
            if (actualJack.transform.localScale.x > 0)
            {
                rb.AddForce(new Vector2(-playerMovement.dodgeForce, 0), ForceMode2D.Impulse);
                
            }
            else if (actualJack.transform.localScale.x < 0)
            {
                rb.AddForce(new Vector2(playerMovement.dodgeForce, 0), ForceMode2D.Impulse);
            }
        }

        if (playerMovement.moveHorizontal > 0)
        {
            actualJack.transform.localScale = new Vector2(Mathf.Abs(actualJack.transform.localScale.x), actualJack.transform.localScale.y);
        }
        else if (playerMovement.moveHorizontal < 0)
        {
            actualJack.transform.localScale = new Vector3(-Mathf.Abs(actualJack.transform.localScale.x), actualJack.transform.localScale.y);
        }
    }

    void ReproduceAttackSoundEffect()
    {
        player.audioManager.PlaySfx(6);
    }

    void ReproduceDamageSoundEffect()
    {
        player.audioManager.PlaySfx(7);
    }

    void ReproduceDodgeSoundEffect()
    {
        player.audioManager.PlaySfx(8);
    }

    void ReproduceJumpSoundEffect()
    {
        player.audioManager.PlaySfx(9);
    }
    void ReproduceStepSoundEffect()
    {
        if(stepsSound != null)
        {
            stepsSound.PlayStepSound();
        }
    }
}