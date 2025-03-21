using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelInfo : MonoBehaviour
{
    public GameObject panelMap;
    public GameObject panelMinimap;
    public GameObject panelHealthRonCoins;
    // Start is called before the first frame update
    void Start()
    {
        // Comprueba que el panel este declarado
       if (panelMap != null)
        {
            panelMap.SetActive(false);
        } 
    }

    // Update is called once per frame
    void Update()
    {
        // Verifica si la tecla "Tab" ha sido presionada para mostrar o ocultar el panel
        if (Input.GetKeyDown(KeyCode.Tab) || Input.GetKeyDown(KeyCode.JoystickButton6))
        {
            // Cambia el estado de visibilidad del panel
            if (panelMap != null && panelMinimap != null && panelHealthRonCoins != null)
            {
                // Si esta activado se desactiva y viceversa
                panelMap.SetActive(!panelMap.activeSelf);
                panelMinimap.SetActive(!panelMinimap.activeSelf);
                panelHealthRonCoins.SetActive(!panelHealthRonCoins.activeSelf);

                bool isActive = panelMap.activeSelf;  // Verificamos el estado actual una sola vez
                Time.timeScale = isActive ? 0f : 1f;
            }
        }
    }
}
