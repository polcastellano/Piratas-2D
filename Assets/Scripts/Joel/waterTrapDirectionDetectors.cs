using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class waterTrapDirectionDetectors : MonoBehaviour
{
    public static bool isLeft;
    public GameObject leftWaterTrap;
    public GameObject rightWaterTrap;

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            if(gameObject.name == "Left")
            {
                isLeft = true;
                leftWaterTrap.SetActive(true);
                rightWaterTrap.SetActive(false);
            }
            else if(gameObject.name == "Right")
            {
                isLeft = false;
                rightWaterTrap.SetActive(true);
                leftWaterTrap.SetActive(false);
            }
        }
    }
}
