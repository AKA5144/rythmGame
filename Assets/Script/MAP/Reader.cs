using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Globalization;
using System;

public class Reader : MonoBehaviour
{
    public string filePath = "Assets/Maps/Test/test.txt";
    public float bpm;
    public int fps;
    public float AR;
    public PlayMusic music;

    private float timeBetweenBeat;
    private float timeAccumulator = 0f;
    private float lastBeatTime = 0f;
    private float timeToReachOne;
    private float playOffsetMap;

    public struct PositionData
    {
        public float timeCode;
        public Vector3 position;

        public PositionData(float timeCode, Vector3 position)
        {
            this.timeCode = timeCode;
            this.position = position;
        }
    }
    private List<PositionData> dataList = new List<PositionData>();
    public GameObject circle;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playOffsetMap = -2;
        SetTimeBetweenBeat();
        lastBeatTime = Time.time;
        ReadDataFromFile();
        timeToReachOne = (2.5f - 1) / (AR * 0.5f);

    }

    void SetTimeBetweenBeat()
    {
        timeBetweenBeat = 60f / bpm;
    }

    // Update is called once per frame
    void Update()
    {
        if (playOffsetMap < 0)
        {
            playOffsetMap += Time.deltaTime;
            if(dataList.Count > 0 && playOffsetMap + timeToReachOne > dataList[0].timeCode)
            {
                Instantiate(circle, dataList[0].position, Quaternion.identity);
                dataList.RemoveAt(0);
            }
        }
        else
        {
            music.StartSong();
            PlayMap();
        }
    }

    void PlayMap()
    {
        float currentTime = Time.time;
        currentTime = currentTime - 2;
        if (dataList.Count > 0 && currentTime >= dataList[0].timeCode - timeToReachOne)
        {
            Instantiate(circle, dataList[0].position, Quaternion.identity);
            dataList.RemoveAt(0);
        }
    }
    void ReadDataFromFile()
    {
        if (!File.Exists(filePath))
        {
            UnityEngine.Debug.Log("Fichier introuvable : " + filePath);
            return;
        }

        try
        {
            string[] lines = File.ReadAllLines(filePath);

            foreach (string line in lines)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;

                string[] values = line.Split(';'); // Séparer les données par ";"

                if (values.Length < 4)
                {
                    UnityEngine.Debug.LogWarning("Ligne mal formatée : " + line);
                    continue;
                }

                try
                {
                    // Utilisation de CultureInfo pour interpréter les virgules comme séparateurs décimaux
                    var culture = CultureInfo.GetCultureInfo("fr-FR");

                    float timeCode = float.Parse(values[0].Trim(), culture);
                    float x = float.Parse(values[1].Trim(), culture);
                    float y = float.Parse(values[2].Trim(), culture);
                    float z = float.Parse(values[3].Trim(), culture);

                    Vector3 position = new Vector3(x, y, z);
                    PositionData data = new PositionData(timeCode, position);

                    dataList.Add(data);
                }
                catch (FormatException ex)
                {
                    UnityEngine.Debug.LogError("Erreur de formatage sur la ligne : " + line + " | " + ex.Message);
                }
            }

            UnityEngine.Debug.Log("Données lues : " + dataList.Count + " entrées.");
        }
        catch (Exception ex)
        {
            UnityEngine.Debug.LogError("Erreur lors de la lecture du fichier : " + filePath + " | " + ex.Message);
        }
    }
    void DebugCountTimeBetweenBeat()
    {
        timeAccumulator += Time.deltaTime;

        if (timeAccumulator >= timeBetweenBeat)
        {
            float currentTime = Time.time;
            float timeSinceLastBeat = currentTime - lastBeatTime;

            UnityEngine.Debug.Log("Time since last beat: " + timeSinceLastBeat + " seconds");

            lastBeatTime = currentTime;

            timeAccumulator -= timeBetweenBeat;
        }
    }

}
