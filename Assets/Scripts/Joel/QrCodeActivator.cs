using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QrCodeActivator : MonoBehaviour
{
    public bool activateInteract;
    public GameObject interactiveBtn;
    public GameObject qr;
    void Start()
    {
        
    }

    void Update()
    {
        if (activateInteract && (Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.JoystickButton3)))
        {
            qr.SetActive(true);
        }
    }

     public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            interactiveBtn.SetActive(true);
            activateInteract = true;
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            interactiveBtn.SetActive(false);
            activateInteract = false;
            qr.SetActive(false);
        }
    }
}
