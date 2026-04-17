using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneArrowNavigation : MonoBehaviour
{
    [SerializeField] private string previousScene;
    [SerializeField] private string nextScene;

    public void GoPrevious()
    {
        if (!string.IsNullOrEmpty(previousScene))
            SceneManager.LoadScene(previousScene);
    }

    public void GoNext()
    {
        if (!string.IsNullOrEmpty(nextScene))
            SceneManager.LoadScene(nextScene);
    }
}