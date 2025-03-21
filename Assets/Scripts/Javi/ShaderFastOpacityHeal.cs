using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaderFastOpacityHeal : MonoBehaviour
{
        public Material targetMaterial; // Material con el shader
    public string parameterName = "_FinalOpacityPower"; // Nombre del parámetro en el shader
    public float duration = 1.0f; // Duración total del efecto

    private void OnEnable()
    {
        if (targetMaterial == null)
        {
            Debug.LogError("El material no está asignado.");
            return;
        }

        if (!targetMaterial.HasProperty(parameterName))
        {
            Debug.LogError($"El parámetro '{parameterName}' no existe en el shader del material asignado.");
            return;
        }

        // Inicia la animación del parámetro
        StartCoroutine(AnimateOpacity());
    }

    private System.Collections.IEnumerator AnimateOpacity()
    {
        float elapsedTime = 0f;
        float halfDuration = duration / 2f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;

            if (elapsedTime <= halfDuration)
            {
                // Incrementa la opacidad en la primera mitad del tiempo
                float progress = elapsedTime / halfDuration;
                float value = Mathf.Lerp(0, 1, progress);
                targetMaterial.SetFloat(parameterName, value);
            }
            else
            {
                // Decrementa la opacidad en la segunda mitad del tiempo
                float progress = (elapsedTime - halfDuration) / halfDuration;
                float value = Mathf.Lerp(1, 0, progress);
                targetMaterial.SetFloat(parameterName, value);
            }

            yield return null;
        }

        // Asegúrate de que el parámetro termine en 0
        targetMaterial.SetFloat(parameterName, 0);
    }
}
