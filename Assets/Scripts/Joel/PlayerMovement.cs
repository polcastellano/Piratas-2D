using System.Collections;
using System.Collections.Generic;

//using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    private string jacks_name = "Peter";
    public GameObject idleJack;
    public GameObject stairsJack;
    public GameObject multipleJack;
    public Rigidbody2D rb;
    public float moveHorizontal;
    public float moveVertical;
    public float speed = 10;
    public float climbStairsSpeed = 15;
    public float jumpForce = 19;
    public float dodgeForce = 10;
    public int enemyDamage = 5;
    public SpriteRenderer sp;
    public CapsuleCollider2D cc;
    public bool isFacingRight = true;
    public CheckGroundMP checkGroundMP;
    private bool isInsideTrigger = false;
    public bool isOnStair = false;
    public AudioManager audioManager;
    public float gravityQuantity = 5;
    public float thrust = 60f;
    public AudioSource stepsAudioSource;
    public AudioSource jumpAudioSource;
    public AudioSource ladderAudioSource;
    public AudioSource dodgeAudioSource;
    public AudioSource damageAudioSource;
    public bool isMoving;

    public PhysicsMaterial2D defaultMaterial; // Material por defecto
    public PhysicsMaterial2D lowFrictionMaterial; // Material de baja fricción

    public AudioClip[] jumpSounds;
    public AudioClip[] ladderSounds;

    private bool isTouchingWall = false;

    public Animator jackStairAnimator;
    public Animator jackMultipleAnimator;

    private bool isNearStair = false;
    public JackStatesMachine jackStateMachine;
    public bool imDodging = false;
    public bool attacking = false;
    // Nueva variable para controlar la inversión de controles
    private bool isControlInverted = false;
    public bool affected;

    public static float baseY;

    public PlayerManager player;

    private DataManager dataManager;
    private Vector2 hostalPosition = new Vector2(49, 12);

    private Vector2 newPosition;

    private const float verticalDeadzone = 0.2f; // Umbral para ignorar movimiento vertical leve del joystick
    
    private string jacks_not_moving_phrase = "Quizá debería consultar el cuaderno de misiones...";
    public Dialogue dialogue;
    public Ship ship;
    public AudioClip dialogueClip;
    public AudioSource waitingTimeAudioSource;
    private float elapsedTime = 0.0f; // Tiempo acumulativo

    public Image faceImage;
    public string faceImageSourceName;

    void Start()
    {
        if(dataManager == null)
        {
            dataManager = FindObjectOfType<DataManager>();
        }

        isOnStair = false;
        if(ship == null) {
            ship = FindObjectOfType<Ship>();
        }
        if(jackStateMachine == null)
        {
            jackStateMachine = FindObjectOfType<JackStatesMachine>();
        }
        multipleJack.SetActive(false);
        multipleJack.SetActive(true);

        if (player == null)
        {
            //player = FindObjectOfType<PlayerManager>();
            player = GameObject.FindWithTag("Player").GetComponent<PlayerManager>();
        }
        if(dataManager != null && dataManager.loadGame == true && SceneManager.GetActiveScene().buildIndex == 1) {
            StartCoroutine(HostalPlayer());
            
            //player.transform.position = hostalPosition;
            Debug.Log("Entra al IF loadGame");
        } else {
            player.audioManager.ReproduceSonidoJackEmpezar();
        }
    }
    

    private IEnumerator HostalPlayer()
    {
        Debug.Log("Antes de WaitForSeconds");
        yield return new WaitForSeconds(0.01f); // Ajusta el tiempo según la duración de la animación de dodge
        Debug.Log("Despues de WaitForSeconds");
        player.transform.position = hostalPosition;
        dataManager.loadGame = false;
    }

    // Método para activar la inversión aleatoria de controles en el eje X por 15 segundos
    public void ActivateRandomInvertedControls()
    {
        StartCoroutine(RandomInvertControlsCoroutine());
    }

    private IEnumerator RandomInvertControlsCoroutine()
    {
        float elapsedTime = 0f;

        while (elapsedTime < 15f)
        {
            // Determina aleatoriamente si los controles estarán invertidos o no
            isControlInverted = Random.Range(0, 2) == 0; // 50% de probabilidad de invertir

            // Cambiar el estado de inversión cada 1.5 segundos
            yield return new WaitForSeconds(1.5f);
            elapsedTime += 1.5f;
        }

        // Al finalizar los 15 segundos, restaurar el control normal
        isControlInverted = false;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.F1)) {
            ship.cargarBossAtajoTeclado();
        }
        if(waitingTimeAudioSource != null) {
            // Verificar si el movimiento horizontal es 0 y el personaje está en el suelo
            if (moveHorizontal == 0 && CheckGround.isGrounded)
            {
                // Incrementar el tiempo acumulativo
                elapsedTime += Time.deltaTime;

                // Si el tiempo acumulado supera los 10 segundos
                if (elapsedTime >= 90.0f)
                {
                    Debug.Log("-----> ABRIR CUADERNO DE MISIONES");

                    Sprite loadedSprite = Resources.Load<Sprite>("Images/HUD/CarasPersonajes/" + faceImageSourceName);
                    if (loadedSprite != null)
                    {
                        faceImage.sprite = loadedSprite;
                    }
                    else
                    {
                        Debug.LogWarning("No se encontró la imagen en Resources/Images/HUD/CarasPersonajes/" + faceImageSourceName);
                    }

                    waitingTimeAudioSource.Play(); 
                    dialogue.panel.SetActive(true);
                    dialogue.StartDialogue(jacks_not_moving_phrase, jacks_name);
                    StartCoroutine(DeactivatePanelAfterDelay());
                    elapsedTime = 0.0f; // Reiniciar el tiempo si es necesario después del mensaje
                }
            }
            else
            {
                // Reiniciar el tiempo acumulativo si las condiciones no se cumplen
                elapsedTime = 0.0f;
            }
        }
    }

    void FixedUpdate()
    {   
        moveHorizontal = Input.GetAxis("Horizontal");
        moveVertical = Input.GetAxis("Vertical");

        // Verificar si los controles están invertidos y aplicar la inversión en X
        if (isControlInverted)
        {
            moveHorizontal *= -1; // Invierte el movimiento en el eje X
        }

        if (isTouchingWall)
            cc.sharedMaterial = lowFrictionMaterial;
        else
            cc.sharedMaterial = defaultMaterial;

        // Movimiento horizontal
        rb.velocity = new Vector2(moveHorizontal * speed, rb.velocity.y);

        // Movimiento y sonidos
        FlipSpriteBasedOnDirection();
        PlayStepsSound();

        // Comportamiento escalera
        if (isOnStair)
        {
            // Permitir movimiento vertical en la escalera
            if (Mathf.Abs(moveVertical) > verticalDeadzone)
            {
                transform.position = new Vector2(newPosition.x, transform.position.y); 
                moveHorizontal = 0;
                stairsJack.SetActive(true);
                //multipleJack.SetActive(true);
                SetStairAnimation(true);

                rb.gravityScale = 0; // Sin gravedad en la escalera
                rb.velocity = new Vector2(moveHorizontal * speed, moveVertical * climbStairsSpeed); // Permitir movimiento lateral y vertical
                PlayRandomSound(ladderAudioSource, ladderSounds);
            }
            else
            {
                // Quieto en la escalera sin gravedad y con posibilidad de movimiento lateral
                rb.velocity = new Vector2(moveHorizontal * speed, 0);
                SetStairAnimation(false);

            }
        }
        else
        {
            rb.gravityScale = gravityQuantity;
            if (ladderAudioSource.isPlaying)
                ladderAudioSource.Stop();
        }

        // Verificar si el jugador está cerca de la escalera y se mueve verticalmente para "engancharse"
        if (isNearStair && Mathf.Abs(moveVertical) > verticalDeadzone)
        {
            isOnStair = true;
            rb.gravityScale = 0; // Sin gravedad al engancharse
        }
        else if (!isNearStair)
        {
            isOnStair = false;
        }
    }

    public void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        //jackStateMachine.jump = false;
        PlayRandomSound(jumpAudioSource, jumpSounds);   
    }

    public void Dodge()
    {
        imDodging = true;
        jackStateMachine.dodge = true;
        StartCoroutine(EndDodge());
    }

    private IEnumerator EndDodge()
    {
        yield return new WaitForSeconds(0.25f); // Ajusta el tiempo según la duración de la animación de dodge
        imDodging = false;
        jackStateMachine.dodge = false;
    }

        private IEnumerator EndBulletDamage()
    {
        yield return new WaitForSeconds(0.05f); // Ajusta el tiempo según la duración de la animación de dodge
        affected = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Floor"))
        {
            baseY = transform.position.y;
        }
        
        if(other.CompareTag("Sword")  || other.CompareTag("Trap") || other.CompareTag("BalaPistola"))
        {
            affected = true;
            player.health.Damage(enemyDamage);
        }
        if(other.CompareTag("BalaPistola"))
        {
            StartCoroutine(EndBulletDamage());
        }
        if (other.CompareTag("VerticalStair")){
            newPosition = new Vector2(other.transform.position.x, transform.position.y);
            isNearStair = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("VerticalStair"))
        {
            isNearStair = false;
            stairsJack.SetActive(false); // Desactiva el StairsJack
            if(isOnStair) {
                if(player.ron.actualRon < 100 && player.ron.actualRon >= 50.0f) {
                    jackMultipleAnimator.Play("Drunk_Idle", 0);
                } else {
                    jackMultipleAnimator.Play("New State", 0); // Vuelve al IDLE en MultipleJack para que se vuelva a mostrar
                }
            }
            isOnStair = false; // Desactiva el estado de escalera
        }

        if(other.CompareTag("Sword") || other.CompareTag("Trap"))
        {
            affected = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
            isTouchingWall = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
            isTouchingWall = false;
    }

    private void FlipSpriteBasedOnDirection()
    {
        if (moveHorizontal > 0)
        {
            sp.flipX = false;
            isMoving = true;
        }
        else if (moveHorizontal < 0)
        {
            sp.flipX = true;
            isMoving = true;
        }
        else
            isMoving = false;
    }

    private void PlayStepsSound()
    {
        if (isMoving && CheckGround.isGrounded && !stepsAudioSource.isPlaying)
            stepsAudioSource.Play();
        else if (!isMoving && stepsAudioSource.isPlaying)
            stepsAudioSource.Stop();
    }

    private void SetStairAnimation(bool climbing) // Si el jugador se encuentra en la escalera
    {
        if (jackStairAnimator != null)
        {
            jackStairAnimator.enabled = climbing;
            if (climbing)
                jackStairAnimator.Play("Subir_Escaleras", 0); // Activa animacion de subir escaleras
        }
        if (jackMultipleAnimator != null)
        {
            if (climbing)
                jackMultipleAnimator.Play("Stair", 0); // Cambia el estado del multipleJack a Stair para que su opacidad sea 0.
        }
    }

    public static void PlayRandomSound(AudioSource audioSource, AudioClip[] clips)
    {
        if (clips.Length > 0 && !audioSource.isPlaying)
        {
            int randomIndex = Random.Range(0, clips.Length);
            audioSource.clip = clips[randomIndex];
            audioSource.Play();
        }
    }

    private IEnumerator DeactivatePanelAfterDelay()
    {
        yield return new WaitForSeconds(5);
        dialogue.panel.SetActive(false); // Desactiva el panel de diálogo a los dos segundos de salir del collider del CrewMember
        dialogue.ClearButtons();
    }
}
