using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class OnlyTheCupCounts : MonoBehaviour
{
    private void Awake()
    {
        Image img = GetComponent<Image>();
        img.alphaHitTestMinimumThreshold = 0.1f;
    }
}