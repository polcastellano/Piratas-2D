using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToogleByPositionY : MonoBehaviour
{
   public float thresholdY = -4f; // Umbral de posición en Y
    private bool isBelowThreshold = false; // Estado actual respecto al umbral

    private void FixedUpdate()
    {
        // Verifica la posición en Y
        if (transform.position.y <= thresholdY && !isBelowThreshold)
        {
            isBelowThreshold = true;
            // Desactiva a todos los hijos
            SetChildrenActive(false);
        }
        else if (transform.position.y > thresholdY && isBelowThreshold)
        {
            isBelowThreshold = false;
            // Reactiva a todos los hijos
            SetChildrenActive(true);
        }
    }

    private void SetChildrenActive(bool state)
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(state);
        }
    }
}
