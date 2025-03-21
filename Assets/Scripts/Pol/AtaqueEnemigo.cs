using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtaqueEnemigo : MonoBehaviour
{
    private EstadosEnemigo estadosEnemigo;


    // Start is called before the first frame update
    void Start(){
        // Obtener el script padre EstadosEnemigo
        estadosEnemigo = GetComponentInParent<EstadosEnemigo>();
    }

    // Update is called once per frame
    void Update(){
    }

    void OnTriggerEnter2D(Collider2D other){
        if (other != null && other.CompareTag("Player")){
            estadosEnemigo.zonaAtaque = true;
        }
    }

    void OnTriggerStay2D(Collider2D other){
        if (other != null && other.CompareTag("Player")){
            estadosEnemigo.zonaAtaque = true;
        }
    }

    void OnTriggerExit2D(Collider2D other){
        if (other != null && other.CompareTag("Player")){
            estadosEnemigo.zonaAtaque = false;
        }
    }
}
