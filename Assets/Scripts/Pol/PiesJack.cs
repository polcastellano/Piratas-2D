using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiesJack : MonoBehaviour
{
    private EstadosEnemigo estadosEnemigo;
    private Rigidbody2D rb;
    public float salto = 8.0f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponentInParent<Rigidbody2D>();
        
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collider2D){
        if (collider2D.CompareTag("Enemigo")) // Utiliza CompareTag en lugar de comparar cadenas
        {
            // Busca el componente EstadosEnemigo en el GameObject del enemigo
            estadosEnemigo = collider2D.GetComponent<EstadosEnemigo>();
            
            if (estadosEnemigo != null) // Verifica que se haya encontrado el componente
            {
                rb.velocity = new Vector2(rb.velocity.x, salto);
                estadosEnemigo.zonaAturdido = true;
            }
        }
    }
}
