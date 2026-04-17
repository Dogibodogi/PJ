// RedButtonController.cs
// Attach this to the RedButton GameObject

using UnityEngine;
using UnityEngine.UI;

public class RedButtonController : MonoBehaviour
{
    [Header("Press Settings")]
    public int pressesRequired = 30;

    [Header("References")]
    public EndingManager endingManager; // drag EndingCanvas here

    private int pressCount = 0;
    private bool endingTriggered = false;
    private Button btn;

    void Awake()
    {
        btn = GetComponent<Button>();
        btn.onClick.AddListener(OnButtonPressed);
    }

    void OnButtonPressed()
    {
        if (endingTriggered) return; // stop counting after ending fires

        pressCount++;
        Debug.Log($"Red button pressed: {pressCount}/{pressesRequired}");

        if (pressCount >= pressesRequired)
        {
            endingTriggered = true;
            endingManager?.TriggerEnding();
        }
    }
}