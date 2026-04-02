using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // This will be called by your "Play" (or "New Game") button
    public void PlayGame()
    {
        // Loads your main gameplay scene
        SceneManager.LoadScene("SampleScene");
    }

    // Link this to your new "Load" button on the Main Menu
    public void LoadGame()
    {
        // Check if a save file actually exists in PlayerPrefs
        if (PlayerPrefs.HasKey("SavedLevel"))
        {
            // Get the saved scene name
            string sceneToLoad = PlayerPrefs.GetString("SavedLevel");

            // Load the saved scene
            SceneManager.LoadScene(sceneToLoad);

            Debug.Log("Game Loaded! Scene: " + sceneToLoad);
        }
        else
        {
            Debug.LogWarning("No save data found!");
            // Optional: You could show a little UI text on the main menu saying "No save file exists!"
        }
    }

    // This will be called by your "Quit" button
    public void QuitGame()
    {
        Debug.Log("The player has quit the game.");
        Application.Quit();
    }
}