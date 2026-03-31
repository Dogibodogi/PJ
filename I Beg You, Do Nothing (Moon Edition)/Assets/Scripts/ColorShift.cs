using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
[RequireComponent(typeof(Image))]
public class GridCellToggle : MonoBehaviour
{
    private Image img;
    private Button btn;
    private bool isOn = false;

    private Color offColor = Color.white;
    private Color onColor = new Color(0.68f, 0.85f, 0.90f, 1f); // baby blue

    void Awake()
    {
        img = GetComponent<Image>();
        btn = GetComponent<Button>();
        btn.onClick.AddListener(ToggleCell);
        UpdateVisual();
    }

    void ToggleCell()
    {
        isOn = !isOn;
        UpdateVisual();
    }

    void UpdateVisual()
    {
        if (isOn == true)
            img.color = onColor;
        else
        {
             img.color = offColor;
        }
    }
}