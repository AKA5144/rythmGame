using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

public class ScrollbarManager : MonoBehaviour
{
    public Scrollbar scrollbar;
    public PlayerSongManager player;
    public SongInfoEditor info;
    public TextMeshProUGUI text;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if (player.isPlaying)
        {
            text.text = info.percent.ToString();
            scrollbar.value = info.percent * 0.01f;
        }
        else
        {
            info.setProgress(scrollbar.value * 100);
            text.text = new string(scrollbar.value * 100 + "%");
        }
    }
}
