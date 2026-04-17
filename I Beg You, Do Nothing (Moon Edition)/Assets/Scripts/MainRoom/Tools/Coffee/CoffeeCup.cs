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

    // we changed this from panelCover to brokenWires
    public GameObject brokenWires;

    private void Start()
    {
        ApplyState();
    }

    private void ApplyState()
    {
        if (PuzzleState.wirePuzzleSolved)
        {
            triggered = true;

            if (normalCup != null) 
                normalCup.SetActive(false);
            if (spilledCup != null) 
                spilledCup.SetActive(false);
            if (spillClickArea != null) 
                spillClickArea.SetActive(false);
            if (brokenWires != null) 
                brokenWires.SetActive(true);

            return;
        }

        if (PuzzleState.spillCleaned)
        {
            triggered = true;

            if (normalCup != null) 
                normalCup.SetActive(false);
            if (spilledCup != null) 
                spilledCup.SetActive(false);
            if (spillClickArea != null) 
                spillClickArea.SetActive(false);
            if (brokenWires != null) 
                brokenWires.SetActive(true);

            return;
        }

        if (PuzzleState.coffeeSpilled)
        {
            triggered = true;

            if (normalCup != null) normalCup.SetActive(false);
            if (spilledCup != null) spilledCup.SetActive(true);
            if (spillClickArea != null) spillClickArea.SetActive(true);
            if (brokenWires != null) brokenWires.SetActive(true);
        }
    }

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
        PuzzleState.coffeeSpilled = true;

        if (normalCup != null)
            normalCup.SetActive(false);

        if (spilledCup != null)
            spilledCup.SetActive(true);

        if (spillClickArea != null)
            spillClickArea.SetActive(true);

        // this now activates the wires instead of hiding a panel
        if (brokenWires != null)
            brokenWires.SetActive(true);

        Debug.Log("Coffee spilled and wires revealed!");
    }
}