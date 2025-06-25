using UnityEngine;
using UnityEditor;

public class CreacionPaletEditor
{
    [MenuItem("Herramientas/Crear paleta de colores")]
    public static void PintaMuestra()
    {
        GameObject paleta = GameObject.FindGameObjectWithTag("paleta");
        if (paleta == null)
        {
            Debug.LogWarning("No se encontró un objeto con la etiqueta 'paleta'");
            return;
        }

        CambiarColor(paleta, "Color_1", Constants.rojo);
        CambiarColor(paleta, "Color_2", Constants.verde);
        CambiarColor(paleta, "Color_3", Constants.azul);
        CambiarColor(paleta, "Color_4", Constants.amarillo);
        CambiarColor(paleta, "Color_5", Constants.morado);
        CambiarColor(paleta, "Color_6", Constants.naranja);
    }

    private static void CambiarColor(GameObject paleta, string nombre, Color color)
    {
        Transform muestraTransform = paleta.transform.Find(nombre);
        if (muestraTransform != null)
        {
            SpriteRenderer renderer = muestraTransform.GetComponent<SpriteRenderer>();
            if (renderer != null)
            {
                renderer.material.color = color;
            }
        }
    }
}
