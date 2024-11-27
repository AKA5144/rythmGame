using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SongInfoEditor : MonoBehaviour
{
    public string SongName;
    public AudioSource AudioSource;
    public PlayerSongManager player;
    public Sprite Background;
    public float clipLength;
    public float ClipTimeCode;
    public float timeCode;
    public float progress;
    public float percent;

    public float songProgress;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        clipLength = AudioSource.clip.length;
    }

    // Update is called once per frame
    void Update()
    {
        if (!player.isPlaying)
        {
            convertPercentToTimeCode(progress);
        }
        else
        {
            timeCode = AudioSource.time;
            percent = (timeCode / clipLength) * 100f;
        }


    }

    public void convertPercentToTimeCode(float percent)
    {
        timeCode = AudioSource.clip.length * (percent * 0.01f);
    }

    public void setProgress(float value)
    {
        progress = value;
    }
}
