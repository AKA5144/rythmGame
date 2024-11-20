using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GetSongInFile : MonoBehaviour
{
    [SerializeField] private string folderPath = "Assets/Maps";
    [SerializeField] private SongSelectorManager manager;

    private int folderCount = 0;
    void Start()
    {
        if (Directory.Exists(folderPath))
        {
            string[] directories = Directory.GetDirectories(folderPath);
            folderCount = directories.Length;

            foreach (string dir in directories)
            {
                manager.folderPaths.Add(dir.Replace("\\", "/"));
            }
            manager.RefreshSongInfo();
            Debug.Log($"Le chemin '{folderPath}' contient {folderCount} dossier(s).");
            foreach (string path in manager.folderPaths)
            {
                Debug.Log($"Dossier trouvé : {path}");
            }
        }
        else
        {
            Debug.LogError($"Le chemin spécifié '{folderPath}' n'existe pas !");
        }
    }

    public int GetFolderCount()
    {
        return folderCount;
    }
}
