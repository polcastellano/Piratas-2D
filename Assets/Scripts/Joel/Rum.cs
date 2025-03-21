using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class Rum : MonoBehaviour
{
    public int RumMinQuantity;
    public int RumMaxQuantity;
    public int RumQuantity;
    public bool activateInteract;
    public GameObject interactiveBtn;
    public PlayerManager player;
    public GameObject effectPrefab;
    public static bool absorbing;
    private SpriteRenderer spriteRenderer;
    public static bool drinkingRum;
    public Rigidbody2D rb;
    
    void Start()
    {
        if (player == null)
        {
            player = GameObject.FindWithTag("Player").GetComponent<PlayerManager>();
        }
        RumQuantity = Random.Range(RumMinQuantity, RumMaxQuantity);
        rb = player.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (activateInteract && (Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.JoystickButton3)) && (rb.velocity.y == 0.0f) && (rb.velocity.x == 0.0f))
        {
            if (gameObject.tag == "Rum")
            {
                drinkingRum = true;
                absorbing = true;
                gameObject.SetActive(false);
                player.ron.DrinkRon(RumQuantity);
                player.audioManager.PlaySfx(3);

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