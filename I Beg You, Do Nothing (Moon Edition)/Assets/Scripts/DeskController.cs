using UnityEngine;

public class DeskController : MonoBehaviour
{
    [Header("Desk Settings")]
    public Color alertColor = Color.red;
    private Color originalColor;

    // We will use this to change the desk color later when things go wrong!
    private UnityEngine.UI.Image deskImage;

    void Start()
    {
        // This grabs the Image component off your desk panel so we can manipulate it
        deskImage = GetComponent<UnityEngine.UI.Image>();

        if (deskImage != null)
        {
            originalColor = deskImage.color;
        }
        else
        {
            Debug.LogError("No Image component found! Make sure this script is attached to your DeskBackground panel.");
        }
    }

    // A public function that our future buttons can trigger
    public void OnRedButtonPressed()
    {
        Debug.Log("WARNING: You touched something!");

        // Example of something happening: The desk flashes red!
        if (deskImage != null)
        {
            deskImage.color = alertColor;
            // Invokes a method to reset the color after 0.5 seconds
            Invoke("ResetDeskColor", 0.5f);
        }
    }

    private void ResetDeskColor()
    {
        if (deskImage != null)
        {
            deskImage.color = originalColor;
        }
    }
}