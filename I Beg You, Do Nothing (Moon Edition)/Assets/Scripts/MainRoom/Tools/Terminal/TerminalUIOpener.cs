using UnityEngine;
using UnityEngine.EventSystems;

public class TerminalUIOpener : MonoBehaviour, IPointerClickHandler
{
    [Header("References")]
    [Tooltip("Drag the object with the TerminalController script here")]
    public TerminalController terminalController;

    // This detects the click on the Image
    public void OnPointerClick(PointerEventData eventData)
    {
        if (terminalController != null)
        {
            Debug.Log("Terminal image clicked! Opening terminal...");
            terminalController.ToggleTerminal(true);
        }
        else
        {
            // Just in case you forgot to drag the reference in the Inspector
            terminalController = FindObjectOfType<TerminalController>();
            if (terminalController != null) terminalController.ToggleTerminal(true);
            else Debug.LogError("TerminalUIOpener: No TerminalController found in scene!");
        }
    }
}