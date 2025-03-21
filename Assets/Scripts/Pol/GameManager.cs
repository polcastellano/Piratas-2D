using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManger : MonoBehaviour
{
    public GameObject player;
    public TextMeshProUGUI textoVidas;

    public void ActualizaVidas(int vidas){
        textoVidas.text = "Vidas: " + vidas;
    }    

    /*public void MatarEnemigo(GameObject enemigo){
        enemigo.GetComponentInChildren<PolygonCollider2D>().enabled = false;
    }*/

    // Start is called before the first frame update
    void Start(){
        player = GameObject.Find("Sparrow");
        textoVidas.text = "Vidas: " + player.GetComponentInChildren<CuerpoJack>().vidas;
    }

    // Update is called once per frame
    void Update(){
        
    }
}
