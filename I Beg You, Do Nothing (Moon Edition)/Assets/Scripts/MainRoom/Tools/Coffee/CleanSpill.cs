using UnityEngine;
using UnityEngine.EventSystems;

public class CleanSpill : MonoBehaviour, IPointerClickHandler // (Change to OnMouseDown if you used the failsafe earlier)
{
    public GameObject wirePanelClickArea;

    private bool cleaned = false;

    private void Start()
    {
        if (PuzzleState.wirePuzzleSolved)
        {
            if (wirePanelClickArea != null)
                wirePanelClickArea.SetActive(false);

            gameObject.SetActive(false);
            return;
        }

        if (PuzzleState.spillCleaned)
        {
            if (wirePanelClickArea != null)
                wirePanelClickArea.SetActive(true);

            gameObject.SetActive(false);
        }
    }

    // This runs automatically the moment the Spilled Coffee appears on the desk
    private void OnEnable()
    {
        Debug.Log("READY: The spill can now be cleaned! Click the coffee puddle.");
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (cleaned) 
            return;

        cleaned = true;
        PuzzleState.spillCleaned = true;

        if (wirePanelClickArea != null)
            wirePanelClickArea.SetActive(true);

        Debug.Log("Spill cleaned! You can now click the wires.");
        gameObject.SetActive(false);
    }
}