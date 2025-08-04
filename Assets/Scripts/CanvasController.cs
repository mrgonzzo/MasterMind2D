using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CanvasController : MonoBehaviour
{
    [SerializeField] private BoardManager boardManagerInstance;
    [SerializeField] private GameController gameControllerInstance;

    void Start() 
    {
        if (boardManagerInstance == null)
        {
            boardManagerInstance = UnityEngine.Object.FindFirstObjectByType<BoardManager>();
        }

        if (gameControllerInstance == null)
        {
            gameControllerInstance = UnityEngine.Object.FindFirstObjectByType<GameController>();
        }
    }
    //Control de los botones
    public void CloseApp()
    {
        // Cierra la aplicaci�n
        Application.Quit();
    }

    public void NewGame()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        else {
            // Recarga la escena de juego reiniciando as� lla partida
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

    }

    public void OnPlayTurnButtonPressed()
    {
        gameControllerInstance.PlayTurnRequest();
    }

}
