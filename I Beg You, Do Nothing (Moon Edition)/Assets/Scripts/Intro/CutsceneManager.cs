using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement; // Useful for loading the next scene

[System.Serializable]
public class StoryStep
{
    [Header("Visuals")]
    public Sprite background;       // Optional: Leave empty to keep the previous background
    public Sprite characterPortrait; // Optional: Leave empty to hide character

    [Header("Text")]
    public string characterName;    // The name shown in the small name box
    [TextArea(3, 10)]
    public string dialogueLine;     // The actual text the character says
}

public class CutsceneManager : MonoBehaviour
{
    [Header("UI Component References")]
    public Image backgroundImage;
    public Image characterImage;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;
    public GameObject dialogueBoxParent; // The gray box containing the text

    [Header("Settings")]
    public float typingSpeed = 0.04f;
    public string nextSceneName;        // Name of the scene to load when finished

    [Header("Story Content")]
    public StoryStep[] storySteps;

    private int currentIndex = 0;
    private bool isTyping = false;
    private Coroutine typingCoroutine;

    void Start()
    {
        // Ensure the UI is visible and start the first step
        if (storySteps.Length > 0)
        {
            UpdateUI();
        }
        else
        {
            Debug.LogError("No Story Steps found in the CutsceneManager inspector!");
        }
    }

    // Call this function from your full-screen Button's OnClick() event
    public void OnScreenClick()
    {
        if (isTyping)
        {
            // 1. If text is still typing, finish it instantly
            CompleteTextInstantly();
        }
        else
        {
            // 2. If text is done, move to the next step
            AdvanceToNextStep();
        }
    }

    private void AdvanceToNextStep()
    {
        currentIndex++;

        if (currentIndex < storySteps.Length)
        {
            UpdateUI();
        }
        else
        {
            FinishCutscene();
        }
    }

    private void UpdateUI()
    {
        StoryStep current = storySteps[currentIndex];

        // Background Logic: Only change if a new sprite is provided
        if (current.background != null)
        {
            backgroundImage.sprite = current.background;
        }

        // Character Logic: Show portrait if assigned, otherwise hide it
        if (current.characterPortrait != null)
        {
            characterImage.sprite = current.characterPortrait;
            characterImage.gameObject.SetActive(true);
        }
        else
        {
            characterImage.gameObject.SetActive(false);
        }

        // Name Logic
        if (nameText != null)
        {
            nameText.text = current.characterName;
        }

        // Start the Typewriter effect
        if (typingCoroutine != null) StopCoroutine(typingCoroutine);
        typingCoroutine = StartCoroutine(TypeText(current.dialogueLine));
    }

    private IEnumerator TypeText(string text)
    {
        isTyping = true;
        dialogueText.text = "";

        foreach (char letter in text.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
    }

    private void CompleteTextInstantly()
    {
        StopCoroutine(typingCoroutine);
        dialogueText.text = storySteps[currentIndex].dialogueLine;
        isTyping = false;
    }

    private void FinishCutscene()
    {
        Debug.Log("Cutscene finished. Loading next scene...");

        // Hide the UI before transitioning
        if (dialogueBoxParent != null) dialogueBoxParent.SetActive(false);

        // Change scene if a name is provided
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }
    }
}