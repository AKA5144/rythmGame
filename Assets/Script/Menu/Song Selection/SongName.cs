using TMPro;
using UnityEngine;

public class SongName : MonoBehaviour
{
    public TextMeshProUGUI m_TextMeshProUGUI;
    public SongInfo String;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        m_TextMeshProUGUI.text = String.songName;
    }
}
