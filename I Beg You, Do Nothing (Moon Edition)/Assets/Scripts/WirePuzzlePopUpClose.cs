using UnityEngine;
using UnityEngine.EventSystems;

public class PopupOverlayClose : MonoBehaviour, IPointerClickHandler
{
    public GameObject popupToClose;
    public RectTransform puzzleWindow;
    public WirePuzzleUI wirePuzzleUI;

    public void OnPointerClick(PointerEventData eventData)
    {
        bool clickedInsideWindow = RectTransformUtility.RectangleContainsScreenPoint(
            puzzleWindow,
            eventData.position,
            eventData.pressEventCamera
        );

        if (clickedInsideWindow)
            return;

        if (wirePuzzleUI != null)
            wirePuzzleUI.ResetPuzzle();

        if (popupToClose != null)
            popupToClose.SetActive(false);
    }
}