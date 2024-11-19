using TMPro;
using UnityEngine;

public class ScoringText : MonoBehaviour
{
    public TextMeshProUGUI textMeshProUGUI;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        textMeshProUGUI.text = ScoringManager.score.ToString();
        
    }
}
