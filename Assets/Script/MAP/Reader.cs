using UnityEngine;
using System.Collections.Generic;
using System.IO;
using static UnityEngine.Rendering.DebugUI;

public class Reader : MonoBehaviour
{
    public string filePath = "Assets/Maps/Test/test.txt";
    public float bpm;
    public int fps;
    public float AR;

    private float timeBetweenBeat;
    private float timeAccumulator = 0f;
    private float lastBeatTime = 0f;
    List<float> values;
    private float timeToReachOne;

    public GameObject circle;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SetTimeBetweenBeat();
        lastBeatTime = Time.time;
        values = ReadValuesFromFile(filePath);
        timeToReachOne = (2.5f - 1) / (AR * 0.5f);
    }

    void SetTimeBetweenBeat()
    {
        timeBetweenBeat = 60f / bpm;
    }
    // Update is called once per frame
    void Update()
    {
        float currentTime = Time.time;

        if (values.Count > 0 && currentTime >= values[0] - timeToReachOne)
        {            
            Instantiate(circle);
            values.RemoveAt(0);
        }
    }
    List<float> ReadValuesFromFile(string path)
    {
        List<float> values = new List<float>();

        try
        {
            if (File.Exists(path))
            {
                string[] lines = File.ReadAllLines(path);

                foreach (string line in lines)
                {
                    if (float.TryParse(line, out float result))
                    {
                        values.Add(result); 
                    }
                    else
                    {
                        Debug.LogWarning($"Impossible de convertir '{line}' en float.");
                    }
                }
            }
            else
            {
                Debug.LogError($"Fichier introuvable : {path}");
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Erreur lors de la lecture du fichier : {ex.Message}");
        }

        return values;
    }
    void DebugCountTimeBetweenBeat()
    {
        timeAccumulator += Time.deltaTime;

        if (timeAccumulator >= timeBetweenBeat)
        {
            float currentTime = Time.time;
            float timeSinceLastBeat = currentTime - lastBeatTime;

            Debug.Log("Time since last beat: " + timeSinceLastBeat + " seconds");

            lastBeatTime = currentTime;

            timeAccumulator -= timeBetweenBeat;
        }
    }

}
