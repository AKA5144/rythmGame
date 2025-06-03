using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class SongInfo : MonoBehaviour
{
    public string path;
    public AudioSource audioSource;
    public Image image;
    public string songName;

    string[] imageExtensions = { ".png", ".jpg", ".jpeg" };
    string[] audioExtensions = { ".mp3", ".ogg", ".wav" };

    void Start()
    {
        LoadResources();
    }

    public void LoadResources()
    {
        string[] parts = path.Split('/');
        string lastValue = parts[parts.Length - 1];
        songName = lastValue;

        string audioPath = GetFileWithExtensions(path, "Song", audioExtensions);
        string imagePath = GetFileWithExtensions(path, "Background", imageExtensions);

        if (audioPath != null)
            LoadAudioClipFromResources(audioPath);
        else
            Debug.LogWarning("Fichier audio non trouvé.");

        if (imagePath != null)
            LoadImageFromResources(imagePath);
        else
            Debug.LogWarning("Fichier image non trouvé.");
    }

    private string GetFileWithExtensions(string folderPath, string baseName, string[] allowedExtensions)
    {
        foreach (var ext in allowedExtensions)
        {
            string fullPath = Path.Combine(folderPath, baseName + ext);
            if (File.Exists(fullPath))
                return fullPath;
        }
        return null;
    }

    private void LoadAudioClipFromResources(string resourcePath)
    {
        audioSource.clip = AssetDatabase.LoadAssetAtPath<AudioClip>(resourcePath);
    }

    private void LoadImageFromResources(string resourcePath)
    {
        image.sprite = AssetDatabase.LoadAssetAtPath<Sprite>(resourcePath);
    }
}
