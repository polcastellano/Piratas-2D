using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class Coin : MonoBehaviour
{
    public int coinMinQuantity;
    public int coinMaxQuantity;
    private int coinQuantity;
    public bool activateInteract;
    public GameObject interactiveBtn;
    public PlayerManager player;

    public GameObject prefabToInstantiate;
    private float particleDuration = 0.6f;

    void Start()
    {
        // Encuentra autom�ticamente al objeto PlayerManager en la escena y lo asigna a player
        if (player == null)
        {
            //player = FindObjectOfType<PlayerManager>();
            player = GameObject.FindWithTag("Player").GetComponent<PlayerManager>();
        }

        coinQuantity = Random.Range(coinMinQuantity, coinMaxQuantity); // Randomiza el numero de monedas que te da una moneda entre el minimo y maximo establecido
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            interactiveBtn.SetActive(true);
            activateInteract = true;
            // Instancia el prefab en la posición del objeto
            if (prefabToInstantiate != null)
            {
                GameObject particleInstance = Instantiate(prefabToInstantiate, transform.position, Quaternion.identity);
                 // Destruye el prefab después de 0.5 segundos
                Destroy(particleInstance, particleDuration);
            }
            gameObject.SetActive(false);
            player.coins.obtainCoins(coinQuantity);
            player.audioManager.PlaySfx(0);
            //Debug.Log("Ha entrado");
        }
    }
}
