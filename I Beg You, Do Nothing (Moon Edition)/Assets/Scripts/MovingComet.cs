using UnityEngine;

public class GlowingCometMove : MonoBehaviour
{
    public Vector2 direction = new Vector2(-1f, -0.4f);
    public float speed = 250f;

    private RectTransform rectTransform;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        rectTransform.anchoredPosition += direction.normalized * speed * Time.deltaTime;
    }
}