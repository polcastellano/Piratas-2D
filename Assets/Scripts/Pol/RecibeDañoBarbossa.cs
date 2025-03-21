using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecibeDañoBarbossa : MonoBehaviour
{
    public EstadosBarbossa estadosBarbossa;
    public float coolDownDaño;
    public bool puedeRecibir = true;


    private IEnumerator CooldownDaño()
    {
        puedeRecibir = false;
        // Espera por el tiempo especificado
        yield return new WaitForSeconds(coolDownDaño);
        puedeRecibir = true;
    }
    // Start is called before the first frame update
    void Start()
    {
        estadosBarbossa = gameObject.GetComponentInParent<EstadosBarbossa>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other != null && other.CompareTag("SwordJack") && puedeRecibir)
        {
            estadosBarbossa.Damage(1f);
            estadosBarbossa.recibeAtaque = true;
        }

        StartCoroutine(CooldownDaño());
    }
}
