using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlowingCometRepeat : MonoBehaviour
{
    public List<Vector2> listStarts = new List<Vector2>()
    {
            new Vector2(250, 60),
            new Vector2(240, 30),
            new Vector2(40, 80),
            new Vector2(120, 80)
    };

    public List<Vector2> listEnds = new List<Vector2>()
    {
            new Vector2(-220, -80),
            new Vector2(-70, -80),
            new Vector2(-239, 0),
            new Vector2(-239, -50)
    };

    public float travelTime = 1.2f;
    public float delayBetweenPasses = 20f;

    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();

        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
    }

    void Start()
    {
        StartCoroutine(CometLoop());
    }

    IEnumerator CometLoop()
    {
        while (true)
        {
            // hide while waiting
            canvasGroup.alpha = 0f;

            // wait 20 seconds
            yield return new WaitForSeconds(delayBetweenPasses);

            int randomIndex = Random.Range(0, listStarts.Count);
            Vector2 chosenStart = listStarts[randomIndex];
            Vector2 chosenEnd = listEnds[randomIndex];

            // start above/right of the window
            rectTransform.anchoredPosition = chosenStart;
            canvasGroup.alpha = 1f;

            float t = 0f;
            while (t < 1f)
            {
                t += Time.deltaTime / travelTime;
                rectTransform.anchoredPosition = Vector2.Lerp(chosenStart, chosenEnd, t);
                yield return null;
            }

            // hide again after passing through
            canvasGroup.alpha = 0f;
        }
    }
}