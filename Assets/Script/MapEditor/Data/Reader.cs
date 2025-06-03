using UnityEngine;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.UI;


#if UNITY_EDITOR
using UnityEditor;
#endif

public class MapFolderViewer : MonoBehaviour
{
    // Définition d'une structure pour stocker les données lues
    public struct PositionTimeData
    {
        public float positionX;
        public float positionY;
        public float timecode;

        public PositionTimeData(float x, float y, float t)
        {
            positionX = x;
            positionY = y;
            timecode = t;
        }

        public override string ToString()
        {
            return $"PosX: {positionX}, PosY: {positionY}, Timecode: {timecode}";
        }
    }

    public EditorCircleManager editorCircleManager;
    [Header("Nom du dossier dans Assets/Maps")]
    public string folderName;

    [Header("AudioSource cible pour la lecture")]
    public PlayerSongManager songManager;

    [SerializeField] Renderer backgroundPlaneRenderer;

    public void Start()
    {
        Debug.Log(MapReader.mapPath);
        string path = MapReader.mapPath;
        string[] parts = path.Split('/');

        // Récupère le dernier mot non vide
        string lastPart = parts.LastOrDefault(part => !string.IsNullOrWhiteSpace(part));
        folderName = lastPart;
        LoadFirstAudioToSongManager();
        ReadAndStoreDataFromTextFile();
    }
    public void LoadFirstAudioToSongManager()
    {
        string basePath = "Assets/Maps";
        string fullPath = Path.Combine(basePath, folderName);

        if (!Directory.Exists(fullPath))
        {
            Debug.LogError($"Le dossier '{fullPath}' n'existe pas.");
            return;
        }

        string[] audioExtensions = new string[] { ".mp3", ".wav", ".ogg", ".aiff", ".flac" };

        // Trouver le premier fichier audio
        string firstAudioFile = Directory.GetFiles(fullPath)
            .FirstOrDefault(file => audioExtensions.Any(ext => file.EndsWith(ext, System.StringComparison.OrdinalIgnoreCase)));

        if (firstAudioFile == null)
        {
            Debug.LogWarning("Aucun fichier audio trouvé dans le dossier.");
            return;
        }

        Debug.Log($"Chargement du fichier audio : {Path.GetFileName(firstAudioFile)}");

        // Charger l'AudioClip depuis le chemin relatif Assets
        string relativePath = firstAudioFile.Replace(Application.dataPath, "Assets").Replace('\\', '/');
        AudioClip clip = AssetDatabase.LoadAssetAtPath<AudioClip>(relativePath);

        if (clip == null)
        {
            Debug.LogError("Impossible de charger l'AudioClip via AssetDatabase. Vérifiez le format du fichier.");
            return;
        }

        // Assignation au PlayerSongManager
        if (songManager != null)
        {
            if (songManager.audioSource == null)
            {
                Debug.LogError("Le PlayerSongManager n'a pas d'AudioSource assigné.");
                return;
            }

            songManager.audioSource.clip = clip;
            Debug.Log("AudioClip assigné avec succès !");
        }
        else
        {
            Debug.LogError("Référence à PlayerSongManager non assignée.");
        }
        LoadFirstImageToPlane(fullPath);
    }


    // Liste publique pour pouvoir la consulter dans l'inspecteur (optionnel)
    public List<PositionTimeData> positionsData = new List<PositionTimeData>();

