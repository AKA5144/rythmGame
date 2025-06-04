using TMPro;
using UnityEngine;

public class BPMText : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI TextMeshProUGUI;
    [SerializeField] BeatManager manager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        TextMeshProUGUI.text = new string("BPM : " + manager.bpm);
    }
}
