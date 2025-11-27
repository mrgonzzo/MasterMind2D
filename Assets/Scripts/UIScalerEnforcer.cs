using UnityEngine;
using UnityEngine.UI;

public class UIScalerEnforcer : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private Vector2 referenceResolution = new Vector2(1920, 1080);
    [Range(0f, 1f)]
    [SerializeField] private float matchWidthOrHeight = 0.5f;

    void Start()
    {
        EnforceCanvasScaling();
    }

    public void EnforceCanvasScaling()
    {
        CanvasScaler[] scalers = FindObjectsOfType<CanvasScaler>();

        foreach (var scaler in scalers)
        {
            if (scaler.uiScaleMode != CanvasScaler.ScaleMode.ScaleWithScreenSize)
            {
                scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
                Debug.Log($"Updated UI Scale Mode for {scaler.gameObject.name}");
            }

            if (scaler.referenceResolution != referenceResolution)
            {
                scaler.referenceResolution = referenceResolution;
                Debug.Log($"Updated Reference Resolution for {scaler.gameObject.name}");
            }

            if (Mathf.Abs(scaler.matchWidthOrHeight - matchWidthOrHeight) > 0.01f)
            {
                scaler.matchWidthOrHeight = matchWidthOrHeight;
                Debug.Log($"Updated Match Width/Height for {scaler.gameObject.name}");
            }
        }
    }
}
