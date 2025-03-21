using System.Collections;
using UnityEngine;

public class PrefabSpawnerBox : MonoBehaviour
{
    [Header("Configuración del Spawner")]
    public GameObject prefabToSpawn; // Prefab a instanciar
    public Transform spawnPoint; // Punto de spawn (solo usa su Y y Z)
    public Transform xMinPoint; // Límite izquierdo en X
    public Transform xMaxPoint; // Límite derecho en X
    public float spawnInterval = 1f; // Intervalo de spawn en segundos

    private void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        while (true)
        {
            SpawnPrefab();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void SpawnPrefab()
    {
        if (prefabToSpawn == null || spawnPoint == null || xMinPoint == null || xMaxPoint == null)
        {
            Debug.LogWarning("⚠️ Falta asignar referencias en el Spawner.");
            return;
        }

        // Generar una posición aleatoria dentro del rango de X
        float randomX = Random.Range(xMinPoint.position.x, xMaxPoint.position.x);
        Vector3 spawnPosition = new Vector3(randomX, spawnPoint.position.y, spawnPoint.position.z);

        // Instanciar el prefab en la posición generada
        Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity);
    }
}
