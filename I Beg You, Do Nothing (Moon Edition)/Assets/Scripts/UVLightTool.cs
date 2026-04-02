using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UVLightTool : InteractableTool, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("UV Light Settings")]
    public bool isUVLightOn = false;
    public Color offColor = Color.white;
    public Color onColor = new Color(0.75f, 0.55f, 1f, 1f);

    [Header("UV Spot")]
    public Image uvSpotImage;
    public float revealRadius = 100f;
    public Vector2 spotOffset = new Vector2(80f, 0f);

    [Header("References")]
    public Button switchButton;

    private Image flashLightImage;
    private RectTransform rectTransform;
    private RectTransform spotRectTransform;
    private Canvas canvas;

    private bool isDraggingTool = false;

    public delegate void UVLightStateChanged(bool isOn);
    public static event UVLightStateChanged OnUVLightStateChanged;

    void Start()
    {
        flashLightImage = GetComponent<Image>();
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();

        if (uvSpotImage != null)
        {
            spotRectTransform = uvSpotImage.GetComponent<RectTransform>();
            uvSpotImage.raycastTarget = true;
        }

        UpdateVisuals();
        UpdateSpotPosition();
    }

    public override void UseTool()
    {
    }

    public void ToggleLight()
    {
        isUVLightOn = !isUVLightOn;
        UpdateVisuals();
        OnUVLightStateChanged?.Invoke(isUVLightOn);
        Debug.Log(toolName + " is now: " + (isUVLightOn ? "ON" : "OFF"));
    }

    private void UpdateVisuals()
    {
        if (flashLightImage != null)
            flashLightImage.color = isUVLightOn ? onColor : offColor;

        if (uvSpotImage != null)
            uvSpotImage.enabled = isUVLightOn;
    }

    private void UpdateSpotPosition()
    {
        if (spotRectTransform != null)
        {
            // UVSpot stays offset from the flashlight parent
            spotRectTransform.anchoredPosition = spotOffset;
        }

        // Update the switch button sprite
        if (switchImage != null)
        {
            switchImage.sprite = isUVLightOn ? switchOnSprite : switchOffSprite;
        }
    }

    public Vector2 GetSpotWorldPosition()
    {
        if (spotRectTransform != null)
            return spotRectTransform.position;

        return rectTransform.position;
    }

    private bool PointerStartedOnSwitch(PointerEventData eventData)
    {
        if (switchButton == null)
            return false;

        GameObject hitObject = eventData.pointerPressRaycast.gameObject;

        if (hitObject == null)
            hitObject = eventData.pointerCurrentRaycast.gameObject;

        if (hitObject == null)
            return false;

        return hitObject.transform == switchButton.transform || hitObject.transform.IsChildOf(switchButton.transform);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // if player started on the switch, do not drag.
        if (PointerStartedOnSwitch(eventData))
        {
            isDraggingTool = false;
            return;
        }

        isDraggingTool = true;

        // bring the whole flashlight to front
        transform.SetAsLastSibling();

        // keep switch above UVSpot inside this hierarchy
        if (switchButton != null)
            switchButton.transform.SetAsLastSibling();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isDraggingTool)
            return;

        if (canvas != null)
            rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        else
            transform.position = eventData.position;

        UpdateSpotPosition();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isDraggingTool = false;
    }
}