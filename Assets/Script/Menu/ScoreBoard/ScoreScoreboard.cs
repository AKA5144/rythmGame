using TMPro;
using UnityEngine;

public class ScoreScoreboard : MonoBehaviour
{
    public TextMeshProUGUI textMeshProUGUI;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        textMeshProUGUI.text = new string(ScoringManager.score.ToString());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
