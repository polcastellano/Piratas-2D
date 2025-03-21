using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class Apple : MonoBehaviour
{
    public int HealQuantity;
    public bool activateInteract;
    public GameObject interactiveBtn;
    public PlayerManager player;
    public static bool eatingApple;
    public static bool absorbing;
    public Rigidbody2D rb;
    public GameObject effectPrefab;
    private SpriteRenderer spriteRenderer;
    public JackStatesMachine jackStateMachine;

    void Start()
        {
            if (player == null)
            {
                player = GameObject.FindWithTag("Player").GetComponent<PlayerManager>();
            }

            if(jackStateMachine == null)
            {
                jackStateMachine = FindObjectOfType<JackStatesMachine>();
            }

            rb = player.GetComponent<Rigidbody2D>();
        }

        void Update()
        {
            if (activateInteract && (Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.JoystickButton3)) && (rb.velocity.y == 0.0f) && (rb.velocity.x == 0.0f))
            {
                if (gameObject.tag == "Apple")
                {
                    eatingApple = true;
                    absorbing = true;
                    gameObject.SetActive(false);
                    player.health.Heal(HealQuantity);
                    player.audioManager.PlaySfx(2);

                    if (effectPrefab != null && player != null)
                    {
                        GameObject instantiatedEffect = Instantiate(effectPrefab, player.transform.position, Quaternion.identity);
                        Destroy(instantiatedEffect, 1.0f); // Destruir después de 1 segundo
                    }
                }
            }

        }
        public void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                interactiveBtn.SetActive(true);
                activateInteract = true;

                GameObject infBtn = interactiveBtn.transform.GetChild(2).gameObject;
                infBtn.SetActive(true);
            }
        }

        public void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                interactiveBtn.SetActive(false);
                activateInteract = false;
                absorbing = false;

                GameObject infBtn = interactiveBtn.transform.GetChild(2).gameObject;
                infBtn.SetActive(false);
            }
        }
    }
