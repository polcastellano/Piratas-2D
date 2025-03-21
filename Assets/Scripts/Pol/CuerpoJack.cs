using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Build.Content;
using UnityEngine;

public class CuerpoJack : MonoBehaviour
{
    public EstadosEnemigo estadosEnemigo;
    public GameObject gm;
    public int vidas;
    private float coolDown = 3.0f;
    

    public void ColisionEnemigo(){
        vidas--;
        gm.GetComponent<GameManger>().ActualizaVidas(vidas);
        CoolDownSparrow();
    }

    public void AtaqueEnemigo(){
        vidas -= 2;
        gm.GetComponent<GameManger>().ActualizaVidas(vidas);
        CoolDownSparrow();
    }

    public void CoolDownSparrow(){
        BoxCollider2D boxCollider = gameObject.GetComponentInChildren<BoxCollider2D>();
        boxCollider.enabled = false;
        Invoke("ResetSparrow", coolDown);
    }

    public void ResetSparrow(){
        BoxCollider2D boxCollider = gameObject.GetComponentInChildren<BoxCollider2D>();
        boxCollider.enabled = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.Find("GameManager");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collider2D){
        if (collider2D.tag == "Espada" || collider2D.tag == "Enemigo"){
            if(collider2D.tag == "Espada"){
                AtaqueEnemigo();
            }
            if(collider2D.tag == "Enemigo"){
                ColisionEnemigo();
            }
        }        
    }
}
