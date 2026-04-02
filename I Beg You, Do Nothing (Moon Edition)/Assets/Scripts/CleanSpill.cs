using UnityEngine;
using UnityEngine.EventSystems;

public class CleanSpill : MonoBehaviour, IPointerClickHandler
{
    public GameObject spilledCup;
    public GameObject spillClickArea;
    public GameObject wirePanelClickArea;

    private bool cleaned = false;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (cleaned) return;

        cleaned = true;

        if (spilledCup != null)
            spilledCup.SetActive(false);

        if (spillClickArea != null)
            spillClickArea.SetActive(false);

        if (wirePanelClickArea != null)
            wirePanelClickArea.SetActive(true);

        Debug.Log("Spill cleaned!");
    }
}