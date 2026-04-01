using UnityEngine;

public class PlanetReverseCounter : MonoBehaviour
{
    public PlanetFrameAnimator planetAnimator;
    public int pressesNeeded = 10;

    private int currentPressCount = 0;

    public void RegisterPress()
    {
        currentPressCount++;
        Debug.Log("Button presses: " + currentPressCount + "/" + pressesNeeded);

        if (currentPressCount >= pressesNeeded)
        {
            currentPressCount = 0;

            if (planetAnimator != null)
            {
                planetAnimator.ReverseDirection();
            }
            else
            {
                Debug.LogWarning("PlanetAnimator is not assigned.");
            }
        }
    }
}