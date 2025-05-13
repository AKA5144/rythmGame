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
    }

    // Update is called once per frame
    void Update()
    {
    }
}

