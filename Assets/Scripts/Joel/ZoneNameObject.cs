using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneNameObject : MonoBehaviour
{
    public string zoneName; // Nombre de la zona que se muestra en el HUD
    private ZoneNameHUD zonePanel; // Panel que informa de la ubicacion actual
    // Start is called before the first frame update
    void Start()
    {
        if(zonePanel == null) // Encuentra el DataManager de forma dinamica si no esta declarado
        {
            zonePanel = FindObjectOfType<ZoneNameHUD>();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            zonePanel.mostrarZona(zoneName);
        }
    }
}