    public void ReadAndStoreDataFromTextFile()
    {
        string basePath = "Assets/Maps";
        string fullPath = Path.Combine(basePath, folderName);

        if (!Directory.Exists(fullPath))
        {
            Debug.LogError($"Le dossier '{fullPath}' n'existe pas.");
            return;
        }

        string textFile = Directory.GetFiles(fullPath)
            .FirstOrDefault(file => file.EndsWith(".txt", System.StringComparison.OrdinalIgnoreCase));

        if (textFile == null)
        {
            Debug.LogWarning("Aucun fichier texte trouvé dans le dossier.");
            return;
        }

        positionsData.Clear();

        try
        {
            string[] lines = File.ReadAllLines(textFile);
            foreach (string line in lines)
            {
                // On skip les lignes vides ou nulles
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                // Chaque ligne attendue sous format : positionx,positiony,timecode,
                // On split par la virgule
                string[] parts = line.Split(',');

                if (parts.Length < 3)
                {
                    Debug.LogWarning($"Ligne mal formée ignorée : {line}");
                    continue;
                }

                if (float.TryParse(parts[0], out float posX) &&
                    float.TryParse(parts[1], out float posY) &&
                    float.TryParse(parts[2], out float timecode))
                {
                    positionsData.Add(new PositionTimeData(posX, posY, timecode));
                }
                else
                {
                    Debug.LogWarning($"Impossible de parser les valeurs dans la ligne : {line}");
                }
            }
            if (editorCircleManager != null)
            {
                editorCircleManager.ReceiveData(positionsData);
                Debug.Log("Données envoyées à EditorCircleManager.");
            }
            else
            {
                Debug.LogWarning("Référence EditorCircleManager non assignée.");
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Erreur lors de la lecture du fichier texte : {e.Message}");
        }
    }

    public void SaveCircleDataInTXT()
    {
        if (editorCircleManager == null)
        {
            Debug.LogError("EditorCircleManager non assigné !");
            return;
        }

        var allData = editorCircleManager.GetAllCirclesData();

        if (allData == null || allData.Count == 0)
        {
            Debug.LogWarning("Aucune donnée de cercle à sauvegarder.");
            return;
        }

        string basePath = Path.Combine(Application.dataPath, "Maps", folderName);
        if (!Directory.Exists(basePath))
        {
            Directory.CreateDirectory(basePath);
        }

        string filePath = Path.Combine(basePath, "info.txt");

        try
        {
            using (StreamWriter writer = new StreamWriter(filePath, false))
            {
                foreach (var data in allData)
                {
                    int posXInt = Mathf.RoundToInt(data.position.x);
                    int posYInt = Mathf.RoundToInt(data.position.y);
                    int timeMsInt = Mathf.RoundToInt(data.timeCode * 1000f);
                    string line = $"{posXInt},{posYInt},{timeMsInt}";
                    writer.WriteLine(line);
                }
            }
            Debug.Log($"Données sauvegardées dans {filePath}");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Erreur lors de l'écriture dans le fichier : {e.Message}");
        }
    }

    void LoadFirstImageToPlane(string fullPath)
    {
        string[] imageExtensions = new string[] { ".png", ".jpg", ".jpeg" };
        string firstImageFile = Directory.GetFiles(fullPath)
            .FirstOrDefault(file => imageExtensions.Any(ext => file.EndsWith(ext, System.StringComparison.OrdinalIgnoreCase)));

        if (firstImageFile == null)
        {
            Debug.LogWarning("Aucune image trouvée dans le dossier.");
            return;
        }

        Debug.Log($"Chargement de l'image : {Path.GetFileName(firstImageFile)}");

        // Charge la texture en asset depuis le chemin relatif Assets
        string relativeImagePath = firstImageFile.Replace(Application.dataPath, "Assets").Replace('\\', '/');
        Texture2D texture = AssetDatabase.LoadAssetAtPath<Texture2D>(relativeImagePath);

        if (texture == null)
        {
            Debug.LogError("Impossible de charger la texture via AssetDatabase. Vérifiez le format de l'image.");
            return;
        }

        if (backgroundPlaneRenderer != null)
        {
            backgroundPlaneRenderer.material.mainTexture = texture;
            Debug.Log("Texture assignée avec succès au Plane !");
        }
        else
        {
            Debug.LogError("Référence au Renderer du Plane non assignée.");
        }
    }
}

