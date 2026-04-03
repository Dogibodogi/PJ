using UnityEngine;
using UnityEngine.EventSystems;

public class WireLeftPointHandler : MonoBehaviour, IPointerDownHandler
{
    public WirePointUI wirePoint;
    public WirePuzzleUI puzzle;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (wirePoint != null)
        {
            Debug.Log(">>> LEFT Point Clicked! ID: " + wirePoint.wireID);
        }
        else
        {
            Debug.LogWarning("Left point clicked, but WirePointUI is missing in the inspector!");
        }

        puzzle.StartWire(wirePoint);
    }
}