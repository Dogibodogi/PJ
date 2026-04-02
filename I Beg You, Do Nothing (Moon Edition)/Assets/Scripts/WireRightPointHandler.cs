using UnityEngine;
using UnityEngine.EventSystems;

public class WireRightPointHandler : MonoBehaviour, IPointerClickHandler
{
    public WirePointUI wirePoint;
    public WirePuzzleUI puzzle;

    public void OnPointerClick(PointerEventData eventData)
    {
        puzzle.EndWire(wirePoint);
    }
}