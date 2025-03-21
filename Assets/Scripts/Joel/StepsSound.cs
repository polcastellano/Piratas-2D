using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepsSound : MonoBehaviour
{
    public AudioClip[] dirtSounds;
    public AudioClip[] woodSounds;
    public AudioClip[] stoneSounds;
    public AudioSource audioSource;

    private AudioClip[] currentSounds;

    void Start()
    {
        // Inicializa con un conjunto predeterminado (por ejemplo, piedra)
        currentSounds = stoneSounds;
    }

    public void PlayStepSound()
    {
        if (currentSounds.Length > 0)
        {
            AudioClip stepSound = currentSounds[Random.Range(0, currentSounds.Length)];
            audioSource.PlayOneShot(stepSound);
            //Debug.Log("Reproduce sonido: " + stepSound);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        MaterialType materialType = other.GetComponent<MaterialType>();
        if (materialType != null)
        {
            switch (materialType.type)
            {
                case MaterialType.Material.Dirt:
                    currentSounds = dirtSounds;
                    break;
                case MaterialType.Material.Wood:
                    currentSounds = woodSounds;
                    break;
                case MaterialType.Material.Stone:
                    currentSounds = stoneSounds;
                    break;
            }
        }
    }
}
