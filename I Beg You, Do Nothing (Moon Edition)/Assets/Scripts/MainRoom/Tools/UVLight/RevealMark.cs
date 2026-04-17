using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class UVRevealMark : MonoBehaviour
{
    public bool isFingerprint = false;
    public int digitValue;

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
        SetAlpha(hiddenAlpha);
    }

    void Update()
    {
        if (uvLight == null)
        {
            uvLight = FindFirstObjectByType<UVLightTool>();
            if (uvLight == null) return;
        }

        float targetAlpha = hiddenAlpha;

        if (uvLight.isUVLightOn)
        {
            float distance = Vector2.Distance(rectTransform.position, uvLight.GetSpotWorldPosition());

            if (distance <= uvLight.revealRadius + extraRevealRadius)
            {
                targetAlpha = revealedAlpha;
            }
        }

        Color c = markImage.color;
        c.a = Mathf.Lerp(c.a, targetAlpha, Time.deltaTime * fadeSpeed);
        markImage.color = c;
    }

    private void SetAlpha(float a)
    {
        Color c = markImage.color;
        c.a = a;
        markImage.color = c;
    }
}