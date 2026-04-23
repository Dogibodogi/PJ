using UnityEngine;

public class CatalogPanelController : MonoBehaviour
{
    // Reference to the panel you want to show/hide
    public GameObject panelToShow;

    // Method to call when the "Open" button is clicked
    public void ShowPanel()
    {
        if (panelToShow != null)
        {
            // Activates the panel in the scene
            panelToShow.SetActive(true);
            Debug.Log("Panel successfully opened.");
        }
        else
        {
            Debug.LogWarning("Panel reference is missing. Please assign it in the Inspector.");
        }
    }

    // Method to call when the "Close" button is clicked
    public void HidePanel()
    {
        if (panelToShow != null)
        {
            // Deactivates the panel in the scene
            panelToShow.SetActive(false);
            Debug.Log("Panel successfully closed.");
        }
    }
}