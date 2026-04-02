using UnityEngine;
using UnityEngine.EventSystems;

public class CoffeeCup : MonoBehaviour, IPointerClickHandler
{
    public int clicksNeeded = 3;
    private int currentClicks = 0;
    private bool triggered = false;

    public GameObject normalCup;
    public GameObject spilledCup;
    public GameObject spillClickArea;
    public GameObject panelCover;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (triggered) return;

        currentClicks++;
        Debug.Log("Coffee clicked: " + currentClicks);

        if (currentClicks >= clicksNeeded)
        {
            TriggerSpill();
        }
    }

    private void TriggerSpill()
    {
        triggered = true;

        if (normalCup != null)
            normalCup.SetActive(false);

        if (spilledCup != null)
            spilledCup.SetActive(true);

        if (spillClickArea != null)
            spillClickArea.SetActive(true);

        if (panelCover != null)
            panelCover.SetActive(false);

        Debug.Log("Coffee spilled!");
    }
}