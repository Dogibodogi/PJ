using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems; // Required for Drag interfaces

// We add IBeginDragHandler, IDragHandler, and IEndDragHandler to make it draggable
public class UVLightTool : InteractableTool, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("UV Light Settings")]
    public bool isUVLightOn = false;
    public Color offColor = Color.white;
    public Color onColor = new Color(0.6f, 0.2f, 1f); // O culoare mov/ultraviolet

    private Image toolImage;
    private RectTransform rectTransform;
    private Canvas canvas; // Needed to scale dragging movement accurately

    // Definim un "Eveniment" la care alte scripturi se pot abona
    public delegate void UVLightStateChanged(bool isOn);
    public static event UVLightStateChanged OnUVLightStateChanged;

    void Start()
    {
        toolImage = GetComponent<Image>();
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>(); // Find the Canvas this UI lives in

        UpdateVisuals();
    }

    // We override the parent's UseTool to do NOTHING. 
    // This stops the light from toggling when clicking the stick.
    public override void UseTool()
    {
        // Left empty intentionally.
    }

    // This is the new method your Switch Button will call
    public void ToggleLight()
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

    // --- Dragging Logic ---

    public void OnBeginDrag(PointerEventData eventData)
    {
        // Bring the lamp to the front of the screen so it doesn't get dragged behind other UI elements
        transform.SetAsLastSibling();
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Move the lamp smoothly alongside the mouse, scaling correctly with the Canvas
        if (canvas != null)
        {
            rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        }
        else
        {
            // Fallback if Canvas somehow isn't found
            transform.position = eventData.position;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // Add any logic here if you want something to happen when you drop the lamp
    }
}