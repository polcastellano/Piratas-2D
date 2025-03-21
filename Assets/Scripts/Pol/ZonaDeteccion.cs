using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ZonaDeteccion : MonoBehaviour
{
    
    public GameObject bossHealth;
    public Image barraBoss;
    public static bool detectado = false;

    
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            detectado = true;
            bossHealth.SetActive(true);
        }
    }
}
