using UnityEngine;
using TMPro;
using System.IO;
using SFB; // UnityStandaloneFileBrowser
#if UNITY_EDITOR
using UnityEditor;
#endif

public class NameInputController : MonoBehaviour
{
    public GameObject inputPanel;
    public TMP_InputField nameInputField;

    private string lastCreatedFolderPath;

    void Start()
    {
        inputPanel.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            inputPanel.SetActive(true);
            nameInputField.text = "";
            nameInputField.ActivateInputField();
        }

        if (inputPanel.activeSelf && (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)))
        {
            string inputText = nameInputField.text.Trim();
            if (string.IsNullOrWhiteSpace(inputText))
            {
                Debug.LogWarning("Le nom du dossier ne peut pas �tre vide !");
                return;
            }

            CreateFolder(inputText);
            inputPanel.SetActive(false);

            OpenImageFileExplorer();
        }
    }

    private void CreateFolder(string folderName)
    {
        lastCreatedFolderPath = Path.Combine("Assets/Maps", folderName);

        if (!Directory.Exists(lastCreatedFolderPath))
        {
            Directory.CreateDirectory(lastCreatedFolderPath);
            Debug.Log($"Dossier cr�� : {lastCreatedFolderPath}");

#if UNITY_EDITOR
            AssetDatabase.Refresh();
#endif
        }
        else
        {
            Debug.LogWarning($"Le dossier existe d�j� : {lastCreatedFolderPath}");
        }
    }

    private void OpenImageFileExplorer()
    {
        var imageExtensions = new[] {
            new ExtensionFilter("Image Files", "png", "jpg", "jpeg", "bmp", "gif")
        };

        string[] paths = StandaloneFileBrowser.OpenFilePanel("Choisir une image", "", imageExtensions, false);

        if (paths.Length > 0)
        {
            string sourcePath = paths[0];
            string extension = Path.GetExtension(sourcePath);
            string destinationPath = Path.Combine(lastCreatedFolderPath, "background" + extension);

            File.Copy(sourcePath, destinationPath, true);
            Debug.Log($"Image copi�e : {destinationPath}");

#if UNITY_EDITOR
            AssetDatabase.Refresh();
#endif
        }
        else
        {
            Debug.Log("Aucune image s�lectionn�e.");
        }

        // Une fois l'image s�lectionn�e (ou annul�e), on passe � l'audio
        OpenAudioFileExplorer();
    }

    private void OpenAudioFileExplorer()
    {
        var audioExtensions = new[] {
            new ExtensionFilter("Audio Files", "mp3", "ogg", "wav", "aiff", "flac")
        };

        string[] paths = StandaloneFileBrowser.OpenFilePanel("Choisir un fichier audio", "", audioExtensions, false);

        if (paths.Length > 0)
        {
            string sourcePath = paths[0];
            string extension = Path.GetExtension(sourcePath);
            string destinationPath = Path.Combine(lastCreatedFolderPath, "Song" + extension);

            File.Copy(sourcePath, destinationPath, true);
            Debug.Log($"Audio copi� : {destinationPath}");
            CreateEmptyInfoFile();
#if UNITY_EDITOR
            AssetDatabase.Refresh();
#endif
        }
        else
        {
            Debug.Log("Aucun audio s�lectionn�.");
        }
    }

    private void CreateEmptyInfoFile()
    {
        string infoPath = Path.Combine(lastCreatedFolderPath, "Info.txt");

        if (!File.Exists(infoPath))
        {
            File.WriteAllText(infoPath, ""); // fichier vide
            Debug.Log("Fichier Info.txt vide cr��.");
        }

#if UNITY_EDITOR
        AssetDatabase.Refresh();
#endif
    }
}
