using UnityEngine;
using UnityEngine.EventSystems;

public class WirePanelOpener : MonoBehaviour, IPointerClickHandler
{
    public GameObject wirePuzzleOverlay;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (wirePuzzleOverlay != null)
            wirePuzzleOverlay.SetActive(true);

        Debug.Log("Opened wire puzzle.");
    }
}