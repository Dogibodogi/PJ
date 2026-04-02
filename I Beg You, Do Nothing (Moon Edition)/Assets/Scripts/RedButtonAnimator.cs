using UnityEngine;
using UnityEngine.UI; // Required for UI elements like Image and Button
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

    public PlanetReverseCounter reverseCounter;

    [Header("Endings")]
    public NuclearEndingCounter nuclearCounter; // NEW: Reference to the nuclear counter

    void Start()
    {
        // Grab the Image and Button components from this GameObject
        buttonImage = GetComponent<Image>();
        buttonComponent = GetComponent<Button>();

        // Automatically listen for the click event so you don't have to wire it manually!
        buttonComponent.onClick.AddListener(TriggerAnimation);
    }

    private void TriggerAnimation()
    {
        if (!isAnimating)
        {
            if (reverseCounter != null)
            {
                reverseCounter.RegisterPress();
            }

            // NEW: Register the press for the Nuclear Ending
            if (nuclearCounter != null)
            {
                nuclearCounter.RegisterPress();
            }

            StartCoroutine(PlayPressAnimation());
        }
    }

    private IEnumerator PlayPressAnimation()
    {
        isAnimating = true;

        // Frame 1: Pushing down
        buttonImage.sprite = pressedSprite1;
        yield return new WaitForSeconds(animationSpeed);

        // Frame 2: Fully pressed
        buttonImage.sprite = pressedSprite2;
        yield return new WaitForSeconds(animationSpeed);

        // Frame 3: Back to normal
        buttonImage.sprite = normalSprite;

        isAnimating = false;
    }
}