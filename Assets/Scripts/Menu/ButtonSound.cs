using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonSound : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, ISelectHandler, ISubmitHandler
{
    public int hoverIndex; // Índice del sonido al pasar el cursor
    public int clickIndex; // Índice del sonido al hacer clic
    private AudioManager audioManager; // Cambié a private

    // Asignar la referencia de AudioManager en Start
    public void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
        if (audioManager == null)
        {
            Debug.LogWarning("No se encontró un AudioManager en la escena.");
        }
    }

    // Sonido al pasar el cursor
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (audioManager != null)
        {
            audioManager.PlayUI(hoverIndex);
        }
    }

    // Sonido al hacer clic
    public void OnPointerClick(PointerEventData eventData)
    {
        if (audioManager != null)
        {
            audioManager.PlayUI(clickIndex);
        }
    }

    // Sonido al recibir selección (mando o teclado)
    public void OnSelect(BaseEventData eventData)
    {
        if (audioManager != null)
        {
            audioManager.PlayUI(hoverIndex);
        }
    }

    // Sonido al confirmar selección (mando o teclado)
    public void OnSubmit(BaseEventData eventData)
    {
        if (audioManager != null)
        {
            audioManager.PlayUI(clickIndex);
        }
    }
}
