using UnityEngine;

public class CatalogPanelController : MonoBehaviour
{
    // Singleton instance to access this script from anywhere
    public static CatalogPanelController Instance { get; private set; }

    // Reference to the panel you want to show/hide
    public GameObject panelToShow;

    [Header("Unlockable Catalog Items")]
    // Reference to the specific image/object to unlock in the catalog
    public GameObject endingLogImage;

    private void Awake()
    {
        // Set up the Singleton instance
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    void Start()
    {
        // Check if the ending was unlocked in a previous session
        // Default is 0 (false) if the key doesn't exist yet
        if (endingLogImage != null)
        {
            bool isUnlocked = PlayerPrefs.GetInt("NuclearEndingUnlocked", 0) == 1;
            endingLogImage.SetActive(isUnlocked);
        }
    }

    // Method to unlock a specific planet in the catalog
    public void UnlockPlanet(string planetName)
    {
        Debug.Log("Unlocking planet in catalog: " + planetName);

        // Save the unlock state for this specific planet to PlayerPrefs
        PlayerPrefs.SetInt("UnlockedPlanet_" + planetName, 1);
        PlayerPrefs.Save();

        // TODO: You can add logic here to activate specific images based on the planetName string
        // Example: if (planetName == "Mars") { marsImage.SetActive(true); }
    }

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