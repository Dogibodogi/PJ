// EndingManager.cs
// Attach this to the Ending Canvas GameObject

using System.Collections;
using UnityEngine;

public class EndingManager : MonoBehaviour
{
    [Header("Ending Canvas")]
    public CanvasGroup endingCanvasGroup; // add CanvasGroup to Ending Canvas

    [Header("Timing")]
    public float fadeInDuration = 1.5f;
    public float delayBeforeFade = 0.3f; // small pause for drama

    [Header("Ending Animations")]
    public Animator nuclearExplosionAnimator; // drag NuclearExplotion here

    void Start()
    {
        // Make sure the ending is invisible at the start
        if (endingCanvasGroup != null)
        {
            endingCanvasGroup.alpha = 0f;
            endingCanvasGroup.gameObject.SetActive(false);
        }
    }

    public void TriggerEnding()
    {
        Debug.Log("Ending triggered!");

        // Save the unlocked state to PlayerPrefs so it persists after restarting
        PlayerPrefs.SetInt("NuclearEndingUnlocked", 1);
        PlayerPrefs.Save();

        StartCoroutine(PlayEndingSequence());
    }

    private IEnumerator PlayEndingSequence()
    {
        yield return new WaitForSeconds(delayBeforeFade);

        endingCanvasGroup.gameObject.SetActive(true);

        // Only play the nuclear explosion
        if (nuclearExplosionAnimator != null)
        {
            nuclearExplosionAnimator.enabled = true;
            nuclearExplosionAnimator.Play(0);
        }

        // Fade in
        float elapsed = 0f;
        while (elapsed < fadeInDuration)
        {
            elapsed += Time.deltaTime;
            endingCanvasGroup.alpha = Mathf.Clamp01(elapsed / fadeInDuration);
            yield return null;
        }

        endingCanvasGroup.alpha = 1f;
    }
}