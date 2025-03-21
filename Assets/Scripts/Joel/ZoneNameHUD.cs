using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ZoneNameHUD : MonoBehaviour
{
    public GameObject ZonePanel;
    public TextMeshProUGUI textZone;
    private CanvasGroup panelCanvasGroup;

    // Start is called before the first frame update
    void Start()
    {
        if (ZonePanel != null)
        {
            panelCanvasGroup = ZonePanel.GetComponent<CanvasGroup>();
            if (panelCanvasGroup == null)
            {
                panelCanvasGroup = ZonePanel.AddComponent<CanvasGroup>();
            }

            // Cambia la transparencia en lugar de activar/desactivar el objeto
            panelCanvasGroup.alpha = 0; // Completamente invisible
        }
        else
        {
            Debug.LogWarning("No se encontró el panel 'Ubication' en la escena.");
        }

    }
    public void mostrarZona(string nameZone)
    {
        if(ZonePanel != null) {
            panelCanvasGroup.alpha = 1; // Panel de guardado activado
            textZone.text = nameZone;
            StartCoroutine(DesactivatePanelAfterDelay());
        }
    }
    private IEnumerator DesactivatePanelAfterDelay()
    {
        yield return new WaitForSeconds(3);
        if(panelCanvasGroup != null) {
        panelCanvasGroup.alpha = 0; // Desactiva el panel de guardado  
        }
    }
}
