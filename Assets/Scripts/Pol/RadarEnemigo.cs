using System.Collections;
using System.Collections.Generic;
// using System.Numerics;
using UnityEngine;

public class RadarEnemigo : MonoBehaviour
{
    private EstadosEnemigo estadosEnemigo;

  /*void Rayos(bool derecha){
    // Determinar la posición de origen y dirección
    Vector2 posicionOrigen = new Vector2(transform.position.x, transform.position.y);
    Vector2 direccion = derecha ? Vector2.right : Vector2.left;

    // Ángulo de apertura del cono (en grados)
    float anguloApertura = 90.0f;
    float distanciaRayo = 4.0f;

    // Calcular los dos ángulos para los rayos del cono
    Vector2 direccionCentral = direccion.normalized;

    // Crear las direcciones de los rayos
    Vector2 direccionArriba = Quaternion.Euler(0, 0, -anguloApertura / 2) * direccionCentral;
    Vector2 direccionAbajo = Quaternion.Euler(0, 0, anguloApertura / 2) * direccionCentral;

    // Lanzar los rayos
    RaycastHit2D hitArriba = Physics2D.Raycast(posicionOrigen, direccionArriba, distanciaRayo);
    RaycastHit2D hitCentral = Physics2D.Raycast(posicionOrigen, direccionCentral, distanciaRayo);
    RaycastHit2D hitAbajo = Physics2D.Raycast(posicionOrigen, direccionAbajo, distanciaRayo);
    
    // Dibujar los rayos
    Debug.DrawRay(posicionOrigen, direccionArriba * distanciaRayo, Color.red);
    Debug.DrawRay(posicionOrigen, direccionCentral * distanciaRayo, Color.red);
    Debug.DrawRay(posicionOrigen, direccionAbajo * distanciaRayo, Color.red);

    // Comprobar si alguno de los rayos detecta a Sparrow
    if (hitArriba.collider != null || hitAbajo.collider != null || hitCentral.collider != null){
        // Obtener el objeto padre del objeto golpeado
        Transform padre = hitArriba.collider.transform.parent;

        // Verificar si el objeto padre no es nulo y comparar su tag
        if (padre != null && padre.CompareTag("Player"))
        {
            Debug.Log("Sparrow detectado por la izquierda");
            Perseguir(); // Llama a la función de persecución
        }
    }
}*/


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
            // Iniciar la persecución hacia el jugador
            estadosEnemigo.objetivo = other.GetComponent<Transform>();
            estadosEnemigo.zonaDeteccion = true;
            estadosEnemigo.patrullando = false;
            estadosEnemigo.barraVida.gameObject.SetActive(true);
        }
    }

    void OnTriggerStay2D(Collider2D other){
        if (other != null && other.CompareTag("Player")){
            // Iniciar la persecución hacia el jugador
            estadosEnemigo.objetivo = other.GetComponent<Transform>();
            estadosEnemigo.zonaDeteccion = true;
            estadosEnemigo.patrullando = false;
            estadosEnemigo.barraVida.gameObject.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other){
        if (other != null && other.CompareTag("Player")){
            // Iniciar la persecución hacia el jugador
            estadosEnemigo.objetivo = null;
            estadosEnemigo.zonaDeteccion = false;
            estadosEnemigo.barraVida.gameObject.SetActive(false);
        }
    }
}
