using UnityEngine;

public class SongInfo : MonoBehaviour
{
    [SerializeField] private string path = "Maps/Slider/BW2BGM"; // Le chemin relatif � partir du dossier Resources (sans l'extension .mp3)
    [SerializeField] private AudioSource audioSource; // R�f�rence � l'AudioSource

    void Start()
    {
        // Charge le clip audio au d�marrage
        LoadAudioClipFromResources(path);
    }

    private void LoadAudioClipFromResources(string resourcePath)
    {
        // Charger le clip audio depuis le dossier Resources
        AudioClip clip = Resources.Load<AudioClip>(resourcePath);

        // V�rifier si le clip a �t� charg� avec succ�s
        if (clip != null)
        {
            // Assigner le clip � l'AudioSource
            audioSource.clip = clip;
            audioSource.Play(); // Facultatif : jouer le clip audio
        }
        else
        {
            Debug.LogError("�chec du chargement du clip audio � partir de : " + resourcePath);
        }
    }
}
