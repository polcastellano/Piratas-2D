using UnityEngine;
using TMPro;

public class Coins : MonoBehaviour
{
    public TextMeshProUGUI textCoins; // Texto de las monedas
    public TextMeshProUGUI textCoinsMenu; // Texto de las monedas del panel info
    public TextMeshProUGUI textCoinsDicesPanel;
    public int actualCoins = 0; // Cantidad de monedas actuales

    void Start()
    {
        textCoins.text = actualCoins + ""; // Monedas iniciales
        if(textCoinsMenu != null) {
            textCoinsMenu.text = actualCoins + ""; 
            textCoinsDicesPanel.text = actualCoins + "";
        }
    }

    // M�todo para restar monedas
    public void spendCoins(int coins)
    {
        actualCoins = Mathf.Max(0, actualCoins - coins); // Evita valores negativos y resta las monedas actuales menos las monedas gastas
        textCoins.text = actualCoins + ""; // Modifica el texto con la cantidad de monedas actuales
        if(textCoinsMenu != null) {
            textCoinsMenu.text = actualCoins + "";
            textCoinsDicesPanel.text = actualCoins + "";
        }
    }

    // Metodo para sumar monedas
    public void obtainCoins(int coins)
    {
        actualCoins = actualCoins + coins; // Monedas actuales + monedas obtenidas
        textCoins.text = actualCoins + ""; // Modifica el texto con la cantidad de monedas actuales
        if(textCoinsMenu != null) {
            textCoinsMenu.text = actualCoins + "";
            textCoinsDicesPanel.text = actualCoins + "";
        }
    }
}
