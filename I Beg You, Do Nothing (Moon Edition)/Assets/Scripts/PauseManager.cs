using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement; // Required for loading scenes

public class PauseManager : MonoBehaviour
{
    public GameObject pauseMenuPanel;
    private bool isPaused = false;

    // You can set the exact name of your main menu scene in the inspector
    [Tooltip("The exact name of the main menu scene as written in the Build Settings")]
    public string mainMenuSceneName = "MainMenu";

    void Start()
    {
        pauseMenuPanel.SetActive(false);
    }

    void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    public void PauseGame()
    {
        pauseMenuPanel.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    // Link this to your "Continue" button
    public void ResumeGame()
    {
        pauseMenuPanel.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    // Link this to your "Restart" button
    public void RestartGame()
    {
        // Always reset time scale before loading a scene
        Time.timeScale = 1f;

        // Reloads the currently active scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Link this to your "Main Menu" button
    public void GoToMainMenu()
    {
        // Always reset time scale before loading a scene
        Time.timeScale = 1f;

        // Load the Main Menu scene
        SceneManager.LoadScene(mainMenuSceneName);
    }
    public void SaveGame()
    {
        // 1. Save the current scene name
        string currentScene = SceneManager.GetActiveScene().name;
        PlayerPrefs.SetString("SavedLevel", currentScene);

        // (Optional) You can save other things here later, for example:
        // PlayerPrefs.SetInt("HasUVLight", playerHasUVLight ? 1 : 0);

        // 2. Actually write the data to the hard drive
        PlayerPrefs.Save();

        Debug.Log("Game Saved! Scene: " + currentScene);
    }
}