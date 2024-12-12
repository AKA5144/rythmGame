using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class SongLenghtDisplayer : MonoBehaviour
{
    public AudioSource song;
    public TextMeshProUGUI text;
    [SerializeField] SongInfoEditor info;

    private string formattedTime;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log(info.clipLength);
    }

    // Update is called once per frame
    void Update()
    {
        formattedTime = ConvertSecondsToTime(info.timeCode);        
        text.text = formattedTime;
    }
    string ConvertSecondsToTime(float totalSeconds)
    {
        int minutes = Mathf.FloorToInt(totalSeconds / 60); // Calcul des minutes
        int seconds = Mathf.FloorToInt(totalSeconds % 60); // Calcul des secondes
        int milliseconds = Mathf.FloorToInt((totalSeconds % 1) * 1000); // Extraction des millisecondes

        // Retourner le temps formaté mm:ss:msms
        return $"{minutes:D2}:{seconds:D2}:{milliseconds:D3}";
    }
}

