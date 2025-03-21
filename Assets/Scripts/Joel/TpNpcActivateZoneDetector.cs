using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TpNpcActivateZoneDetector : MonoBehaviour
{
    public GameObject tpGirl_1;
    public GameObject tpGirl_2;
    public GameObject tpGirl_3;

    private bool activeTpZone;

    private DataManager dataManager;
    private PlayerManager player;

    void Start()
    {
        if(dataManager == null)
        {
            dataManager = FindObjectOfType<DataManager>();
        }
        if (player == null)
        {
            player = GameObject.FindWithTag("Player").GetComponent<PlayerManager>();
        } 
        if(dataManager != null)
        {
            activeTpZone = dataManager.playerData.activeTpZone;
        }
    }

    void Update()
    {   
        if (dataManager != null && dataManager.playerData != null)
        {
            if (dataManager.playerData.activeTpZone == true)
            {
                tpGirl_1.SetActive(true);
                tpGirl_2.SetActive(true);
                tpGirl_3.SetActive(true);
            }
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            if(dataManager != null) {
                dataManager.playerData.activeTpZone = true;
                Debug.Log("-----> ESTADO ZONA TP: " + dataManager.playerData.activeTpZone);
            } 
        }
    }
}
