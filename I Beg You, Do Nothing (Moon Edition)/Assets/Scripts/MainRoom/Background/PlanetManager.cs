using UnityEngine;
using System.Collections.Generic;

public class PlanetManager : MonoBehaviour
{
    // Singleton instance so other scripts can easily call methods here
    public static PlanetManager Instance { get; private set; }

    [Header("Planet Database")]
    public List<PlanetData> planets = new List<PlanetData>();

    [Header("Startup Settings")]
    public string defaultPlanetName = "Earth"; // The name of the planet to show on start

    // A custom struct to pair a planet's name with its GameObject in the Inspector
    [System.Serializable]
    public struct PlanetData
    {
        public string planetName;
        public GameObject planetObject;
    }

    // Keep track of the currently active planet and its name
    private GameObject currentActivePlanet = null;
    private string activePlanetName = ""; // NEW: Tracks the exact name of the active planet

    private void Awake()
    {
        // Standard Singleton setup to ensure only one manager exists
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        // Ensure all planets start hidden to clear the scene
        HideAllPlanets();

        // Show the default planet if a name is provided
        if (!string.IsNullOrEmpty(defaultPlanetName))
        {
            ShowPlanet(defaultPlanetName);
        }
    }

    // Call this method to show a specific planet by name
    public void ShowPlanet(string targetName)
    {
        bool planetFound = false;

        foreach (PlanetData data in planets)
        {
            // Convert to uppercase to prevent case-sensitive typos
            if (data.planetName.ToUpper() == targetName.ToUpper())
            {
                // If a different planet is currently active, hide it first
                if (currentActivePlanet != null && currentActivePlanet != data.planetObject)
                {
                    currentActivePlanet.SetActive(false);
                }

                // Turn on the requested planet
                data.planetObject.SetActive(true);
                currentActivePlanet = data.planetObject;
                activePlanetName = data.planetName; // NEW: Save the name of the new planet
                planetFound = true;

                // Unlock this planet in the Catalog
                if (CatalogPanelController.Instance != null)
                {
                    CatalogPanelController.Instance.UnlockPlanet(data.planetName);
                }

                Debug.Log($"PlanetManager: '{targetName}' is now active.");
                break;
            }
        }

        if (!planetFound)
        {
            Debug.LogWarning($"PlanetManager: Planet named '{targetName}' was not found in the database.");
        }
    }

    // Call this method to hide the currently active planet
    public void HideAllPlanets()
    {
        foreach (PlanetData data in planets)
        {
            if (data.planetObject != null)
            {
                data.planetObject.SetActive(false);
            }
        }

        currentActivePlanet = null;
        activePlanetName = "";
        Debug.Log("PlanetManager: All planets have been hidden.");
    }

    // --- NEW METHOD ---
    // GameManager will call this when the Fire Ending triggers
    public void HandleFireEnding()
    {
        // Check if the current planet is the Egg
        if (activePlanetName.ToUpper() == "EGG")
        {
            Debug.Log("PlanetManager: Egg detected during Fire Ending. Switching to Fried Egg.");
            ShowPlanet("Fried Egg");
        }
        else
        {
            Debug.Log("PlanetManager: Standard planet detected during Fire Ending. Switching to Ash.");
            ShowPlanet("Ash");
        }
    }
}