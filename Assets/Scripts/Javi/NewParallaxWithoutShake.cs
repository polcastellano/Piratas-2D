using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewParallaxWithoutShake : MonoBehaviour
{
   public Transform cameraTransform;                // Transform de la cámara
    public List<GameObject> backgrounds;             // Lista de imágenes de fondo en el orden en que aparecen
    public float backgroundWidth;                    // Ancho de cada imagen de fondo en unidades
    public float triggerDistance = 0.8f;             // Porcentaje de distancia (ej. 0.8 = 80%) antes de activar la siguiente imagen

    private int currentIndex = 0;                    // Índice de la primera imagen activa
    private int nextIndex = 1;                       // Índice de la segunda imagen activa

    void Update()
    {
        float cameraPositionX = cameraTransform.position.x;
        float currentBackgroundEnd = backgrounds[currentIndex].transform.position.x + backgroundWidth * triggerDistance;

        // Avanzar hacia la derecha
        if (cameraPositionX > currentBackgroundEnd)
        {
            // Activa la siguiente imagen y actualiza los índices
            backgrounds[nextIndex].transform.position = backgrounds[currentIndex].transform.position + new Vector3(backgroundWidth, 0, 0);
            backgrounds[nextIndex].SetActive(true);
            backgrounds[currentIndex].SetActive(false);

            // Actualizar los índices para que se muevan a la derecha
            currentIndex = nextIndex;
            nextIndex = (nextIndex + 1) % backgrounds.Count;
        }

        // Retroceder hacia la izquierda
        float previousBackgroundStart = backgrounds[currentIndex].transform.position.x - backgroundWidth * triggerDistance;

        if (cameraPositionX < previousBackgroundStart)
        {
            // Activa la imagen previa y actualiza los índices
            int previousIndex = (currentIndex - 1 + backgrounds.Count) % backgrounds.Count;
            backgrounds[previousIndex].transform.position = backgrounds[currentIndex].transform.position - new Vector3(backgroundWidth, 0, 0);
            backgrounds[previousIndex].SetActive(true);
            backgrounds[nextIndex].SetActive(false);

            // Actualizar los índices para que se muevan a la izquierda
            nextIndex = currentIndex;
            currentIndex = previousIndex;
        }
    }
}



