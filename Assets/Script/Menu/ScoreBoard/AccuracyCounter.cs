using TMPro;
using UnityEngine;


public class AccuracyCounter : MonoBehaviour
{
    public TextMeshProUGUI textMeshProUGUI;
    public bool miss;
    public bool bad;
    public bool good;
    public bool perfect;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(miss){
        textMeshProUGUI.text = new string(" x" + ScoringManager.miss.ToString());
        }
        else if(bad) {
            textMeshProUGUI.text = new string(" x" + ScoringManager.Bad.ToString());
        }
        else if(good) {
            textMeshProUGUI.text = new string(" x" + ScoringManager.Good.ToString());
        }
        else if (perfect) {
            textMeshProUGUI.text = new string(" x" + ScoringManager.Perfect.ToString());
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
