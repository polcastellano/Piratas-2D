using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HostelParticleSleepingEffect : MonoBehaviour
{
    public ParticleSystem[] hostelSleepingEffects;
    public Day_Night day_Night;
    private bool wasDay = false;

    void Start()
    {
        /* if(day_Night == null)
        {
            day_Night = GetComponent<Day_Night>();
            Debug.Log("----> DIAAAAA: " + day_Night.isDay);
        } */
        //Debug.Log("----> NOMBRE: " + hostelSleepingEffects[0].name);
        //Debug.Log("----> NOMBRE: " + hostelSleepingEffects[1].name);
    }

    void Update()
    {
        // Solo actuamos cuando el estado cambia de día a noche o viceversa
        if (day_Night.isDay != wasDay)
        {
            wasDay = day_Night.isDay;

            foreach (ParticleSystem particle in hostelSleepingEffects)
            {
                if (day_Night.isDay)
                {
                    particle.Stop();
                }
                else
                {
                    particle.Play();
                }
            }
        }
    }
}
