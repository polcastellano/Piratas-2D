using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class VideoSceneController : MonoBehaviour
{
    [Header("Scene Settings")]
    public string nextSceneName = "MenuPrincipal"; // Nombre de la escena a cargar después del video
    public string videoName = "nombre_del_video"; // Nombre del video sin extensión

    private VideoPlayer videoPlayer;

    private void Start()
    {
        // Obtener y configurar el VideoPlayer
        videoPlayer = GetComponent<VideoPlayer>();
        if (videoPlayer == null)
        {
            Debug.LogError("No se encontró el componente VideoPlayer en este GameObject.");
            return;
        }

        // Cargar el VideoClip desde Resources
        VideoClip videoClip = Resources.Load<VideoClip>($"Videos/{videoName}");
        if (videoClip != null)
        {
            videoPlayer.clip = videoClip;
        }
        else
        {
            Debug.LogError($"No se encontró el video '{videoName}' en Resources/Videos.");
            return;
        }

        // Suscribirse al evento que se activa al terminar el video
        videoPlayer.loopPointReached += OnVideoEnd;

        // Reproducir el video
        videoPlayer.Play();
    }
    private void Update()
    {
        // Cancelar creditos y volver al menu principal
        if(Input.GetKey(KeyCode.Escape) || Input.GetButton("Cancel") || Input.GetKey(KeyCode.JoystickButton7) || Input.GetButtonDown("Fire2"))
        {
            SceneManager.LoadScene(0);
        }
    }
    public void volverAlMenu()
    {
        SceneManager.LoadScene(0);
    }
    private void OnVideoEnd(VideoPlayer vp)
    {
        // Cargar la escena cuando termina el video
        SceneManager.LoadScene(nextSceneName);
    }

    private void OnDisable()
    {
        // Desuscribirse del evento para evitar posibles errores
        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached -= OnVideoEnd;
        }
    }

}
