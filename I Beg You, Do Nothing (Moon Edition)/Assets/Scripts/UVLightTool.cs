using UnityEngine;
using UnityEngine.UI;

public class UVLightTool : InteractableTool
{
    [Header("UV Light Settings")]
    public bool isUVLightOn = false;
    public Color offColor = Color.white;
    public Color onColor = new Color(0.6f, 0.2f, 1f); // O culoare mov/ultraviolet

    private Image toolImage;

    // Definim un "Eveniment" la care alte scripturi se pot abona
    public delegate void UVLightStateChanged(bool isOn);
    public static event UVLightStateChanged OnUVLightStateChanged;

    void Start()
    {
        toolImage = GetComponent<Image>();
        UpdateVisuals();
    }

    // Aici scriem logica specifică pentru funcția UseTool cerută de clasa părinte
    public override void UseTool()
    {
        // Inversăm starea (din ON în OFF și invers)
        isUVLightOn = !isUVLightOn;
        UpdateVisuals();

        // Anunțăm toate obiectele din joc că starea luminii s-a schimbat
        if (OnUVLightStateChanged != null)
        {
            OnUVLightStateChanged(isUVLightOn);
        }

        Debug.Log(toolName + " este acum " + (isUVLightOn ? "APRINSĂ" : "STINSĂ"));
    }

    private void UpdateVisuals()
    {
        if (toolImage != null)
        {
            // Schimbăm culoarea uneltei ca să știm vizual că e aprinsă
            toolImage.color = isUVLightOn ? onColor : offColor;
        }
    }
}