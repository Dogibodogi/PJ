using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

[System.Serializable]
public class StoryStep
{
    [Header("UI Toggles")]
    public bool showText = true;
    public bool useGrayBox = true;

    [Header("Visuals & Animation")]
    public Sprite background;
    public float individualFadeDuration = 0.5f; // Set to 0 if background doesn't change
    public Sprite characterPortrait;

    [Header("Content")]
    public string characterName;
    [TextArea(3, 10)]
    public string dialogueLine;

    [Header("Audio")]
    public AudioClip customTypingSound;
}

public class CutsceneManager : MonoBehaviour
{
    [Header("UI Component References")]
    public Image backgroundImage;
    public CanvasGroup backgroundCanvasGroup;
    public Image characterImage;
    public Image dialogueBoxImage;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;

    [Header("Audio Settings")]
    public AudioSource audioSource;
    public AudioClip defaultTypeSound;
    [Range(0, 0.3f)] public float pitchVariation = 0.1f;

    [Header("General Settings")]
    public float typingSpeed = 0.04f;
    public string nextSceneName;

    [Header("Story Content")]
    public StoryStep[] storySteps;

    private int currentIndex = 0;
    private bool isTyping = false;
    private Coroutine typingCoroutine;
    private Coroutine fadeCoroutine;
    private float lastClickTime = 0f;
    private float clickCooldown = 0.15f;

    void Start()
    {
        if (storySteps.Length > 0) UpdateUI();
    }

    public void OnScreenClick()
    {
        if (Time.time - lastClickTime < clickCooldown) return;
        lastClickTime = Time.time;

        if (!storySteps[currentIndex].showText)
        {
            AdvanceToNextStep();
            return;
        }

        if (isTyping) CompleteTextInstantly();
        else AdvanceToNextStep();
    }

    private void AdvanceToNextStep()
    {
        if (audioSource != null) audioSource.Stop();
        currentIndex++;
        if (currentIndex < storySteps.Length) UpdateUI();
        else FinishCutscene();
    }

    private void UpdateUI()
    {
        StoryStep current = storySteps[currentIndex];

        // 1. Handle Background Fade
        // We only fade if a background is assigned AND the duration is > 0
        if (current.background != null && current.background != backgroundImage.sprite)
        {
            if (current.individualFadeDuration > 0)
            {
                if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
                fadeCoroutine = StartCoroutine(FadeBackground(current.background, current.individualFadeDuration));
            }
            else
            {
                // Instant swap if duration is 0
                backgroundImage.sprite = current.background;
                backgroundCanvasGroup.alpha = 1;
            }
        }

        // 2. Portrait Logic
        if (current.characterPortrait != null)
        {
            characterImage.sprite = current.characterPortrait;
            characterImage.gameObject.SetActive(true);
        }
        else characterImage.gameObject.SetActive(false);

        // 3. Dialogue Box Logic
        dialogueBoxImage.gameObject.SetActive(current.showText && current.useGrayBox);

        if (current.showText)
        {
            nameText.gameObject.SetActive(true);
            dialogueText.gameObject.SetActive(true);
            nameText.text = current.characterName;

            if (typingCoroutine != null) StopCoroutine(typingCoroutine);
            typingCoroutine = StartCoroutine(TypeText(current.dialogueLine, current.customTypingSound));
        }
        else
        {
            nameText.gameObject.SetActive(false);
            dialogueText.gameObject.SetActive(false);
        }
    }

    private IEnumerator FadeBackground(Sprite newNextSprite, float duration)
    {
        float timer = 0;
        // Fade Out
        while (timer < duration)
        {
            timer += Time.deltaTime;
            backgroundCanvasGroup.alpha = Mathf.Lerp(1, 0, timer / duration);
            yield return null;
        }

        backgroundImage.sprite = newNextSprite;

        // Fade In
        timer = 0;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            backgroundCanvasGroup.alpha = Mathf.Lerp(0, 1, timer / duration);
            yield return null;
        }
        backgroundCanvasGroup.alpha = 1;
    }

    [Header("Advanced Audio")]
    [Range(0.01f, 0.2f)] public float minTimeBetweenSounds = 0.06f;
    [Range(0.1f, 0.5f)] public float baseVolume = 0.3f;
    [Range(0f, 0.2f)] public float volumeVariation = 0.05f;

    private float lastSoundPlayTime;

    private IEnumerator TypeText(string text, AudioClip customSound)
    {
        isTyping = true;
        dialogueText.text = "";
        AudioClip soundToPlay = customSound != null ? customSound : defaultTypeSound;

        foreach (char letter in text.ToCharArray())
        {
            dialogueText.text += letter;

            if (soundToPlay != null && !char.IsWhiteSpace(letter) && Time.time - lastSoundPlayTime > minTimeBetweenSounds)
            {
                audioSource.pitch = Random.Range(1f - pitchVariation, 1f + pitchVariation);
                float randomVol = Random.Range(baseVolume - volumeVariation, baseVolume + volumeVariation);
                audioSource.PlayOneShot(soundToPlay, randomVol);
                lastSoundPlayTime = Time.time;
            }
            yield return new WaitForSeconds(typingSpeed);
        }
        isTyping = false;
        audioSource.pitch = 1f;
    }

    private void CompleteTextInstantly()
    {
        if (typingCoroutine != null) StopCoroutine(typingCoroutine);
        dialogueText.text = storySteps[currentIndex].dialogueLine;
        isTyping = false;
    }

    private void FinishCutscene()
    {
        if (audioSource != null) audioSource.Stop();
        dialogueBoxImage.gameObject.SetActive(false);
        nameText.gameObject.SetActive(false);
        dialogueText.gameObject.SetActive(false);
        if (!string.IsNullOrEmpty(nextSceneName)) SceneManager.LoadScene(nextSceneName);
    }
}