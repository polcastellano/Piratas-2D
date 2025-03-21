using UnityEngine;

public class Map : MonoBehaviour
{
    public Transform player;              // Referencia al transform del jugador
    private float zoomSpeed = 30f;         // Velocidad del zoom
    private float minZoom = 20f; // Defecto 20f          // Límite mínimo de zoom
    private float maxZoom = 60f; // Defecto 133f          // Límite máximo de zoom
    private float panSpeed = 1f;          // Velocidad de desplazamiento

    public Vector2 mapBoundsMin; // Límite inferior del mapa (X, Y)
    public Vector2 mapBoundsMax; // Límite superior del mapa (X, Y)

    private Camera mapCamera;
    private Vector3 dragOrigin;

    void Start()
    {
        mapCamera = GetComponent<Camera>();
        

        if (player == null)
        {
            player = GameObject.FindWithTag("Player").transform;
        }

        FocusOnPlayer(); // Enfoca al jugador por defecto
        SetInitialZoom(); // Asigna el zoom por defecto

    }

    void Update()
    {
        //HandleZoom(); // Permite hacer zoom
        //HandlePan(); // Permite desplazarse

        FocusOnPlayer(); // Centrar al jugador en el mapa constantemente
    }

// Funcion que declara el zoom Inicial de la camara
    void SetInitialZoom()
    {
        if (mapCamera.orthographic)
        {
            mapCamera.orthographicSize = maxZoom;
        }
        else
        {
            Debug.LogWarning("El cálculo para cámaras en perspectiva no está implementado.");
        }
    }

    // Funcionalidad que permite hacer zoom con mando y raton
    void HandleZoom()
    {
        float scroll = 0;

        if (Input.GetAxis("Xbox_LTrigger") > 0.1f)
        {
            scroll = 1;
        }
        else if (Input.GetAxis("Xbox_RTrigger") > 0.1f)
        {
            scroll = -1;
        }

        float mouseScroll = Input.GetAxis("Mouse ScrollWheel");
        if (mouseScroll != 0)
        {
            scroll = mouseScroll > 0 ? 1 : -1;
        }

        if (scroll != 0)
        {
            if (mapCamera.orthographic)
            {
                float previousSize = mapCamera.orthographicSize;
                mapCamera.orthographicSize -= scroll * zoomSpeed;
                mapCamera.orthographicSize = Mathf.Clamp(mapCamera.orthographicSize, minZoom, maxZoom);

                Vector3 mousePositionWorld = mapCamera.ScreenToWorldPoint(Input.mousePosition);
                Vector3 cameraPosition = mapCamera.transform.position;

                Vector3 zoomDifference = mousePositionWorld - cameraPosition;
                mapCamera.transform.position += zoomDifference * (1 - mapCamera.orthographicSize / previousSize);
            }
            else
            {
                Debug.LogWarning("El manejo de zoom para cámaras en perspectiva no está implementado.");
            }
        }
    }
    //Funcion que permite al jugador desplazarse con el joystick derecho o raton 
    void HandlePan()
    {
        float horizontal = Input.GetAxis("Xbox_RightStickHorizontal");
        float vertical = Input.GetAxis("Xbox_RightStickVertical");

        if (Mathf.Abs(horizontal) > 0.1f || Mathf.Abs(vertical) > 0.1f)
        {
            Vector3 direction = new Vector3(horizontal, vertical, 0);
            mapCamera.transform.position += direction * panSpeed * Time.deltaTime;
        }

        if (Input.GetMouseButtonDown(0))
        {
            dragOrigin = mapCamera.ScreenToWorldPoint(Input.mousePosition);
        }

        if (Input.GetMouseButton(0))
        {
            Vector3 difference = dragOrigin - mapCamera.ScreenToWorldPoint(Input.mousePosition);
            mapCamera.transform.position += difference;
        }

        // Limita la posición de la cámara para que no se desplace fuera de los márgenes
        Vector3 currentPos = mapCamera.transform.position;
        currentPos.x = Mathf.Clamp(currentPos.x, mapBoundsMin.x, mapBoundsMax.x);
        currentPos.y = Mathf.Clamp(currentPos.y, mapBoundsMin.y, mapBoundsMax.y);
        currentPos.z = -10f; // Mantén el Z fijo para 2D
        mapCamera.transform.position = currentPos;
    }
    // Funcion que centra el jugador en el mapa
    void FocusOnPlayer()
    {
        if (player != null)
        {
            Vector3 newPos = player.position;
            newPos.z = -10f; 
            mapCamera.transform.position = newPos;
        }
    }
}
