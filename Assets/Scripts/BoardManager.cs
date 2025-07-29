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

    [Header("betTurns por turno")]
    [SerializeField] private List<Transform> betTurns; // Lista de contenedores de pines de apuesta (Turn_0, Turn_1, etc.)
    [SerializeField] private int maxTurns = 9;
    private int currentTurnIndex = 0;
    // Referencias a otros controladores del juego
    [SerializeField] private GameController gameControllerInstance;
    [SerializeField] private TurnController turnControllerInstance;

    private Color selectedColor = Constants.grisInactivo; // Color seleccionado actual, inicia como inactivo

    private int selectedColorCode;
    int[] secretCode;
    // Start es llamado antes del primer frame
    void Start()
    {
        if (gameControllerInstance == null)
        {
            gameControllerInstance = UnityEngine.Object.FindFirstObjectByType<GameController>();
            secretCode = gameControllerInstance.GetSecretCode();
        }
        else {
            secretCode = gameControllerInstance.GetSecretCode();
        }

            // Oculta el código secreto al iniciar la partida
            SecretCodeCoverSwitch();       

        // Dibuja el código secreto en pantalla (aunque esté oculto visualmente)
        DrawSecretCode(secretCode);

        // Activa el collider de Turn_0 (para permitir interacción)
        GameObject currentTurnObjet = GameObject.Find("Turn_0");
        // Activa todos los colliders de los CodePin dentro del turno actual
        foreach (Collider2D col in currentTurnObjet.GetComponentsInChildren<Collider2D>())
        {
            col.enabled = true;
        }

        ValidateSetup();
    }



    // Update se llama una vez por frame, de momento sin uso
    void Update() { }

    /// <summary>
    /// Dibuja el código secreto usando los valores del array recibido.
    /// </summary>
    public void DrawSecretCode(int[] secretCode)
    {
        for (int i = 0; i <= 3; i++)
        {
            Transform codePinTransform = secretCodeGameObjet.transform.Find("CodePin_" + i);
            SpriteRenderer codePinRenderer = codePinTransform.GetComponent<SpriteRenderer>();

            // Traduce el número a su color correspondiente y lo pinta
            switch (secretCode[i])
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
    private void HandleColorPinClicked(Color color, int code)
    {
        selectedColor = color;
        selectedColorCode = code;
    }

    /// <summary>
    /// Método para manejar clics sobre pines de apuesta específicos  y pinta la apuesta..
    /// </summary>
    private void HandleCodePinClicked(CodePin codePin)
    {
        codePin.SetColor(selectedColor, selectedColorCode);
    }

    public void EndCurrentTurnAndActivateNext()
    {
        if (betTurns == null || betTurns.Count == 0) return;

        if (currentTurnIndex >= maxTurns)
        {
            Debug.Log("Ya no hay más turnos disponibles.");
            // EndOfGame()
            return;
        }

        // 1. Bloquear turno actual
        Transform turnoActual = betTurns[currentTurnIndex];
        foreach (Collider2D col in turnoActual.GetComponentsInChildren<Collider2D>())
        {
            col.enabled = false;
        }

        // 2. Avanzar turno
        currentTurnIndex++;

        // 3. Activar siguiente turno si no hemos llegado al límite
        if (currentTurnIndex < maxTurns)
        {
            Transform siguienteTurno = betTurns[currentTurnIndex];
            foreach (Collider2D col in siguienteTurno.GetComponentsInChildren<Collider2D>())
            {
                col.enabled = true;
            }
        }
        else
        {
            Debug.Log(" Se completaron todos los turnos.");
            // EndOfGame()
            // Aquí se puede desactivar botón o notificar al GameController
        }
    }

    public int[] GetCurrentBetCode()
    {
        int[] betCode = new int[4];

        Transform currentBetCode = betTurns[currentTurnIndex].Find("BetCode_"+ currentTurnIndex);

        for (int i = 0; i < 4; i++)
        {
            var pin = currentBetCode.Find("CodePin_" + i);
            var codePin = pin.GetComponent<CodePin>();
            if (codePin != null)
            {
                betCode[i] = codePin.GetColorCode();
            }
            else
            {
                betCode[i] = 0;
            }
        }

        return betCode;
    }

    /// <summary>
    /// Pinta los pines de respuesta (negros y blancos) en el turno actual.
    /// </summary>
    public void DrawResponse(int blackPins, int whitePins)
    {
        Transform turnoActual = betTurns[currentTurnIndex];
        Transform respuestaActual = turnoActual.Find("Response_" + currentTurnIndex);

        if (respuestaActual == null)
        {
            Debug.LogError($"No se encontró Response_{currentTurnIndex} en {turnoActual.name}");
            return;
        }

        int index = 0;

        for (int i = 0; i < blackPins; i++)
        {
            SpriteRenderer sr = respuestaActual.Find("ResponsePin_" + index)?.GetComponent<SpriteRenderer>();
            if (sr != null)
                sr.color = Constants.negro;
            index++;
        }

        for (int i = 0; i < whitePins; i++)
        {
            SpriteRenderer sr = respuestaActual.Find("ResponsePin_" + index)?.GetComponent<SpriteRenderer>();
            if (sr != null)
                sr.color = Constants.blanco;
            index++;
        }
    }




    private void ValidateSetup()
    {
        Debug.Log("Validando estructura del tablero...");

        if (gameControllerInstance == null) Debug.LogError("GameController no asignado");
        if (turnControllerInstance == null) Debug.LogError("TurnController no asignado");
        if (secretCodeGameObjet == null) Debug.LogError("secretCodeGameObjet no asignado");
        if (coverGameObjet == null) Debug.LogError("coverGameObjet no asignado");

        if (colorPins == null || colorPins.Count == 0)
            Debug.LogError("Lista de ColorPins vacía o no asignada");

        if (betTurns == null || betTurns.Count == 0)
        {
            Debug.LogError("Lista de betTurns vacía o no asignada");
            return;
        }

        for (int t = 0; t < betTurns.Count; t++)
        {
            Transform turno = betTurns[t];
            if (turno == null)
            {
                Debug.LogError($"Turno {t} es null");
                continue;
            }

            // 🔸 BetCode_X
            Transform apuesta = turno.Find("BetCode_" + t);
            if (apuesta == null)
            {
                Debug.LogError($"No se encontró BetCode_{t} en Turn_{t}");
            }
            else
            {
                for (int i = 0; i < 4; i++)
                {
                    Transform pin = apuesta.Find("CodePin_" + i);
                    if (pin == null)
                    {
                        Debug.LogError($"Faltante: CodePin_{i} en BetCode_{t}");
                    }
                    else if (pin.GetComponent<SpriteRenderer>() == null)
                    {
                        Debug.LogError($"CodePin_{i} en BetCode_{t} no tiene SpriteRenderer");
                    }
                }
            }

            // 🔸 Response_X
            Transform respuesta = turno.Find("Response_" + t);
            if (respuesta == null)
            {
                Debug.LogError($"Faltante: contenedor Response_{t} en Turn_{t}");
            }
            else
            {
                for (int r = 0; r < 4; r++)
                {
                    Transform pin = respuesta.Find("ResponsePin_" + r);
                    if (pin == null)
                    {
                        Debug.LogError($"Faltante: ResponsePin_{r} en Response_{t}");
                    }
                    else if (pin.GetComponent<SpriteRenderer>() == null)
                    {
                        Debug.LogError($"ResponsePin_{r} en Response_{t} no tiene SpriteRenderer");
                    }
                }
            }
        }

        Debug.Log("Validación completada.");
    }


}
