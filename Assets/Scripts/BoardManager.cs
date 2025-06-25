using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    [SerializeField] private GameObject secretCodeGameObjet;
    [SerializeField] private GameObject coverGameObjet;
    public GameController gameControllerInstance;
    public TurnController turnControllerInstance;
    // Start is called before the first frame update
    void Start()
    {
        int[] secretCodeArray  = gameControllerInstance.CodeGenerator(4, 1, 6);
        masterCodeCoverSwitch();
        DrawSecretCode(secretCodeArray);
        // 2 activar Turn_0
        GameObject currentTurnObjet = GameObject.Find("Turn_"+ turnControllerInstance.currentTurn);        
        currentTurnObjet.GetComponent<Collider2D>().enabled = true;
        //currentTurnObjet.SetActive(false);
}

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DrawSecretCode(int[] secretCodeArray)
    {
        Debug.Log("SecretCodeManager secretCodeArray[] = [" + secretCodeArray[0] + "," + secretCodeArray[1] + "," + secretCodeArray[2] + "," + secretCodeArray[3] + "]");
        for (int i = 0; i <= 3; i++)
        {
            // busecretCodemos el hijo de secretCode correspondiente
            Transform codePinTransform = secretCodeGameObjet.transform.Find("CodePin_" + i);
            // y accedemos a su render
            SpriteRenderer codePinRenderer = codePinTransform.GetComponent<SpriteRenderer>();
            //traducimos los numero a nuestros colores y pintamos
            switch (secretCodeArray[i])
            {
                case 1: //Verde
                    codePinRenderer.color = Constants.rojo;
                    break;
                case 2: //Azul
                    codePinRenderer.color = Constants.verde;
                    break;
                case 3: //Rojo
                    codePinRenderer.color = Constants.azul;
                    break;
                case 4: //Amarillo
                    codePinRenderer.color = Constants.amarillo;
                    break;
                case 5: // morado
                    codePinRenderer.color = Constants.morado;
                    break;
                case 6: //Naranja
                    codePinRenderer.color = Constants.naranja;
                    break;
            } //switch
        } //for
    } //metodo*/


    public void masterCodeCoverSwitch()
    {        
        if (coverGameObjet == null)
        {
            Debug.Log("SecretCodeManager error al asignar la tapa");
        }
        else
        { 
            // Verifica si el objeto tiene un MeshRenderer
            SpriteRenderer spriteRenderer = coverGameObjet.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                // Desactiva el MeshRenderer (no se "pintará" el objeto)
                spriteRenderer.enabled = !spriteRenderer.enabled;
            }
        }
    }

}
