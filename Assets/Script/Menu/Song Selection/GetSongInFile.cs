using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GetSongInFile : MonoBehaviour
{
    [SerializeField] private string folderPath = "/Maps"; // <-- chemin corrigé
    [SerializeField] private SongSelectorManager manager;

    private int folderCount = 0;

    void Awake()
    {
        GetSong();
    }

    public void GetSong()
    {
        if (Directory.Exists(Application.persistentDataPath + folderPath))
        {
            string[] directories = Directory.GetDirectories(Application.persistentDataPath + folderPath);
            folderCount = directories.Length;
            foreach (string dir in directories)
            {
                manager.folderPaths.Add(dir.Replace("\\", "/") + "/");
            }
        }
        else
        {
            Debug.LogError($"Le chemin spécifié '{folderPath}' n'existe pas !");
        }
        manager.RefreshSongInfo();
    }

    public int GetFolderCount()
    {
        return folderCount;
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.F5))
        {
            GetSong();
        }
    }
}