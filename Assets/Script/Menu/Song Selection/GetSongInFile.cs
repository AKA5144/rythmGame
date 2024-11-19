using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GetSongInFile : MonoBehaviour
{
    [SerializeField] private string folderPath = "Assets/Maps";

    private int folderCount = 0;
    private List<string> folderPaths = new List<string>();

    void Start()
    {
        if (Directory.Exists(folderPath))
        {
            string[] directories = Directory.GetDirectories(folderPath);
            folderCount = directories.Length;

            foreach (string dir in directories)
            {
                folderPaths.Add(dir.Replace("\\", "/"));
            }

            Debug.Log($"Le chemin '{folderPath}' contient {folderCount} dossier(s).");
            foreach (string path in folderPaths)
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
    public List<string> GetFolderPaths()
    {
        return folderPaths;
    }
}
