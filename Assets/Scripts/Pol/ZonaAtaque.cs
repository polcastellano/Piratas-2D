using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZonaAtaque : MonoBehaviour
{

    public static bool rangoAtaque = false;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("RANGO ATAQUE "  +rangoAtaque);
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            rangoAtaque = true;
        }
    }

    public void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            rangoAtaque = true;
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            rangoAtaque = false;
        }
    }
}
