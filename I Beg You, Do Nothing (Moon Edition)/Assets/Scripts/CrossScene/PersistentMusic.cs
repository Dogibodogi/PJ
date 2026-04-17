using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
public class PersistentMusic : MonoBehaviour
{
    private static PersistentMusic instance;

    [SerializeField] private AudioClip[] playlist;
    [SerializeField] private bool loopPlaylist = true;

    [Header("Persist only in these scenes")]
    [SerializeField] private string[] allowedScenes;

    private AudioSource audioSource;
    private int currentIndex = 0;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        audioSource = GetComponent<AudioSource>();
        audioSource.loop = false;
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Start()
    {
        if (playlist != null && playlist.Length > 0)
        {
            PlayCurrent();
        }
    }

    private void Update()
    {
        if (playlist == null || playlist.Length == 0) 
            return;

        if (audioSource.isPlaying) 
            return;

        currentIndex++;

        if (currentIndex >= playlist.Length)
        {
            if (loopPlaylist)
                currentIndex = 0;
            else
                return;
        }

        PlayCurrent();
    }

    private void PlayCurrent()
    {
        audioSource.clip = playlist[currentIndex];
        audioSource.Play();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        bool allowed = false;

        for (int i = 0; i < allowedScenes.Length; i++)
        {
            if (scene.name == allowedScenes[i])
            {
                allowed = true;
                break;
            }
        }

        if (!allowed)
        {
            Destroy(gameObject);
        }
    }
}