using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

    private int[] secretCodeArray;
    int currentTurn;
    [SerializeField] private BoardManager boardManagerinstance;
    [SerializeField] private TurnController turnControllerInstance;
    // Start is called before the first frame update
    void Start()
    {
        // Genera un nuevo código secreto de 4 colores aleatorios
        secretCodeArray = CodeGenerator(4, 1, 6);
    }

    public int[] CodeGenerator(int length, int minValue, int maxValue)
    {
        int[] array = new int[length];
        for (int i = 0; i < length; i++)
        {
            array[i] = UnityEngine.Random.Range(minValue, maxValue + 1);
        }
        return array;
    }

    public int[] GetSecretCode()
    {
        return secretCodeArray;
    }

}
