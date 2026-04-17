using UnityEngine;

public class DropScreen : MonoBehaviour
{
    public float targetY;
    public float speed = 300f;

    private bool shouldDrop = false;
    private RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void Drop()
    {
        shouldDrop = true;
    }

    void Update()
    {
        if (!shouldDrop) return;

        Vector2 pos = rectTransform.anchoredPosition;
        pos.y -= speed * Time.deltaTime;

        if (pos.y <= targetY)
        {
            pos.y = targetY;
            shouldDrop = false;
        }

        rectTransform.anchoredPosition = pos;
    }
}