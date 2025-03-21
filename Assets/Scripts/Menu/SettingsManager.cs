using System.IO;
using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    private string filePath;

    // Instancia de configuración que usaremos en el juego
    public GameSettings gameSettings = new GameSettings();
    public static SettingsManager instance;

    private void Awake()
    {
        // Verifica si ya existe una instancia de SettingsManager
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);  // No destruir este objeto entre escenas
        }
        else
        {
            Destroy(gameObject);  // Si ya existe una instancia, destruye el nuevo objeto
        }

        // Define la ruta donde se almacenará el archivo JSON
        filePath = Path.Combine(Application.persistentDataPath, "gamesettings.json");

        // Cargar las configuraciones al iniciar el juego
        LoadSettings();
    }

    // Guarda las configuraciones actuales en un JSON
    public void SaveSettings()
    {
        // Convierte los datos a formato JSON
        string jsonData = JsonUtility.ToJson(gameSettings, true);

        // Escribe el JSON en un archivo
        File.WriteAllText(filePath, jsonData);

        Debug.Log("Configuraciones guardadas en " + filePath);
    }
    // Guarda configuraciones específicas (usado en ResetToDefault de menuOpciones)
    public void SaveSettings(GameSettings settings)
    {
        // Convierte los datos a formato JSON
        string jsonData = JsonUtility.ToJson(settings, true);

        // Escribe el JSON en un archivo
        File.WriteAllText(filePath, jsonData);

        Debug.Log("Configuraciones predeterminadas guardadas en " + filePath);
    }

    // Método para cargar los ajustes desde un archivo JSON
    public void LoadSettings()
    {
        if (File.Exists(filePath))
        {
            // Lee el archivo JSON
            string jsonData = File.ReadAllText(filePath);

            // Convierte el JSON a una instancia de GameSettings
            gameSettings = JsonUtility.FromJson<GameSettings>(jsonData);

            Debug.Log("Configuraciones cargadas desde " + filePath);
        }
        else
        {
            Debug.Log("No se encontró archivo de configuraciones. Cargando valores por defecto.");
        }
    }

    // Método para restablecer los ajustes a valores predeterminados
    public void ResetToDefault()
    {
        gameSettings = new GameSettings();
        SaveSettings();
        Debug.Log("Configuraciones restablecidas a los valores predeterminados.");
    }
}
