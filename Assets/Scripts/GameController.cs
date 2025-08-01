using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controlador principal de la partida. Genera el c�digo secreto, procesa turnos, 
/// eval�a condiciones de victoria o derrota, y comunica los resultados al BoardManager.
/// </summary>
public class GameController : MonoBehaviour
{
	/// <summary>
	/// C�digo secreto generado al inicio de la partida.
	/// </summary>
	private int[] secretCodeArray;

	/// <summary>
	/// Referencia al gestor del tablero para obtener apuestas y dibujar respuestas.
	/// </summary>
	[SerializeField] private BoardManager boardManagerInstance;

	/// <summary>
	/// Referencia al controlador de turnos que compara la apuesta con el c�digo secreto.
	/// </summary>
	[SerializeField] private TurnController turnControllerInstance;

	/// <summary>
	/// �ndice del turno actual.
	/// </summary>
	private int currentTurn = 0;

	/// <summary>
	/// N�mero m�ximo de turnos permitidos en la partida.
	/// </summary>
	[SerializeField] private int maxTurns = 9;

	/// <summary>
	/// Se ejecuta al inicio de la partida. Genera el c�digo secreto.
	/// </summary>
	void Start()
	{
		secretCodeArray = CodeGenerator(4, 1, 6);
	}

	/// <summary>
	/// Genera un array de n�meros aleatorios entre minValue y maxValue (ambos inclusivos).
	/// </summary>
	/// <param name="length">Cantidad de elementos a generar</param>
	/// <param name="minValue">Valor m�nimo permitido</param>
	/// <param name="maxValue">Valor m�ximo permitido</param>
	/// <returns>Array con la secuencia generada</returns>
	public int[] CodeGenerator(int length, int minValue, int maxValue)
	{
		int[] array = new int[length];
		for (int i = 0; i < length; i++)
		{
			array[i] = UnityEngine.Random.Range(minValue, maxValue + 1);
		}
		return array;
	}

	/// <summary>
	/// Devuelve el c�digo secreto para que otros componentes puedan acceder a �l.
	/// </summary>
	/// <returns>El array con el c�digo secreto</returns>
	public int[] GetSecretCode()
	{
		return secretCodeArray;
	}

	/// <summary>
	/// Llamado cuando el jugador pulsa el bot�n de jugar turno.
	/// Procesa la apuesta, eval�a la respuesta y avanza al siguiente turno o finaliza el juego.
	/// </summary>
	public void PlayTurnRequest()
	{
		// Obtener la apuesta actual desde el tablero
		int[] apuesta = boardManagerInstance.GetCurrentBetCode();

		// Evaluar la apuesta contra el c�digo secreto
		(int negros, int blancos) = turnControllerInstance.SubmitTurn(apuesta);

		// Mostrar la respuesta en el tablero
		boardManagerInstance.DrawResponse(negros, blancos);

		// Verificar condici�n de victoria
		if (negros == 4)
		{
			Debug.Log("Victoria: el jugador ha adivinado el c�digo secreto.");
			EndGame(true);
			return;
		}

		// Avanzar turno en el tablero
		boardManagerInstance.EndCurrentTurnAndActivateNext();
		currentTurn++;

		// Verificar condici�n de derrota (sin m�s turnos disponibles)
		if (currentTurn >= maxTurns)
		{
			Debug.Log("Derrota: el jugador ha agotado todos los turnos.");
			EndGame(false);
		}
	}

	/// <summary>
	/// Finaliza la partida, mostrando el resultado final.
	/// </summary>
	/// <param name="victoria">True si se gan� la partida, False si se perdi�</param>
	private void EndGame(bool victoria)
	{
		//descubrimos el codigo secreto
		boardManagerInstance.SecretCodeCoverSwitch();

		if (victoria)
		{
			
			Debug.Log("Fin de partida: Victoria");
		}
		else
		{
			Debug.Log("Fin de partida: Derrota");
		}

		// Aqu� se puede implementar: desactivar botones, mostrar UI final, cargar escena, etc.
		// Ejemplo:
		// UIController.Instance.ShowEndGameScreen(victoria);
		// SceneManager.LoadScene("PantallaFinal");
	}
}
