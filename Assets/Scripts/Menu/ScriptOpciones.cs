using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;

public class ScriptOpciones : MonoBehaviour
{
    public Image BrightnessPanel; // Panel de brillo del juego
    public Slider BrightnessSlider; // Slider de Brillo
    public Slider audioGlobalSlider; // Slider del volumen global
    public Slider audioMusicSlider; // Slider de la musica
    public Slider audioSfxSlider; // Slider del volumen de efectos
    public Slider audioDialogueSlider; // Slider del volumen de dialogos
    public AudioMixer audioMixer;
    public Toggle fullScreenToggle; // Booleana casilla de pantalla completa
    public TMP_Dropdown QualityDropdown; // Desplegable de la calidad del juego
    public TMP_Dropdown ResolutionDropdown; // Desplegable de las resoluciones
    public TMP_Text textoInfo; // Texto informativo

    private Resolution[] resolutions;
    private List<Resolution> filteredResolutions = new List<Resolution>(); // Lista para almacenar resoluciones únicas
    private SettingsManager settingsManager; // Referencia al SettingsManager

    // Funcion start, ejecutada al iniciar el script
    private void Start()
    {

        // Obtener la instancia de SettingsManager
        settingsManager = FindObjectOfType<SettingsManager>();

        // Cargar configuraciones guardadas
        LoadSettingsFromJSON();

        textoInfo.text = "";
    }

    // Función para cargar configuraciones desde JSON
    private void LoadSettingsFromJSON()
    {
        // Establecer la calidad
        QualityDropdown.value = settingsManager.gameSettings.qualityIndex;
        setQuality();

        // Configurar pantalla completa
        fullScreenToggle.isOn = settingsManager.gameSettings.fullScreen;
        setFullscreen(fullScreenToggle.isOn);

        // Configurar brillo
        BrightnessSlider.value = settingsManager.gameSettings.brightness;
        setBrightness(BrightnessSlider.value);

        // Configurar resolución
        checkResolution();
        ResolutionDropdown.value = settingsManager.gameSettings.resolutionIndex;
        setResolution(ResolutionDropdown.value);

        // Configurar volumen global
        audioGlobalSlider.value = settingsManager.gameSettings.globalVolume;
        setAudioGlobal(audioGlobalSlider.value);

        // Configurar volumen musica
        audioMusicSlider.value = settingsManager.gameSettings.musicVolume;
        setAudioMusic(audioMusicSlider.value);

        // Configurar volumen SFX
        audioSfxSlider.value = settingsManager.gameSettings.sfxVolume;
        setAudioSFX(audioSfxSlider.value);

        // Configurar volumen dialogos
        audioDialogueSlider.value = settingsManager.gameSettings.dialogueVolume;
        setAudioSFX(audioDialogueSlider.value);
    }

    // Función para cambiar la calidad de imagen del juego entre muy bajo y ultra
    public void setQuality()
    {
        QualitySettings.SetQualityLevel(QualityDropdown.value);
        settingsManager.gameSettings.qualityIndex = QualityDropdown.value;
        
    }

    // Función para cambiar entre pantalla completa y modo ventana
    public void setFullscreen(bool fullscreen)
    {
        Screen.fullScreen = fullscreen;
        settingsManager.gameSettings.fullScreen = fullscreen;
        
    }

    // Función para ajustar el brillo del panel BrightnessPanel
    public void setBrightness(float valor)
    {
        BrightnessPanel.color = new Color(BrightnessPanel.color.r, BrightnessPanel.color.g, BrightnessPanel.color.b, valor);
        settingsManager.gameSettings.brightness = valor;
        
    }

    public void setAudioGlobal(float volume)
    {
        settingsManager.gameSettings.globalVolume = volume;
        // Si el valor del slider es 0, asignamos -80dB, de lo contrario ajustamos el volumen
        float volumeInDecibels;

        if (volume == 0)
        {
            volumeInDecibels = -80f; // El volumen mínimo (silencio)
        }
        else
        {
            // Para valores entre 0.01 y 1, escala de -20dB a 0dB
            volumeInDecibels = Mathf.Lerp(-20f, 0f, Mathf.InverseLerp(0.01f, 1f, volume));
        }
        audioMixer.SetFloat("MasterVolume", volumeInDecibels); // Ajusta el volumen global
    }

    public void setAudioMusic(float volume)
    {
        settingsManager.gameSettings.musicVolume = volume;
        // Si el valor del slider es 0, asignamos -80dB, de lo contrario ajustamos el volumen
        float volumeInDecibels;

        if (volume == 0)
        {
            volumeInDecibels = -80f; // El volumen mínimo (silencio)
        }
        else
        {
            // Para valores entre 0.01 y 1, escala de -20dB a 0dB
            volumeInDecibels = Mathf.Lerp(-20f, 0f, Mathf.InverseLerp(0.01f, 1f, volume));
        }
        audioMixer.SetFloat("MusicVolume", volumeInDecibels); // Ajusta el volumen de musica

        // Debugging: Verifica que el valor está siendo cambiado
        //Debug.Log("Music Volume: " + volumeInDecibels);
    }

