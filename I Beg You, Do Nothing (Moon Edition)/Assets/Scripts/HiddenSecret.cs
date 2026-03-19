using UnityEngine;
using UnityEngine.UI;

// Ne asigurăm că obiectul are o componentă Image
[RequireComponent(typeof(Image))]
public class HiddenSecret : MonoBehaviour
{
    [Header("Secret Settings")]
    [Tooltip("Transparența când lumina UV este STINSĂ (0 = invizibil)")]
    public float hiddenAlpha = 0f;
    [Tooltip("Transparența când lumina UV este APRINSĂ (1 = complet vizibil)")]
    public float revealedAlpha = 1f;

    private Image secretImage;

    void Awake()
    {
        secretImage = GetComponent<Image>();
        // La început, secretul este invizibil
        SetVisibility(false);
    }

    void OnEnable()
    {
        // Când acest obiect este activ, se abonează să asculte lampa UV
        UVLightTool.OnUVLightStateChanged += HandleUVLightState;
    }

    void OnDisable()
    {
        // Când obiectul este distrus/dezactivat, se dezabonează pentru a preveni erori
        UVLightTool.OnUVLightStateChanged -= HandleUVLightState;
    }

    private void HandleUVLightState(bool isUVOn)
    {
        SetVisibility(isUVOn);
    }

    private void SetVisibility(bool isVisible)
    {
        if (secretImage != null)
        {
            // Modificăm doar "Alpha" (transparența) din culoarea imaginii
            Color currentColor = secretImage.color;
            currentColor.a = isVisible ? revealedAlpha : hiddenAlpha;
            secretImage.color = currentColor;
        }
    }
}