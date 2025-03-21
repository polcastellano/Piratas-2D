using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class Dialogue : MonoBehaviour
{
    public GameObject panel;
    public TextMeshProUGUI textPanel;
    public TextMeshProUGUI namePanel;
    public CrewMember crewMember;
    //public Button purchaseBtn; // Botón preconfigurado como ejemplo
    //public Button lodgingBtn; // Otro botón preconfigurado
    public GameObject buttonPanel; // Panel donde están los botones
    public GameObject buttonPrefab; // Prefab para botones dinámicos
    public float textSpeed;
    public Coroutine typingCoroutine;
    public static bool isDialogueOptionsActive; // Booleana utilizada por la maquina de estados para no dejar saltar si estas seleccionando una opcion en los dialogos

    void Start()
    {
        crewMember = FindObjectOfType<CrewMember>();

        if (panel != null)
        {
            textPanel.text = panel.GetComponentInChildren<TextMeshProUGUI>().text;
        }
        else
        {
            Debug.Log("No hay panel");
        }

        if (textPanel == null)
        {
            Debug.LogError("No se encontró el componente Text dentro de Canvas.");
        }

        // Asegúrate de desactivar los botones al inicio si no están en uso
        if(buttonPanel != null) {
            buttonPanel.SetActive(false);
        }
        
    }

    // Solo recibe una frase y la muestra, nombre vacío
    public void StartDialogue(string phrase)
    {
        isDialogueOptionsActive = true;
        ShowText(phrase, "");
        ClearButtons(); // Limpiar botones previos
    }

    // Recibe una frase y un nombre, y lo muestra
    public void StartDialogue(string phrase, string name)
    {
        isDialogueOptionsActive = true;
        //Debug.Log("StartDialogue");
        ShowText(phrase, name);
        ClearButtons(); // Limpiar botones previos
    }

    // Recibe una frase y un nombre del script de carteles y lo muestra instantaneamente
    public void StartDialogueInstant(string phrase, string name)
    {
        isDialogueOptionsActive = true;
        //Debug.Log("StartDialogueInstant");
        textPanel.text = phrase;
        namePanel.text = name;
        ClearButtons(); // Limpiar botones previos
    }

    // Añade opciones de decisión
    public void ShowOptions(string phrase, string name, string[] options, System.Action<int>[] actions)
    {
        //Debug.Log("ShowOptions BoolBotones: " + isDialogueOptionsActive);
        if (options.Length != actions.Length)
        {
            Debug.LogError("Las opciones y las acciones no tienen el mismo tamaño.");
            return;
        }

        StartDialogue(phrase, name);
        isDialogueOptionsActive = true;
        buttonPanel.SetActive(true);

        GameObject firstButton = null; // Referencia al primer botón

        for (int i = 0; i < options.Length; i++)
        {
            int index = i;
            GameObject button = Instantiate(buttonPrefab, buttonPanel.transform);
            button.GetComponentInChildren<TMP_Text>().text = options[i];
            button.GetComponent<Button>().onClick.AddListener(() =>
            {
                actions[index]?.Invoke(index);
                //ClearButtons();
            });

            // Guardar referencia al primer botón creado
            if (i == 0)
            {
                firstButton = button;
            }
        }

        // Seleccionar el primer botón por defecto
        if (firstButton != null)
        {
            EventSystem.current.SetSelectedGameObject(firstButton);
        }
    }

    // Muestra el texto progresivamente en el panel
    IEnumerator TypeLine(string phrase)
    {
        foreach (char c in phrase.ToCharArray())
        {
            textPanel.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
        typingCoroutine = null; // Reset coroutine reference when done
    }

    // Limpia los botones del panel
    public void ClearButtons()
    {
        foreach (Transform child in buttonPanel.transform)
        {
            Destroy(child.gameObject);
        }
        buttonPanel.SetActive(false);
        StartCoroutine(DesactivateDialogueOptionsDelay());
        //Debug.Log("ClearButtons BoolBotones: " + isDialogueOptionsActive);
    }

    private IEnumerator DesactivateDialogueOptionsDelay()
    {
        yield return new WaitForSeconds(2);
        isDialogueOptionsActive = false;
    }

    private void ShowText(string phrase, string name)
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }

        textPanel.text = "";
        namePanel.text = name;
        typingCoroutine = StartCoroutine(TypeLine(phrase));
    }
}
