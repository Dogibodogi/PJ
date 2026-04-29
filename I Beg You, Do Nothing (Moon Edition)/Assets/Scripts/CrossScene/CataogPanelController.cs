using UnityEngine;
using System.Collections.Generic;

public class CatalogPanelController : MonoBehaviour
{
    // Singleton instance for easy access
    public static CatalogPanelController Instance { get; private set; }

    // Reference to the panel you want to show/hide
    public GameObject panelToShow;

    [System.Serializable]
    public struct CatalogItem
    {
        public string planetName;
        // The UI Image or GameObject that represents the revealed planet in the catalog
        public GameObject unlockedUIObject;
    }

    [Header("Catalog Setup")]
    public List<CatalogItem> catalogItems = new List<CatalogItem>();

    private void Awake()
    {
        // Setup Singleton
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        // Load the catalog progress when the game starts
        RefreshCatalog();
    }

    // Call this to permanently unlock a planet in the catalog
    public void UnlockPlanet(string planetName)
    {
        // Save the unlocked state using PlayerPrefs (1 means unlocked, 0 means locked)
        string saveKey = "UnlockedPlanet_" + planetName.ToUpper();
        PlayerPrefs.SetInt(saveKey, 1);
        PlayerPrefs.Save();

        // Update the UI immediately without needing to restart
        foreach (CatalogItem item in catalogItems)
        {
            if (item.planetName.ToUpper() == planetName.ToUpper())
            {
                if (item.unlockedUIObject != null)
                {
                    item.unlockedUIObject.SetActive(true);
                }
                Debug.Log($"CatalogPanel: Unlocked '{planetName}' in the catalog.");
                break;
            }
        }
    }

    // Checks PlayerPrefs to see which planets have been unlocked previously
    public void RefreshCatalog()
    {
        foreach (CatalogItem item in catalogItems)
        {
            string saveKey = "UnlockedPlanet_" + item.planetName.ToUpper();

            // Check if the key exists and equals 1. Default to 0 if not found.
            bool isUnlocked = PlayerPrefs.GetInt(saveKey, 0) == 1;

            if (item.unlockedUIObject != null)
            {
                // Activates the image if unlocked, deactivates if it isn't
                item.unlockedUIObject.SetActive(isUnlocked);
            }
        }
    }

    // Method to call when the "Open" button is clicked
    public void ShowPanel()
    {
        if (panelToShow != null)
        {
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
            panelToShow.SetActive(false);
            Debug.Log("Panel successfully closed.");
        }
    }
}