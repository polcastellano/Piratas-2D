using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EspadaBarbossa : MonoBehaviour
{
    public Health health;
    public float daño;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other){
        if (other != null && other.CompareTag("Player")){
            health.Damage(daño);
        }
    }
}
