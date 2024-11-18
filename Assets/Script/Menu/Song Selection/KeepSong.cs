using UnityEngine;

public class KeepSong : MonoBehaviour
{
    public static KeepSong instance;
    public AudioSource audioSource;
    public bool MusicPlayed;
    void Awake()
    {
        MusicPlayed = false;
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject);
        }
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
}
