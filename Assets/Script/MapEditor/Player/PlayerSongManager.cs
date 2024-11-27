using UnityEngine;

public class PlayerSongManager : MonoBehaviour
{
    public AudioSource audioSource;
    [SerializeField] SongInfoEditor info;
    public bool isPlaying;
    private void Start()
    {
        isPlaying = false;
    }
    public void PlaySong()
    {
        if (!isPlaying)
        {
            audioSource.time = info.timeCode;
            audioSource.Play();
            isPlaying = true;
        }
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
    }
}
