using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsList : MonoBehaviour
{
    public List<string> missionObjectsList = new List<string>();
    public List<GameObject> loadingObjectsList = new List<GameObject>();

    private DataManager dataManager;

    void Start()
    {
        if (dataManager == null)
        {
            dataManager = FindObjectOfType<DataManager>();
        }
        if(dataManager != null) {
            missionObjectsList = dataManager.playerData.collectedItemsName; // Obtiene la lista de objetos del dataManager

            if(dataManager.loadGame == false) {
                missionObjectsList.Clear(); // Limpia la lista si no se ha cargado partida
            }
        }
    }
    //Guarda los objetos de mision en un array de objetos
   public void keepObjects(string itemName)
    {
        if (!string.IsNullOrEmpty(itemName))
        {
            missionObjectsList.Add(itemName);
            Debug.Log("ELEMENTO AÑADIDO: " + itemName); // Log solo del elemento añadido
            if (dataManager != null)
            {
                dataManager.playerData.collectedItemsName.Add(itemName);
            }
        }
    }


    public bool findObjectInList(string name)
    {
        //Debug.Log("ITEM PARA VERIFICAR: " + name);
        foreach (string missionObject in missionObjectsList)
        {
            if (missionObject == name)
            {
                Debug.Log("Objeto obtenido: " + missionObject);
                return true;
            }
            else
            {
                Debug.Log("Te equivocas de objeto!");
            }
        }
        return false;
    }
}
