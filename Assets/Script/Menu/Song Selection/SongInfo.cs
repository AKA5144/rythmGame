using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

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

    public void ReloadSong()
    {
        LoadResources();
    }

    public void LoadResources()
    {
        string[] parts = path.Split('/');
        string lastValue = parts[parts.Length - 2]; // nom du dossier
        songName = lastValue;

        string audioPath = GetFileWithExtensions(path, "Song", audioExtensions);
        string imagePath = GetFileWithExtensions(path, "Background", imageExtensions);

        if (audioPath != null)
            LoadAudioFromFile(audioPath);
        else
            Debug.LogWarning("Fichier audio non trouvé.");

        if (imagePath != null)
            LoadImageFromFile(imagePath);
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

    private void LoadAudioFromFile(string fullPath)
    {
        string url = "file://" + fullPath;
        using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(url, AudioType.UNKNOWN))
        {
            var operation = www.SendWebRequest();
            while (!operation.isDone) { } // <-- attend (⚠ blocant)

#if UNITY_2020_1_OR_NEWER
            if (www.result != UnityWebRequest.Result.Success)
#else
            if (www.isNetworkError || www.isHttpError)
#endif
            {
                Debug.LogError($"Erreur lors du chargement de l'audio : {www.error}");
            }
            else
            {
                AudioClip clip = DownloadHandlerAudioClip.GetContent(www);
                audioSource.clip = clip;
            
            }
        }
    }

    private void LoadImageFromFile(string fullPath)
    {
        byte[] fileData = File.ReadAllBytes(fullPath);
        Texture2D tex = new Texture2D(2, 2);
        tex.LoadImage(fileData);

        Sprite loadedSprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
        image.sprite = loadedSprite;
    }
}
