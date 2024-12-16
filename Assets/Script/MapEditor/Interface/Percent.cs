using TMPro;
using UnityEngine;

public class Percent : MonoBehaviour
{
    public TextMeshProUGUI text;
    public SongInfoEditor info;
    float currentpercent;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        currentpercent = (info.timeCode * 100) / info.clipLength;
        string formattedPercent = currentpercent.ToString("F1");
        text.text = formattedPercent + " %";
    }
}
