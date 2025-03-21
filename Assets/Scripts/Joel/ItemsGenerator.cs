using System.Linq;
using UnityEngine;


public class ItemsGenerator : MonoBehaviour
{
    public GameObject[] coins; // Array donde añadir cada moneda
    public int QuantityCoinsDespawn; // Numero de monedas que queremos desactivar

    public GameObject[] rums; // Array donde añadir cada ron
    public int QuantityRumsDespawn; // Numero de rons que queremos desactivar
    void Start()
    {
        // Obtén el numero de monedas que queremos activar y indexa de forma forma aleatoria la cantidad especificada en QuatityCoinsDespawn y la guarda en randomIndex
        int[] randomCoinsIndex = Enumerable.Range(0, coins.Length)
                                            .OrderBy(x => Random.value)
                                            .Take(QuantityCoinsDespawn)
                                            .ToArray();

        // Desactiva las monedas indexadas y por lo tanto deja activadas las que no se han seleccionado
        foreach (int i in randomCoinsIndex)
        {
            coins[i].SetActive(false); // Desactiva las monedas indexadas y deja activas las otras
        }


        // Obtiene todas las botellas de ron y las indexa aleatoriamente en un array
        int[] randomRumsIndex = Enumerable.Range(0, rums.Length)
                                            .OrderBy(x => Random.value)
                                            .Take(QuantityRumsDespawn)
                                            .ToArray();

        // Desactiva el numero de QuantityRumsDespawn del array para que se desactiven
        foreach (int i in randomRumsIndex)
        {
            rums[i].SetActive(false); 
        }
    }
}
