using UnityEngine;

public class SongInfoEditor : MonoBehaviour
{
    public AudioSource AudioSource;
    public PlayerSongManager player;
    public float clipLength;
    public float timeCode;
    public float progress;
    public float percent;

    void Start()
    {
        clipLength = AudioSource.clip.length;
    }

    void Update()
    {
        if (player.isPlaying)
        {
            timeCode = AudioSource.time;
            percent = (timeCode / clipLength) * 100f;
        }
    }

    public void setProgress(float value)
    {
        progress = value;
    }
}
