using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class SongSelectorManager : MonoBehaviour
{
    public List<string> folderPaths = new List<string>();
    [SerializeField] GameObject Main;
    [SerializeField] GameObject Left;
    [SerializeField] GameObject Right;
    [SerializeField] Image bg;
    int selected;

    [SerializeField] SongInfo MainSongInfo;
    [SerializeField] SongInfo LeftSongInfo;
    [SerializeField] SongInfo RightSongInfo;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        selected = 0;
    }

    // Update is called once per frame
    void Update()
    {
        bg.sprite = MainSongInfo.image.sprite;
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (selected + 1 < folderPaths.Count())
            {
                selected++;
            }
            else
            {
                selected = 0;
            }
            RefreshSongInfo();
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (selected - 1 >= 0)
            {
                selected--;
            }
            else
            {
                selected = folderPaths.Count() - 1;
            }
            RefreshSongInfo();
        }
    }
    public void RefreshSongInfo()
    {
        MainSongInfo.audioSource.Stop();
        MainSongInfo.path = folderPaths[selected];
        if (selected + 1 < folderPaths.Count())
        {
            RightSongInfo.path = folderPaths[selected + 1];
        }
        else
        {
            RightSongInfo.path = folderPaths[0];

        }
        if (selected - 1 >= 0)
        {
            LeftSongInfo.path = folderPaths[selected - 1];
        }
        else
        {
            LeftSongInfo.path = folderPaths[folderPaths.Count() - 1];
        }
        MainSongInfo.LoadRessource();
        LeftSongInfo.LoadRessource();
        RightSongInfo.LoadRessource();
        MainSongInfo.audioSource.time = 2f;
        MainSongInfo.audioSource.Play();
        MainSongInfo.audioSource.loop = true;
        bg.sprite = MainSongInfo.image.sprite;
    }
}
