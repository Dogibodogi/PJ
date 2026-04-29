using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(Button))]
[RequireComponent(typeof(Image))]
public class RedButtonAnimator : MonoBehaviour
{
    [Header("Animation Sprites")]
    public Sprite normalSprite;   // The default unpressed state
    public Sprite pressedSprite1; // The halfway pressed state
    public Sprite pressedSprite2; // The fully pressed state

    [Tooltip("How long each frame stays on screen (in seconds)")]
    public float animationSpeed = 0.05f;

    private Image buttonImage;
    private Button buttonComponent;
    private bool isAnimating = false;

    [Header("Main Manager")]
    public GameManager gameManager; // The only manager you need now!

    // Keeps track of the total clicks to send to the GameManager
    private int totalClicks = 0;

    void Start()
    {
        buttonImage = GetComponent<Image>();
        buttonComponent = GetComponent<Button>();

        // Automatically listen for the click event
        buttonComponent.onClick.AddListener(TriggerAnimation);
    }

    private void TriggerAnimation()
    {
        if (!isAnimating)
        {
            // Increase the total click count
            totalClicks++;

            // Send the click count directly to the GameManager
            if (gameManager != null)
            {
                gameManager.CheckButtonPresses(totalClicks);
            }
            else
            {
                Debug.LogWarning("RedButtonAnimator: GameManager is not assigned in the Inspector!");
            }

            StartCoroutine(PlayPressAnimation());
        }
    }

    private IEnumerator PlayPressAnimation()
    {
        isAnimating = true;

        buttonImage.sprite = pressedSprite1;
        yield return new WaitForSeconds(animationSpeed);

        buttonImage.sprite = pressedSprite2;
        yield return new WaitForSeconds(animationSpeed);

        buttonImage.sprite = normalSprite;

        isAnimating = false;
    }
}