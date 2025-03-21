using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RandomItemOnChest : MonoBehaviour
{
    public bool activateInteract;
    public bool isChestOpened = false;
    public GameObject interactiveBtn;
    
    // Posicion Y del prefab instanciado
    public float yOffset = 1.0f; 

    // Prefabs a instanciar
    public GameObject coin;
    public GameObject apple;
    public GameObject rum;
    public string openedChestRoute;

    public AudioSource audioSource;  // AudioSource del objeto
    public AudioClip[] boxOpenSounds;

    public GameObject ChestLight2D;
    public GameObject particleChest;

    public bool instantiateCoin;
    public bool instantiateApple;
    public bool instantiateRum;

    void Update()
    {
        if(activateInteract && (Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.JoystickButton3)) && isChestOpened == false)
        {
            Sprite loadedSprite = Resources.Load<Sprite>("Images/" + openedChestRoute);
            SpriteRenderer chestSprite = gameObject.GetComponent<SpriteRenderer>();
            chestSprite.sprite = loadedSprite;
            
            // Detecta si este cofre tiene asignado un objeto a instanciar definido para llamar a la funcion que lo crea.
            if(instantiateCoin == true || instantiateApple == true || instantiateRum == true) {
                InstantiateItem();
            } else { // Si no esta declarado el objeto a generar generara un objeto de forma aleatoria.
                InstantiateRandomItem();
            }

            interactiveBtn.SetActive(false);
            // Reproducir un sonido aleatorio si hay AudioSource y clips asignados
            PlayRandomChestOpenSound();
                // Activar la luz
            if (ChestLight2D != null)
            {
                ChestLight2D.SetActive(true);
                StartCoroutine(DeactivateChestLightAfterDelay(1.1f));
            }
             if (particleChest != null)
            {
                particleChest.SetActive(true);
            }
        }
    }

    public void InstantiateItem()
    {
        Vector3 spawnPosition = transform.position + new Vector3(0, yOffset, 0);

        if(instantiateCoin == true) {
            Instantiate(coin, spawnPosition, Quaternion.identity);
        } else if(instantiateApple == true) {
            Instantiate(apple, spawnPosition, Quaternion.identity);
        } else if(instantiateRum == true) {
            Instantiate(rum, spawnPosition, Quaternion.identity);
        }
        isChestOpened = true;
    }
    // Funcion para instanciar RON, MONEDAS O MANZANA de forma RANDOM
    public void InstantiateRandomItem()
    {
        int itemNum = Random.Range(1,4);
        Vector3 spawnPosition = transform.position + new Vector3(0, yOffset, 0);

        switch(itemNum)
        {
            case 1:
                Instantiate(coin, spawnPosition, Quaternion.identity);
            break;
            case 2:
                Instantiate(apple, spawnPosition, Quaternion.identity);
            break;
            case 3:
                Instantiate(rum, spawnPosition, Quaternion.identity);
            break;
        }
        isChestOpened = true;
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if(isChestOpened == false)
            {
                interactiveBtn.SetActive(true);
                activateInteract = true;
                
                GameObject infBtn = interactiveBtn.transform.GetChild(2).gameObject;
                infBtn.SetActive(true);
            }
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            interactiveBtn.SetActive(false);
            activateInteract = false;
            
            GameObject infBtn = interactiveBtn.transform.GetChild(2).gameObject;
            infBtn.SetActive(false);
        }
    }

    private void PlayRandomChestOpenSound()
    {
        if (audioSource != null && boxOpenSounds.Length > 0)
        {
            int randomIndex = Random.Range(0, boxOpenSounds.Length);
            audioSource.clip = boxOpenSounds[randomIndex];
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning("No se encontró un AudioSource o no hay clips asignados.");
        }
    }
        private IEnumerator DeactivateChestLightAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (ChestLight2D != null)
        {
            ChestLight2D.SetActive(false);
        }
    }
}
