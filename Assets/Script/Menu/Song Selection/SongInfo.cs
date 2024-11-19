using UnityEngine;

public class SongInfo : MonoBehaviour
{
    [SerializeField] private string path = "Maps/Slider/BW2BGM"; // Le chemin relatif à partir du dossier Resources (sans l'extension .mp3)
    [SerializeField] private AudioSource audioSource; // Référence à l'AudioSource

    void Start()
    {
        // Charge le clip audio au démarrage
        LoadAudioClipFromResources(path);
    }

    private void LoadAudioClipFromResources(string resourcePath)
    {
        // Charger le clip audio depuis le dossier Resources
        AudioClip clip = Resources.Load<AudioClip>(resourcePath);

        // Vérifier si le clip a été chargé avec succès
        if (clip != null)
        {
            // Assigner le clip à l'AudioSource
            audioSource.clip = clip;
            audioSource.Play(); // Facultatif : jouer le clip audio
        }
        else
        {
            Debug.LogError("Échec du chargement du clip audio à partir de : " + resourcePath);
        }
    }
}
