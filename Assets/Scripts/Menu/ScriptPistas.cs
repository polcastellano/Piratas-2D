using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class Pistas : MonoBehaviour // Pistas = Misiones
{
    public TextMeshProUGUI[] pistasTextos; // Array para los textos de las misiones
    public Image[] faceImages; // Array de las imagenes de los tripulantes
    public Image[] faceImagesReclutados; // Array de las imagenes de los tripulantes reclutados
    public TextMeshProUGUI mensajeSinPistas; // Mensaje de "No hay pistas activas"
    private bool[] estadoMisiones; // Almacena el estado de cada misi�n (activa o completada)
    //private Color colorGrisClaro = new Color(0.7f, 0.7f, 0.7f); // Definir el color gris claro
    private DataManager dataManager;
    private Objects objects;
    private List<int> listRecruitedCrewMembers = new List<int>(); 
    private List<int> acceptedMissionCrewMembers = new List<int>(); 
    public int npcState;

    void Start()
    {
        if (dataManager == null)
        {
            dataManager = FindObjectOfType<DataManager>();
        }
        if (objects == null)
        {
            objects = FindObjectOfType<Objects>();
        }
        if(dataManager != null) {
            // Guardo los tripulantes reclutados para comparar ids y activar/completar pistas correspondientes
            listRecruitedCrewMembers = dataManager.playerData.collectedCrewMembers; 
            acceptedMissionCrewMembers = dataManager.playerData.activeMissionCrewMembers;
            npcState = dataManager.playerData.npcState; 
        }
        

        // Inicializa el estado de cada misi�n como no activa
        estadoMisiones = new bool[pistasTextos.Length];
        // Desactiva todas las pistas al inicio
        foreach (var pista in pistasTextos)
        {
            pista.gameObject.SetActive(false);
        }
        foreach (var face in faceImages)
        {
            face.gameObject.SetActive(false);
        }
        foreach (var imagen in faceImagesReclutados)
        {
            imagen.gameObject.SetActive(false);
        }
        ActualizarMensajeSinPistas(); // Llama a la funcion

        // Activa las pistas correspondientes a los miembros de la tripulación reclutados
        for (int i = 0; i < pistasTextos.Length; i++)
        {
            if(acceptedMissionCrewMembers.Contains(i))
            {
                ActivarPista(i);
            }
            if (listRecruitedCrewMembers.Contains(i)) // Verifica si el índice está en la lista
            {
                ActivarPista(i);
                CompletarPista(i);
            }
        }
        if(dataManager != null && dataManager.loadGame == true) {
            switch(npcState) {
                case 1:
                    ActivarPista(6);
                break;
                case 2:
                    ActivarPista(7);
                break;
                case 3:
                    ActivarPista(8);
                break;
            }
        }
        
    }

    // M�todo para activar una pista espec�fica
    public void ActivarPista(int index)
    {
        Debug.Log(index + " ");
        Debug.Log(pistasTextos[index]);
        if (index >= 0 && index < pistasTextos.Length && !estadoMisiones[index])
        {
            if (pistasTextos[index] != null)
            {
                pistasTextos[index].gameObject.SetActive(true);
            }
            if (faceImages[index] != null)
            {
                faceImages[index].gameObject.SetActive(true);
            }
            estadoMisiones[index] = true;
            ActualizarMensajeSinPistas();
        }
    }

    // M�todo para tachar una pista cuando la misi�n se complete
    public void CompletarPista(int index)
    {
        if (index >= 0 && index < pistasTextos.Length && estadoMisiones[index])
        {
            //pistasTextos[index].fontStyle = FontStyles.Strikethrough; // Tacha el texto
            //pistasTextos[index].color = colorGrisClaro; // Cambia el color a gris claro

            pistasTextos[index].gameObject.SetActive(false); // Elimina la mision de misiones activas
            faceImages[index].gameObject.SetActive(false); // Elimina la cara de misiones activas
            
            if(index <= 5) 
            { // Si es entre 0 y 5 son los tripulantes y sus objetos
                faceImagesReclutados[index].gameObject.SetActive(true); // Activa la cara del tripulante en el submenu tripulantes reclutados
                objects.EntregarObjeto(index); // Elimina el objeto del inventario
            }
            
            if(index == 7) 
            { // Si es la mision de la brujula
                 objects.EntregarObjeto(6); // Elimina la brujula del inventario
            }
           
            
            estadoMisiones[index] = false; // Marcar la misi�n como completada
            ActualizarMensajeSinPistas();
            
        }
    }
    // Funcion para mostrar o no un mensaje de que no hay pistas activas.
    private void ActualizarMensajeSinPistas()
    {
        // Verifica si hay alguna pista activa
        bool hayPistasActivas = false;
        foreach (var estado in estadoMisiones)
        {
            if (estado) hayPistasActivas = true;
        }

        // Muestra o oculta el mensaje seg�n haya o no pistas activas
        mensajeSinPistas.gameObject.SetActive(!hayPistasActivas);
    }
}
