using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;
using UnityEngine.SceneManagement;

/// <summary>
/// Se encarga de gestionar el tablero de juego: generación del código secreto,
/// pintura de pines de apuesta, manejo del turno actual y suscripción a eventos de entrada.
/// </summary>
public class BoardManager : MonoBehaviour
{
    [Header("Código secreto y su tapa")]
    [SerializeField] private GameObject secretCodeGameObjet;  // Referencia al GameObject del código secreto
    [SerializeField] private GameObject coverGameObjet;       // Referencia a la tapa del código secreto

    [Header("Entrada de usuario")]
    [SerializeField] private MouseInput2DHandler inputHandler; // Referencia al manejador de entrada del mouse

    [Header("Referencias del esquema de color")]
    [SerializeField] private List<ColorPin> colorPins; // Lista de pines de colores disponibles para elegir

    [Header("BetCode por turno")]
    [SerializeField] private List<Transform> betCodeTurns; // Lista de contenedores de pines de apuesta (Turn_0, Turn_1, etc.)

    // Referencias a otros controladores del juego
    [SerializeField] private GameController gameControllerInstance;
    [SerializeField] private TurnController turnControllerInstance;

    private Color selectedColor = Constants.grisInactivo; // Color seleccionado actual, inicia como inactivo
    private int currentTurnIndex = 0; // Índice del turno actual (empieza en 0)

    // Start es llamado antes del primer frame
    void Start()
    {
        if (gameControllerInstance == null)
        {
            gameControllerInstance = UnityEngine.Object.FindFirstObjectByType<GameController>();
        }
        // Genera un nuevo código secreto de 4 colores aleatorios
        int[] secretCodeArray = gameControllerInstance.CodeGenerator(4, 1, 6);

        // Oculta el código secreto al iniciar la partida
        SecretCodeCoverSwitch();

        // Dibuja el código secreto en pantalla (aunque esté oculto visualmente)
        DrawSecretCode(secretCodeArray);

        // Activa el collider de Turn_0 (para permitir interacción)
        GameObject currentTurnObjet = GameObject.Find("Turn_0");
        // Activa todos los colliders de los CodePin dentro del turno actual
        foreach (Collider2D col in currentTurnObjet.GetComponentsInChildren<Collider2D>())
        {
            col.enabled = true;
        }
    }

    // Update se llama una vez por frame, de momento sin uso
    void Update() { }

    /// <summary>
    /// Dibuja el código secreto usando los valores del array recibido.
    /// </summary>
    public void DrawSecretCode(int[] secretCodeArray)
    {
        Debug.Log("SecretCodeManager secretCodeArray[] = [" + secretCodeArray[0] + "," + secretCodeArray[1] + "," + secretCodeArray[2] + "," + secretCodeArray[3] + "]");

        for (int i = 0; i <= 3; i++)
        {
            Transform codePinTransform = secretCodeGameObjet.transform.Find("CodePin_" + i);
            SpriteRenderer codePinRenderer = codePinTransform.GetComponent<SpriteRenderer>();

            // Traduce el número a su color correspondiente y lo pinta
            switch (secretCodeArray[i])
            {
                case 1:
                    codePinRenderer.color = Constants.rojo;
                    break;
                case 2:
                    codePinRenderer.color = Constants.verde;
                    break;
                case 3:
                    codePinRenderer.color = Constants.azul;
                    break;
                case 4:
                    codePinRenderer.color = Constants.amarillo;
                    break;
                case 5:
                    codePinRenderer.color = Constants.morado;
                    break;
                case 6:
                    codePinRenderer.color = Constants.naranja;
                    break;
            }
        }
    }

    /// <summary>
    /// Activa o desactiva visualmente la tapa que cubre el código secreto.
    /// </summary>
    public void SecretCodeCoverSwitch()
    {
        if (coverGameObjet == null)
        {
            Debug.Log("SecretCodeManager error al asignar la tapa");
        }
        else
        {
            SpriteRenderer spriteRenderer = coverGameObjet.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                spriteRenderer.enabled = !spriteRenderer.enabled;
            }
        }
    }

    /// <summary>
    /// Suscripción a eventos cuando el objeto se activa.
    /// </summary>
    private void OnEnable()
    {
        inputHandler.OnColorPinClicked += HandleColorPinClicked;
        inputHandler.OnCodePinClicked += HandleCodePinClicked; // Comentado por ahora
    }

    /// <summary>
    /// Desuscripción a eventos cuando el objeto se desactiva.
    /// </summary>
    private void OnDisable()
    {
        inputHandler.OnColorPinClicked -= HandleColorPinClicked;
        inputHandler.OnCodePinClicked -= HandleCodePinClicked;
    }

    /// <summary>
    /// Maneja la lógica al hacer clic en un ColorPin: guarda el color
    /// </summary>
    private void HandleColorPinClicked(Color color)
    {
        selectedColor = color;
        Debug.Log("Color seleccionado: " + selectedColor);

    }

    /// <summary>
    /// Método para manejar clics sobre pines de apuesta específicos  y pinta la apuesta..
    /// </summary>
    private void HandleCodePinClicked(CodePin codePin)
    {
        codePin.SetColor(selectedColor);
    }

    //Control de los botones
    public void CloseApp()
    {
        Application.Quit();
        Debug.Log("La aplicación se está cerrando...");
    }

    public void NewGame()
    {
        // Recarga la escena actual
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
