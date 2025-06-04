using UnityEngine;
using UnityEngine.UI;

public class PlayerSongManager : MonoBehaviour
{
    public static PlayerSongManager Instance { get; private set; }
    public AudioSource audioSource;
    public Scrollbar scrollbar;
    [SerializeField] SongInfoEditor info;

    public bool isPlaying;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public bool IsPlaying()
    {
        return audioSource.isPlaying;
    }
    private void Start()
    {
        isPlaying = false;
    }

    private void Update()
    {
        if (audioSource.clip == null) return;

        if (!audioSource.isPlaying)
        {
            audioSource.time = scrollbar.value * audioSource.clip.length;
        }
    }

    public void PlaySong()
    {
        if (audioSource.clip == null) return;

        audioSource.time = scrollbar.value * audioSource.clip.length;
        audioSource.Play();
        isPlaying = true;
    }

    public void PauseSong()
    {
        if (isPlaying)
        {
            audioSource.Pause();
            isPlaying = false;
        }
    }

    public void StopSong()
    {
        if (isPlaying)
        {
            audioSource.Stop();
            isPlaying = false;
        }
        scrollbar.value = 0;
    }
}
