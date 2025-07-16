using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CanvasController : MonoBehaviour
{
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
        Debug.Log("siguiente turno");
    }
}
