using UnityEngine;
using UnityEngine.UI;

public class PlanetFrameAnimator : MonoBehaviour
{
    public Image targetImage;
    public Sprite[] frames;
    public float framesPerSecond = 10f;
    public bool playOnStart = true;

    private int currentFrame = 0;
    private float timer = 0f;
    private int direction = 1; // 1 = forward, -1 = backward
    private bool isPlaying;

    void Awake()
    {
        if (targetImage == null)
            targetImage = GetComponent<Image>();
    }

    void Start()
    {
        isPlaying = playOnStart;

        if (targetImage != null && frames != null && frames.Length > 0)
        {
            targetImage.sprite = frames[currentFrame];
        }
    }

    void Update()
    {
        if (!isPlaying || targetImage == null || frames == null || frames.Length == 0)
            return;

        timer += Time.deltaTime;
        float frameTime = 1f / framesPerSecond;

        while (timer >= frameTime)
        {
            timer -= frameTime;

            currentFrame += direction;

            if (currentFrame >= frames.Length)
                currentFrame = 0;

            if (currentFrame < 0)
                currentFrame = frames.Length - 1;

            targetImage.sprite = frames[currentFrame];
        }
    }

    public void ReverseDirection()
    {
        direction *= -1;

        if (direction == 1)
            Debug.Log("Planet direction changed. New direction: forward");
        else
            Debug.Log("Planet direction changed. New direction: backward");
    }

    public void SetForward()
    {
        direction = 1;
    }

    public void SetBackward()
    {
        direction = -1;
    }

    public void StopAnimation()
    {
        isPlaying = false;
    }

    public void StartAnimation()
    {
        isPlaying = true;
    }
}