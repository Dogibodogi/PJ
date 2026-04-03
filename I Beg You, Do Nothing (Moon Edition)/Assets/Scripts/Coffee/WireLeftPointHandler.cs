using UnityEngine;
using UnityEngine.EventSystems;

public class WireLeftPointHandler : MonoBehaviour, IPointerDownHandler
{
    public WirePointUI wirePoint;
    public WirePuzzleUI puzzle;

    public void OnPointerDown(PointerEventData eventData)
    {
        puzzle.StartWire(wirePoint);
    }
}