using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class UVRevealMark : MonoBehaviour
{
    public float extraRevealRadius = 0f;
    public float hiddenAlpha = 0f;
    public float revealedAlpha = 1f;
    public float fadeSpeed = 8f;

    private Image markImage;
    private RectTransform rectTransform;
    private UVLightTool uvLight;

    void Start()
    {
        markImage = GetComponent<Image>();
        rectTransform = GetComponent<RectTransform>();
        uvLight = FindObjectOfType<UVLightTool>();   // finding the flashlight script in the scene

        SetAlpha(hiddenAlpha);   // hide the mark immediately
    }

    void Update()
    {
        if (uvLight == null)
            return;

        float targetAlpha = hiddenAlpha;

        if (uvLight.isUVLightOn)
        {
            float distance = Vector2.Distance(rectTransform.position, uvLight.GetSpotWorldPosition());   // distance between mark and UV spot

            if (distance <= uvLight.revealRadius + extraRevealRadius)
            {
                targetAlpha = revealedAlpha;
            }
        }

        Color c = markImage.color;   // copy the color of the mark into variable c
        c.a = Mathf.Lerp(c.a, targetAlpha, Time.deltaTime * fadeSpeed);   // fading from visible to invisible (or reversed)
        markImage.color = c;
    }

    private void SetAlpha(float a)
    {
        Color c = markImage.color;
        c.a = a;
        markImage.color = c;
    }
}