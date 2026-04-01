using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // This will be called by your "Play" button
    public void PlayGame()
    {
        // Loads your main gameplay scene
        SceneManager.LoadScene("SampleScene");
    }

    // This will be called by your "Quit" button
    public void QuitGame()
    {
        Debug.Log("The player has quit the game.");
        Application.Quit();
    }
}