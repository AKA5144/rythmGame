using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SongInfo : MonoBehaviour
{
    public string path;
    public AudioSource audioSource;
    public Image image;

    void Start()
    {
        LoadRessource();
    }
    public void LoadRessource()
    {
        LoadAudioClipFromResources(path + "/Song.mp3");
        LoadImageFromResources(path + "/Background.jpg");
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
