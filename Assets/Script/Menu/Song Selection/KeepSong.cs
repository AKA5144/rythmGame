using UnityEngine;

public class KeepSong : MonoBehaviour
{
    public static KeepSong instance;
    public AudioSource audioSource;
    public bool MusicPlayed;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        MusicPlayed = false;
    }

    public void SetAudioClip(AudioClip newClip)
    {
        audioSource.clip = newClip;
    }

    public void PlayAudio()
    {
        if (!MusicPlayed)
        {
            audioSource.Play();
            MusicPlayed = true;
        }
    }

    public void StopAudio()
    {
        if (MusicPlayed)
        {
            audioSource.Stop();
            MusicPlayed = false;
        }
    }
    public void PauseAudio()
    {
        audioSource.Pause();
        MusicPlayed = false;
    }
}
