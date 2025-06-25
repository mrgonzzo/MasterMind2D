using UnityEngine;
using UnityEditor;

public class ReplaceCodePinsInCodePrefab
{
    [MenuItem("Tools/Replace CodePins in Code Prefab")]
    static void ReplaceCodePins()
    {
        // Ruta relativa al prefab
        string CodePrefabPath = "Assets/Prefabs/Code.prefab";
        string codePinPrefabPath = "Assets/Prefabs/CodePin.prefab";

        // Cargar los prefabs
        GameObject CodePrefab = AssetDatabase.LoadAssetAtPath<GameObject>(CodePrefabPath);
        GameObject codePinPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(codePinPrefabPath);

        if (CodePrefab == null || codePinPrefab == null)
        {
            Debug.LogError("No se encontraron los prefabs. Revisa las rutas.");
            return;
        }

        // Instanciar el Code temporalmente para editarlo
        GameObject tempCode = PrefabUtility.InstantiatePrefab(CodePrefab) as GameObject;

        Transform betCode = tempCode.transform.Find("CodePin_0");
        if (betCode == null)
        {
            Debug.LogError("No se encontró el objeto 'CodePin_0' en el Code prefab.");
            return;
        }

        // Eliminar CodePins actuales
        foreach (Transform child in betCode)
        {
            GameObject.DestroyImmediate(child.gameObject);
        }

        // Instanciar nuevos CodePins desde el prefab
        for (int i = 0; i < 4; i++)
        {
            GameObject newPin = (GameObject)PrefabUtility.InstantiatePrefab(codePinPrefab, betCode);
            newPin.name = $"CodePin_{i}";
            newPin.transform.localPosition = new Vector3(i * 0.5f, 0, 0); // Ajusta esto a tu layout real
        }

        // Aplicar los cambios al prefab original
        PrefabUtility.SaveAsPrefabAsset(tempCode, CodePrefabPath);
        GameObject.DestroyImmediate(tempCode);

        Debug.Log("✔ Reemplazo de CodePins completado en el prefab Code.");
    }
}

