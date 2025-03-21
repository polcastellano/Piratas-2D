using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PanelNavigation : MonoBehaviour
{
    public Button defaultButton;    // Boton que por defecto queremos que sea el seleccionado
    public GameObject backPanel;    // Panel al que volver al presionar Escape

    private void OnEnable()
    {
        // Selecciona el bot�n predeterminado cuando el panel se activa
        if(defaultButton != null) {
            EventSystem.current.SetSelectedGameObject(defaultButton.gameObject);
        }
        
    }

    private void Update()
    {
        // Asegura que el bot�n siga seleccionado
        if (EventSystem.current.currentSelectedGameObject == null && defaultButton != null)
        {
            EventSystem.current.SetSelectedGameObject(defaultButton.gameObject);
        }

        // Volver atr�s con Escape o bot�n B en mando
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetButtonDown("Cancel"))
        {
            if (backPanel != null)
            {
                backPanel.SetActive(true);     // Activa el panel de regreso
                gameObject.SetActive(false);   // Desactiva el panel actual
            }
        }
    }
}
