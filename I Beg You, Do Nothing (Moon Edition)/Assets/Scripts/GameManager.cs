using UnityEngine;
using System.Collections; // Required for Coroutines

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

    [Header("Fire Ending Logic")]
    public int pressesToTriggerFireEnding = 2;
    public GameObject fireEndingObject;

    void Start()
    {
        if (endingCanvas != null)
        {
            endingCanvas.SetActive(false);
        }

        if (fireEndingObject != null)
        {
            fireEndingObject.SetActive(false);
        }
    }

    public void CheckButtonPresses(int clickCount)
    {
        Debug.Log("GameManager received the number: " + clickCount);

        if (clickCount > 0 && clickCount % pressesToReversePlanet == 0)
        {
            if (planetAnimator != null)
            {
                planetAnimator.ReverseDirection();
            }
        }

        if (clickCount == pressesToTriggerFireEnding)
        {
            TriggerFireEnding();
        }

        if (clickCount == pressesToTriggerEnding)
        {
            TriggerEnding();
        }
    }

    private void TriggerEnding()
    {
        Debug.Log("GameManager: " + pressesToTriggerEnding + " presses reached! Triggering Ending...");

        if (endingCanvas != null)
        {
            endingCanvas.SetActive(true);
        }

        if (endingAnimator != null)
        {
            endingAnimator.SetTrigger(triggerName);
        }
    }

    private void TriggerFireEnding()
    {
        Debug.Log("GameManager: " + pressesToTriggerFireEnding + " presses reached! Triggering Fire Ending...");

        if (fireEndingObject != null)
        {
            StartCoroutine(FireEndingRoutine());
        }
        else
        {
            Debug.LogWarning("GameManager: Fire ending triggered, but no object is assigned in the Inspector!");
        }
    }

    private IEnumerator FireEndingRoutine()
    {
        // 1. Activate the fire object
        fireEndingObject.SetActive(true);
        Debug.Log("GameManager: Fire Ending activated. Waiting 3 seconds...");

        // --- NEW ADDITION ---
        // 2. Tell the PlanetManager to swap the planets based on what is currently active
        if (PlanetManager.Instance != null)
        {
            PlanetManager.Instance.HandleFireEnding();
        }
        else
        {
            Debug.LogWarning("GameManager: Could not find PlanetManager to swap planets!");
        }

        // 3. Wait for exactly 3 seconds
        yield return new WaitForSeconds(3f);

        // 4. Deactivate the fire object
        fireEndingObject.SetActive(false);
        Debug.Log("GameManager: Fire Ending deactivated.");
    }
}