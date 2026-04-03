using UnityEngine;
using UnityEngine.EventSystems;

public class WireRightPointHandler : MonoBehaviour, IPointerClickHandler
{
    public WirePointUI wirePoint;
    public WirePuzzleUI puzzle;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (wirePoint != null)
        {
            Debug.Log(">>> RIGHT Point Clicked! ID: " + wirePoint.wireID);
        }
        else
        {
            Debug.LogWarning("Right point clicked, but WirePointUI is missing in the inspector!");
        }

        puzzle.EndWire(wirePoint);
    }
}