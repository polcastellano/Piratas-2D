using UnityEngine;
using UnityEngine.Video;

public class VideoLoader : MonoBehaviour
{
    public string videoName = "Creditos_Pirata2D"; // Nombre del video sin extensión

    private void Start()
    {
        // Obtiene el componente VideoPlayer del objeto
        VideoPlayer videoPlayer = GetComponent<VideoPlayer>();

        if (videoPlayer != null)
        {
            // Carga el video desde la carpeta Resources/Videos
            VideoClip videoClip = Resources.Load<VideoClip>($"Videos/{videoName}");
            if (videoClip != null)
            {
                // Asigna el video al VideoPlayer
                videoPlayer.clip = videoClip;
                videoPlayer.Play();
            }
            else
            {
                Debug.LogError("Video no encontrado en la carpeta Resources/Videos.");
            }
        }
        else
        {
            Debug.LogError("No se encontró el componente VideoPlayer en el objeto.");
        }
    }
}
