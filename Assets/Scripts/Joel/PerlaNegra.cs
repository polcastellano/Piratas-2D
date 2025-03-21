using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PerlaNegra : MonoBehaviour
{
    public static bool barbossaIsDead;

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && barbossaIsDead == true)
        {
            StartCoroutine(LoadNextSceneAfterDelay(3f)); // Iniciar la espera de 10 segundos
        }
    }

    private IEnumerator LoadNextSceneAfterDelay(float delay)
    {
        Debug.Log("Esperando " + delay + " segundos antes de cargar la siguiente escena...");
        yield return new WaitForSeconds(delay); // Esperar el tiempo definido
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); // Cargar la siguiente escena
    }
}
