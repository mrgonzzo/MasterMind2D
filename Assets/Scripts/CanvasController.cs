using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CanvasController : MonoBehaviour
{
    [SerializeField] private BoardManager boardManagerInstance;
    [SerializeField] private TurnController turnControllerInstance;

    void Start() 
    {
        if (boardManagerInstance == null)
        {
            boardManagerInstance = UnityEngine.Object.FindFirstObjectByType<BoardManager>();
        }

        if (turnControllerInstance == null)
        {
            turnControllerInstance = UnityEngine.Object.FindFirstObjectByType<TurnController>();
        }
    }
    //Control de los botones
    public void CloseApp()
    {
        // Cierra la aplicación
        Application.Quit();
    }

    public void NewGame()
    {
        // Recarga la escena actual reiniciando así lla partida
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void PlayTurn() 
    {
        Debug.Log($"PlayTurn()");

        int[] apuesta = boardManagerInstance.GetCurrentBetCode();

        Debug.Log($"GetCurrentBetCode() obtenido");
        (int negros, int blancos) = turnControllerInstance.SubmitTurn(apuesta);
        boardManagerInstance.DrawResponse(negros, blancos);
        boardManagerInstance.EndCurrentTurnAndActivateNext();
    }
}
