using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RandomItemOnBox : MonoBehaviour
{
    // Sprite de la caja
    public SpriteRenderer boxSpriteRenderer;
    public bool isBoxBroken = false;
    // Posicion Y del prefab instanciado
    public float yOffset = 1.0f; 

    // Prefabs a instanciar
    public GameObject coin;
    public GameObject apple;
    public GameObject rum;

    public string brokenBoxRoute;

    public ParticleSystem boxParticle;

    public AudioSource audioSource;
    public AudioClip[] boxBreakSounds;

    public bool instantiateCoin;
    public bool instantiateApple;
    public bool instantiateRum;
    // Nuevo objeto que se desactivará al romper la caja
    public GameObject objectToDisable;

    public void OnTriggerEnter2D(Collider2D other)
    {
        // Si la espada del jack toca la caja, se desactiva el sprite y se instancia un RANDOM PREGAB
        if (other.CompareTag("SwordJack") && isBoxBroken == false)
        {
            if(boxSpriteRenderer != null)
            {
                Sprite loadedSprite = Resources.Load<Sprite>("Images/" + brokenBoxRoute);
                SpriteRenderer breakableBoxSprite = gameObject.GetComponent<SpriteRenderer>();
                breakableBoxSprite.sprite = loadedSprite;
            }

            // Detecta si esta caja tiene asignada un objeto a instanciar definido para llamar a la funcion que lo crea.
            if(instantiateCoin == true || instantiateApple == true || instantiateRum == true) {
                InstantiateItemOnBreakableBox();
            } else { // Si no esta declarado el objeto a generar genera un objeto de forma aleatoria.
                InstantiateRandomItemOnBreakableBox();
            }

            if (boxParticle != null)
            {
                boxParticle.Play();
            }
            PlayRandomBoxBreakSound();
             // Desactivar el objeto pasado como referencia
            if (objectToDisable != null)
            {
                objectToDisable.SetActive(false);
                Debug.Log($"Objeto {objectToDisable.name} desactivado.");
            }
        }
    }
    public void InstantiateItemOnBreakableBox()
    {
        Vector3 spawnPosition = transform.position + new Vector3(0, yOffset, 0);
        if(instantiateCoin == true) {
            Instantiate(coin, spawnPosition, Quaternion.identity);
        } else if(instantiateApple == true) {
            Instantiate(apple, spawnPosition, Quaternion.identity);
        } else if(instantiateRum == true) {
            Instantiate(rum, spawnPosition, Quaternion.identity);
        }
        isBoxBroken = true;
    }
    
    public void InstantiateRandomItemOnBreakableBox()
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
        isBoxBroken = true;
    }

     private void PlayRandomBoxBreakSound()
    {
        if (audioSource != null && boxBreakSounds.Length > 0)
        {
            int randomIndex = Random.Range(0, boxBreakSounds.Length);
            audioSource.clip = boxBreakSounds[randomIndex];
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning("No se encontró un AudioSource o no hay clips asignados.");
        }
    }

}
