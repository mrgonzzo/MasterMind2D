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
        // Cierra la aplicación
        Application.Quit();
    }

    public void NewGame()
    {
        // Recarga la escena actual reiniciando así lla partida
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnPlayTurnButtonPressed()
    {
        gameControllerInstance.PlayTurnRequest();
    }

}
