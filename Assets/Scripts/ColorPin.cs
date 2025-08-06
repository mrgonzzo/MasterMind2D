using UnityEngine;

/// <summary>
/// Representa un pin de color en el tablero. Asigna autom�ticamente su color
/// en base al nombre del GameObject (por ejemplo: "ColorPin_1" ser� rojo, etc.).
/// </summary>
public class ColorPin : MonoBehaviour
{
    // Color y colorCode asignado a este pin
    private Color color;
    private int colorCode;
    // Propiedad p�blica para acceder al color desde otros scripts
    public Color PinColor => color;
    public int ColorCode => colorCode;

    /// <summary>
    /// Se llama autom�ticamente al iniciar el objeto. Asigna el color correspondiente.
    /// </summary>
    private void Awake()
    {
        AsignarColorPorNombre();
    }

    /// <summary>
    /// Asigna un color al pin en funci�n del n�mero contenido en su nombre.
    /// Espera nombres como "ColorPin_1", "ColorPin_2", etc.
    /// </summary>
    private void AsignarColorPorNombre()
    {
        string nombre = gameObject.name;

        // Verifica que el nombre comience con "ColorPin_"
        if (nombre.StartsWith("ColorPin_"))
        {
            // Extrae el n�mero despu�s del guion bajo
            string numeroStr = nombre.Substring("ColorPin_".Length);

            // Intenta convertir el n�mero a entero
            if (int.TryParse(numeroStr, out int index))
            {
                // Asigna el color correspondiente al �ndice
                color = ObtenerColorPorIndice(index);
                colorCode = index;
                return;
            }
        }

        // Si el nombre no es v�lido, muestra advertencia y asigna gris
        Debug.LogWarning($"Nombre de objeto no v�lido para ColorPin: {gameObject.name}");
        color = Constants.grisInactivo;
    }

    /// <summary>
    /// Devuelve el color correspondiente al �ndice dado.
    /// </summary>
    /// <param name="index">N�mero del pin extra�do del nombre</param>
    /// <returns>Color asociado al �ndice</returns>
    private Color ObtenerColorPorIndice(int index)
    {
        return index switch
        {
            1 => Constants.rojo,
            2 => Constants.verde,
            3 => Constants.azul,
            4 => Constants.amarillo,
            5 => Constants.morado,
            6 => Constants.naranja,
            _ => Constants.grisInactivo // Color por defecto si el �ndice no es v�lido
        };
    }
}
