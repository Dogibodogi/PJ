using UnityEngine;

public class NuclearEndingCounter : MonoBehaviour
{
    [Tooltip("The GameObject containing the Nuclear Explosion Image and Animator")]
    public GameObject nuclearExplosionObject;

    public int pressesNeeded = 30;

    private int currentPressCount = 0;
    private bool hasExploded = false;

    public void RegisterPress()
    {
        // Don't keep counting if the ending is already triggered
        if (hasExploded) return;

        currentPressCount++;
        Debug.Log("Nuclear Ending Progress: " + currentPressCount + "/" + pressesNeeded);

        if (currentPressCount >= pressesNeeded)
        {
            TriggerEnding();
        }
    }

    private void TriggerEnding()
    {
        hasExploded = true;

        if (nuclearExplosionObject != null)
        {
            // Enabling the GameObject will automatically play the default state in its Animator
            nuclearExplosionObject.SetActive(true);
            Debug.Log("Nuclear Explosion Triggered!");
        }
        else
        {
            Debug.LogWarning("Nuclear Explosion Object is not assigned in the inspector.");
        }
    }
}