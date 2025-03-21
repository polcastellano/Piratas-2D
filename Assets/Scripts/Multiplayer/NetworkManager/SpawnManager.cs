using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance; // Singleton para acceder desde cualquier lugar

    public Transform[] spawnPoints; // Array de puntos de spawn en el orden deseado

    private void Awake()
    {
        // Configurar el Singleton
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    // Método para obtener un punto de spawn específico
    public Transform GetSpawnPoint(int index)
    {
        if (index >= 0 && index < spawnPoints.Length)
        {
            return spawnPoints[index];
        }

        Debug.LogWarning("Índice de punto de spawn fuera de rango.");
        return null;
    }
}