using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    int currentTurn;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
}
