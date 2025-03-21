using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalDoorBoss : MonoBehaviour
{
    [Header("References")]
    public AudioSource audioSource;        // AudioSource en el mismo objeto que contiene el Animator
    public AudioClip soundEffect;          // Sonido a reproducir
    public GameObject objectToDeactivate;  // Objeto que se desactivará
    public Animator animator;              // Animator que se activará

    [Header("Debug")]
    public bool triggerAction = false;     // Checkbox para probar el método desde el editor
    public bool isActivated;

    void Start()
    {
        isActivated = false;
    }
    void Update()
    {
        if (PerlaNegra.barbossaIsDead == true && !isActivated)
        {
            triggerAction = true;
            ActivateAction();      // Llama al método principal
        }
    }

    public void ActivateAction()
    {
        // Reproduce el sonido si está configurado
        if (audioSource != null && soundEffect != null)
        {
            audioSource.PlayOneShot(soundEffect);
        }

        // Desactiva el objeto si está configurado
        if (objectToDeactivate != null)
        {
            objectToDeactivate.SetActive(false);
        }

        // Activa el Animator si está configurado
        if (animator != null)
        {
            animator.enabled = true;
        }
        isActivated = true;
    }
}