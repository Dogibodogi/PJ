using UnityEngine;
using UnityEngine.UI;

public class MoonFrameAnimator : MonoBehaviour
{
    public Image targetImage;
    public Sprite[] frames;
    public float framesPerSecond = 10f;
    public bool playOnStart = true;

    private int currentFrame = 0;
    private float timer = 0f;
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

            currentFrame += 1;

            if (currentFrame >= frames.Length)
                currentFrame = 0;

            targetImage.sprite = frames[currentFrame];
        }
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