using UnityEngine;
using UnityEngine.EventSystems;

public class NumpadButton : InteractableTool
{
    [Header("Button Settings")]
    [Tooltip("Valoarea butonului: 0, 1, 2... sau CLEAR, ENTER")]
    public string buttonValue;

    private NumpadController controller;

    void Start()
    {
        // Căutăm automat NumpadController-ul pe obiectul părinte sau în scenă
        // Astfel nu trebuie să îl tragi manual pentru fiecare din cele 12 butoane
        controller = GetComponentInParent<NumpadController>();

        if (controller == null)
        {
            controller = FindObjectOfType<NumpadController>();
        }

        if (controller == null)
        {
            Debug.LogError("NumpadButton (" + gameObject.name + ") nu a putut găsi un NumpadController!");
        }
    }

    // Suprascriem funcția din clasa ta de bază InteractableTool
    public override void UseTool()
    {
        if (controller != null)
        {
            // Trimitem valoarea la creierul tastaturii
            controller.ButtonPressed(buttonValue);
        }
    }
}