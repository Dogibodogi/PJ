using UnityEngine;

public class MoonOrbitUI : MonoBehaviour
{
    public RectTransform orbitCenter;
    public float orbitRadius = 80f;
    public float orbitSpeed = 50f;

    private RectTransform rectTransform;
    private float angle;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        if (orbitCenter == null) return;

        angle += orbitSpeed * Time.deltaTime;
        float rad = angle * Mathf.Deg2Rad;

        Vector2 offset = new Vector2(
            Mathf.Cos(rad) * orbitRadius,
            Mathf.Sin(rad) * orbitRadius
        );

        rectTransform.anchoredPosition = orbitCenter.anchoredPosition + offset;
    }
}