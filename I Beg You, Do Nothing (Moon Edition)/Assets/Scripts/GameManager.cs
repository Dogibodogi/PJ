using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Planet Logic")]
    public int pressesToReversePlanet = 10;
    public PlanetFrameAnimator planetAnimator;

    [Header("Ending Logic")]
    public int pressesToTriggerEnding = 30;
    public GameObject endingCanvas;
    public Animator endingAnimator;
    public string triggerName = "PlayEnding";

    void Start()
    {
        if (endingCanvas != null)
        {
            endingCanvas.SetActive(false);
        }
    }

    // The button will call this method and pass its click count to it
    public void CheckButtonPresses(int clickCount)
    {
        // Add this line right here!
        Debug.Log("GameManager received the number: " + clickCount);

        // 1. Check if we should reverse the planet
        if (clickCount % pressesToReversePlanet == 0)
        {
            if (planetAnimator != null)
            {
                planetAnimator.ReverseDirection();
            }
        }

        // 2. Check if we should trigger the ending
        if (clickCount == pressesToTriggerEnding)
        {
            TriggerEnding();
        }
    }

    private void TriggerEnding()
    {
        Debug.Log("GameManager: 30 presses reached! Triggering Ending...");

        if (endingCanvas != null)
        {
            endingCanvas.SetActive(true);
        }

        if (endingAnimator != null)
        {
            endingAnimator.SetTrigger(triggerName);
        }
    }
}