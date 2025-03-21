using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ron : MonoBehaviour
{
    public Image RonBar; // Imagen de la barra de ron (relleno)
    public Image RonBarMenu; // Imagen de la barra de ron del menu de info
    public float maximumRon = 100f; // Ron maximo
    public float actualRon; // Ron actual
    public float tasaDisminucionRon = 0.1f; // Cantidad de ron perdida por segundo
    public float fillTime = 1f; // Tiempo que tarda en llenarse la barra despues de beber una botella

    public float maxRonDuration = 15f; // Tiempo que se mantiene la barra al maximo al pasar del limite
    private bool isMaxRonActive = false; // Controla si la barra est� en su m�ximo

    Color ronColor = new Color(0.8f, 0.5f, 0.1f); // Dorado oscuro
    Color alertColor = new Color(0.6f, 0.2f, 0.1f);  // Marr�n rojizo
    Color drunkColor = new Color(0.4f, 0.2f, 0.1f); // Color oscuro para el efecto de emborrachamiento

    public Alcohol_Effect alcoholEffect; // Puedes asignar este en el Inspector

    public GameObject crossed_sword;

     // Referencia al componente que quieres activar/desactivar cuando actualRon está en el 50% o más
    public Animator animator;
    public Animator characterAnimator; // Animator del personaje para comprobar el estado Attack1
    public GameObject objectToToggle; // El objeto que quieres activar/desactivar
    public GameObject additionalObjectToToggle; // El segundo objeto que quieres activar/desactivar al 50% de ron

    private bool isAnimatorActive = false; // Controla el estado del Animator
    private bool isObjectActive = false;   // Controla el estado del objeto

     private bool isAdditionalObjectActive = false; // Controla el estado del segundo objeto

    public PlayerMovement playerMovement;
    public static bool bebido = false;
    public bool startRum50;

    void Start()
    {
        if (startRum50 == true) {
            actualRon = maximumRon / 2; // El ron actual al empezar sera la mitad del ron maximo
        }
        
        animator.enabled = false; // Asegurarse de que el Animator esté desactivado al inicio
        objectToToggle.SetActive(false); // Desactivar el objeto al inicio
        additionalObjectToToggle.SetActive(false); // Desactivar el segundo objeto al inicio
    }

    // Disminucion del ron mediante el metodo update
    void Update()
    {
        if(actualRon >= 50)
        {
            if(crossed_sword != null)
            {
                crossed_sword.SetActive(true);
            }
        }
        else
        {
            if(crossed_sword != null)
            {
                crossed_sword.SetActive(false);
            }
        }
        
        if (actualRon >= maximumRon && !isMaxRonActive)
        {
            StartCoroutine(MaxRonCoroutine());
        }
        else if (!isMaxRonActive)
        {
            // float actualSpeed = actualRon > 50 ? tasaDisminucionRon : tasaDisminucionRon; // Modifica la velocidad de la tasa de disminucion de 1f a 2f si es mayor que 50

            actualRon -= tasaDisminucionRon * Time.deltaTime; // Ron actual menos la tasa de disminucion por segundo
            actualRon = Mathf.Clamp(actualRon, 0, maximumRon); // Guardamos en actualRon el ron actual que no puede ser inferior a 0 ni superior al maximo de maximumRon
            RonBar.fillAmount = actualRon / maximumRon; // Rellena la imagen en base al ron actual dividido por el ron maximo
            if(RonBarMenu != null) {   
                RonBarMenu.fillAmount = actualRon / maximumRon; // Rellena la imagen en base al ron actual dividido por el ron maximo
            }

            // Si el ron es inferior al 20%, parpadea
            if (actualRon <= 20)
            {
                RonBar.color = Color.Lerp(ronColor, alertColor, Mathf.PingPong(Time.time * 2, 1));  // Parpadeo entre el color de ron y el color de alerta
                if(RonBarMenu != null) {   
                    RonBarMenu.color = Color.Lerp(ronColor, alertColor, Mathf.PingPong(Time.time * 2, 1));
                }
            }
            else
            {
                RonBar.color = ronColor; // Color normal de la barra de ron
                if(RonBarMenu != null) {   
                    RonBarMenu.color = ronColor; // Color normal de la barra de ron
                }
            }
        }
       // Activar o desactivar el Animator según el nivel de ron
        if (actualRon >= maximumRon / 2 && !isAnimatorActive)
        {
            animator.enabled = true; // Activa el Animator
            isAnimatorActive = true;
            bebido = true;
        }
        else if (actualRon < maximumRon / 2 && isAnimatorActive)
        {
            animator.enabled = false; // Desactiva el Animator
            isAnimatorActive = false;
            bebido = false;
        }
       // Activar o desactivar el objeto según el nivel de ron y el estado de la animación Attack1 en el characterAnimator
        AnimatorStateInfo stateInfo = characterAnimator.GetCurrentAnimatorStateInfo(0);
        bool isInAttack1 = stateInfo.IsName("Attack1");

        if (actualRon >= maximumRon / 2 && isInAttack1 && !isObjectActive)
        {
            objectToToggle.SetActive(true); // Activa el objeto
            isObjectActive = true;
        }
        else if ((actualRon < maximumRon / 2 || !isInAttack1) && isObjectActive)
        {
            objectToToggle.SetActive(false); // Desactiva el objeto
            isObjectActive = false;
        }

        // Activar o desactivar el segundo objeto solo según el nivel de ron
        if (actualRon >= maximumRon / 2 && !isAdditionalObjectActive)
        {
            additionalObjectToToggle.SetActive(true); // Activa el segundo objeto
            isAdditionalObjectActive = true;
        }
        else if (actualRon < maximumRon / 2 && isAdditionalObjectActive)
        {
            additionalObjectToToggle.SetActive(false); // Desactiva el segundo objeto
            isAdditionalObjectActive = false;
        }
    }

    // Funcion que aumenta la barra de ron al beber de forma progresiva
    public void DrinkRon(float quantity)
    {
        StartCoroutine(AumentarRonProgresivamente(quantity));
    }

    // Corrutina para aumentar la cantidad de ron de forma gradual
    private IEnumerator AumentarRonProgresivamente(float quantity)
    {
        float startIncrease = actualRon;
        float finalIncrese = Mathf.Clamp(actualRon + quantity, 0, maximumRon);
        float elapsedTime = 0f;

        while (elapsedTime < fillTime)
        {
            elapsedTime += Time.deltaTime;
            actualRon = Mathf.Lerp(startIncrease, finalIncrese, elapsedTime / fillTime);
            RonBar.fillAmount = actualRon / maximumRon;
            if(RonBarMenu != null) {   
                RonBarMenu.fillAmount = actualRon / maximumRon;
            }
            yield return null;
        }

        actualRon = finalIncrese; // Asegurarse de que el valor final se asigna
    }

    // Si el jugador se pasa bebiendo ron y llega al maximo la barra dejara de bajar por "x" tiempo y invocara los efectos convenientes
    private IEnumerator MaxRonCoroutine()
    {
        Debug.Log("Se ha superado el limite de ron del jugador");
        isMaxRonActive = true;
        RonBar.color = drunkColor; // Cambia el color de la barra de ron a uno mas intenso para indicar que estas borracho
        if(RonBarMenu != null) {   
            RonBarMenu.color = drunkColor; // Cambia el color de la barra de ron a uno mas intenso para indicar que estas borracho
        }
      
        // A�adir aqui las funciones negativas que queremos que se activen cuando te emborrachas demasiado
        alcoholEffect.Alcohol_Effect_Ya();
        playerMovement.ActivateRandomInvertedControls();

        yield return new WaitForSeconds(maxRonDuration);
        RonBar.color = ronColor; // Restaurar el color de la barra despu�s del tiempo
        if(RonBarMenu != null) {   
            RonBarMenu.color = ronColor; // Restaurar el color de la barra despu�s del tiempo
        }
        actualRon = maximumRon * 0.99f; // Baja el ron al 99% para evitar que se vuelva a activar inmediatamente
        isMaxRonActive = false; // Reactiva la disminuci�n del ron
        Debug.Log("Vuelve a bajar el ron");
    }
}