    public void setAudioSFX(float volume)
    {
        settingsManager.gameSettings.sfxVolume = volume;
        // Si el valor del slider es 0, asignamos -80dB, de lo contrario ajustamos el volumen
        float volumeInDecibels;

        if (volume == 0)
        {
            volumeInDecibels = -80f; // El volumen mínimo (silencio)
        }
        else
        {
            // Para valores entre 0.01 y 1, escala de -20dB a 0dB
            volumeInDecibels = Mathf.Lerp(-20f, 0f, Mathf.InverseLerp(0.01f, 1f, volume));
        }
        audioMixer.SetFloat("SFXVolume",volumeInDecibels); // Ajusta el volumen de efectos
    }

    public void setAudioDialogue(float volume)
    {
        settingsManager.gameSettings.dialogueVolume = volume;
        // Si el valor del slider es 0, asignamos -80dB, de lo contrario ajustamos el volumen
        float volumeInDecibels;

        if (volume == 0)
        {
            volumeInDecibels = -80f; // El volumen mínimo (silencio)
        }
        else
        {
            // Para valores entre 0.01 y 1, escala de -20dB a 0dB
            volumeInDecibels = Mathf.Lerp(-20f, 0f, Mathf.InverseLerp(0.01f, 1f, volume));
        }
        audioMixer.SetFloat("DialogueVolume", volumeInDecibels); // Ajusta el volumen de diálogos
    }

    // Comprobación de resoluciones disponibles
    public void checkResolution()
    {
        resolutions = Screen.resolutions;
        ResolutionDropdown.ClearOptions(); // Limpiar las opciones existentes
        List<string> options = new List<string>();
        int actualResolution = 0;

        HashSet<string> uniqueResolutions = new HashSet<string>(); // Usamos HashSet para evitar duplicados
        filteredResolutions.Clear(); // Limpiamos la lista de resoluciones filtradas

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height + "px";

            // Solo agregamos la resolución si no está en el HashSet (para evitar duplicados)
            if (uniqueResolutions.Add(option))
            {
                options.Add(option);
                filteredResolutions.Add(resolutions[i]); // También añadimos la resolución única a la lista filtrada
            }

            // Comprobamos si esta es la resolución actual
            if (Screen.fullScreen && resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                actualResolution = filteredResolutions.Count - 1; // Alineamos el índice con la lista filtrada
            }
        }

        ResolutionDropdown.AddOptions(options); // Añadimos las opciones únicas
        ResolutionDropdown.value = actualResolution; // Seleccionamos la resolución actual
        ResolutionDropdown.RefreshShownValue(); // Refrescamos el Dropdown para mostrar los cambios
    }

    // Función para ajustar la resolución
    public void setResolution(int indexResolution)
    {
        if (indexResolution >= 0 && indexResolution < filteredResolutions.Count)
        {
            Resolution resolution = filteredResolutions[indexResolution];
            Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
            settingsManager.gameSettings.resolutionIndex = ResolutionDropdown.value;
            settingsManager.SaveSettings(); // Guardar ajustes
        }
        else
        {
            Debug.LogError("Índice de resolución fuera de rango: " + indexResolution);
        }
    }
    // Funcion para reiniciar los valores por defecto
    public void ResetToDefault()
    {
        GameSettings defaultSettings = new GameSettings
        {
            brightness = 0.0f,  // Valores predeterminados
            qualityIndex = 2,  // Media calidad
            resolutionIndex = 0,  // Resolución más baja
            fullScreen = false,  // Pantalla completa activada
            globalVolume = 1.0f, // Volumen por defecto global
            musicVolume = 1.0f,  // Volumen de música por defecto
            sfxVolume = 1.0f, // Volumen de efectos por defecto
            dialogueVolume = 1.0f // Volumen por defecto de los dialogos
        };

        // Aplica los valores predeterminados
        ApplySettings(defaultSettings);

        // Guarda los valores predeterminados
        settingsManager.SaveSettings(defaultSettings);
        textoInfo.text = "Configuración reiniciada por defecto...";
        StartCoroutine(VaciarTexto());
    }
    public void ApplySettings(GameSettings settings)
    {
        // Aplicar las configuraciones a los distintos elementos
        BrightnessSlider.value = settings.brightness;
        setBrightness(settings.brightness);

        QualityDropdown.value = settings.qualityIndex;
        setQuality();

        ResolutionDropdown.value = settings.resolutionIndex;
        setResolution(settings.resolutionIndex);

        fullScreenToggle.isOn = settings.fullScreen;
        setFullscreen(settings.fullScreen);

        audioGlobalSlider.value = settings.globalVolume;
        setAudioMusic(settings.globalVolume);

        audioMusicSlider.value = settings.musicVolume;
        setAudioMusic(settings.musicVolume);

        audioSfxSlider.value = settings.sfxVolume;
        setAudioMusic(settings.sfxVolume);

        audioDialogueSlider.value = settings.dialogueVolume;
        setAudioMusic(settings.dialogueVolume);
    }

    public void SaveSettings() {
        settingsManager.SaveSettings(); // Llama a la funcion para guardar los ajustes
        textoInfo.text = "Se ha guardado la configuración...";
        StartCoroutine(VaciarTexto());
    }
     IEnumerator VaciarTexto()
    {
        // Espera 3 segundos
        yield return new WaitForSeconds(3);

        // Cambia el texto después de esperar
        textoInfo.text = "";
    }
}
