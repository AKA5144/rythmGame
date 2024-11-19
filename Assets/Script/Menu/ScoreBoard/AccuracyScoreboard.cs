using TMPro;
using UnityEngine;

public class AccuracyScoreboard : MonoBehaviour
{
    public TextMeshProUGUI textMeshProUGUI;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        textMeshProUGUI.text = new string(ScoringManager.accuracy.ToString() + " %");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
