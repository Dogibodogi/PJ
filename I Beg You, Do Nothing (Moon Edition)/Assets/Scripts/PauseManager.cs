using UnityEngine;
using UnityEngine.InputSystem; // Adăugăm asta pentru sistemul nou

public class PauseManager : MonoBehaviour
{
    public GameObject pauseMenuPanel;
    private bool isPaused = false;

    void Start()
    {
        pauseMenuPanel.SetActive(false);
    }

    void Update()
    {
        // Așa se scrie "GetKeyDown" în sistemul nou
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

    public void ResumeGame()
    {
        pauseMenuPanel.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }
}