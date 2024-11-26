using System.IO;
using UnityEditor;
using UnityEngine;

public class DrangAndDrop : MonoBehaviour
{
    private string fileContent = "";

    private void OnGUI()
    {
        // Zone d'instructions
        GUILayout.Label("Drag and drop a file into the window.", EditorStyles.boldLabel);
        GUILayout.Space(10);

        // Zone de détection du drag and drop
        Rect dropArea = GUILayoutUtility.GetRect(0.0f, 100.0f, GUILayout.ExpandWidth(true));
        GUI.Box(dropArea, "Drop your file here");

        Event evt = Event.current;

        if (evt.type == EventType.DragUpdated || evt.type == EventType.DragPerform)
        {
            if (dropArea.Contains(evt.mousePosition))
            {
                DragAndDrop.visualMode = DragAndDropVisualMode.Copy;

                if (evt.type == EventType.DragPerform)
                {
                    DragAndDrop.AcceptDrag();

                    foreach (string path in DragAndDrop.paths)
                    {
                        Debug.Log("File dropped: " + path);

                        // Lire le contenu du fichier
                        if (File.Exists(path))
                        {
                            fileContent = File.ReadAllText(path);
                            Debug.Log("File content:\n" + fileContent);
                        }
                        else
                        {
                            Debug.LogError("The dropped item is not a valid file.");
                        }
                    }

                    evt.Use();
                }
            }
        }

        // Afficher le contenu du fichier si disponible
        if (!string.IsNullOrEmpty(fileContent))
        {
            GUILayout.Space(10);
            GUILayout.Label("File Content:");
            GUILayout.TextArea(fileContent, GUILayout.Height(200));
        }
    }
}
