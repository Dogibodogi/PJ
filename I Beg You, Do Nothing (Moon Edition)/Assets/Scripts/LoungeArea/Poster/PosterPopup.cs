using UnityEngine;

public class PosterPopup : MonoBehaviour
{
    [SerializeField] private GameObject actualPoster;

    private void Start()
    {
        if (actualPoster != null)
            actualPoster.SetActive(false);
    }

    public void ShowPoster()
    {
        if (actualPoster != null)
            actualPoster.SetActive(true);
    }

    public void HidePoster()
    {
        if (actualPoster != null)
            actualPoster.SetActive(false);
    }
}
