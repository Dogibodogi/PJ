using UnityEngine;

public class WirePointUI : MonoBehaviour
{
    public int wireID;
    public bool isLeftSide;
    public RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }
}