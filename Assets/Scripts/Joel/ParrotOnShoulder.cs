using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParrotOnShoulder : MonoBehaviour
{
    public GameObject parrot;
    //public ItemsList itemsList;

    public GameObject parrotCrewMember;
    void Update()
    {
        /* foreach (string missionObject in itemsList.missionObjectsList)
        {
            if (missionObject == "Parrot")
            {
                parrot.SetActive(true);
                Debug.Log("----> LORO POSICIONADO EN EL HOMBRO");
            }            
        } */
        if(parrotCrewMember.GetComponent<CrewMember>().isRecruited == true)
        {
            parrot.SetActive(true);
        }
    }
}
