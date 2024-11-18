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
    public Transform ParentTransform;
    public float offset;

    private float timeBetweenBeat;
    private float timeAccumulator = 0f;
    private float lastBeatTime = 0f;
    private float timeToReachOne;
    private float playOffsetMap;

    public enum ObjectType
    {
        Circle,
        Slider
    }
    public struct PositionData
    {
        public float timeCode;
        public Vector3 position;
        public ObjectType objectType;
        public List<Vector3> sliderPoints;
        public PositionData(float timeCode, Vector3 position, ObjectType objectType, List<Vector3> sliderPoints = null)
        {
            this.timeCode = timeCode;
            this.position = position;
            this.objectType = objectType;
            this.sliderPoints = sliderPoints ?? new List<Vector3>();
        }
    }
    private List<PositionData> dataList = new List<PositionData>();
    public GameObject Circle;
    public GameObject SliderPoint;
    public GameObject Slider;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playOffsetMap = -2;
        SetTimeBetweenBeat();
        lastBeatTime = Time.time;
        ReadDataFromFile();
        timeToReachOne = (2.5f - 1) / (AR * 0.5f);
        timeToReachOne = timeToReachOne - offset;

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
            if (dataList.Count > 0 && playOffsetMap + timeToReachOne > dataList[0].timeCode)
            {
                Vector3 convertPos = Vector3.zero;
                convertPos.x = -5 + (dataList[0].position.x * 0.029296875f);
                convertPos.y = 7 - (dataList[0].position.y * 0.02057291666666666666666666666667f);
                convertPos.z = 1;
                if (dataList[0].objectType == ObjectType.Circle)
                {
                    GameObject circleInstance = Instantiate(Circle, Vector3.zero, Quaternion.identity, ParentTransform);
                    circleInstance.transform.localPosition = convertPos;
                }
                else if(dataList[0].objectType == ObjectType.Slider)
                {
                    GameObject sliderInstance = Instantiate(Slider, Vector3.zero, Quaternion.identity, ParentTransform);

                    Transform pointsParent = sliderInstance.transform.Find("Point");
                    foreach (var point in dataList[0].sliderPoints)
                    {
                        float px = -7.5f + (point.x * 0.030078125f);
                        float py = 3.7f - (point.y * 0.02005208333333333333333333333333f);
                        Instantiate(SliderPoint, new Vector3(px, py, 0), Quaternion.identity, pointsParent);
                    }
                }
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
            Vector3 convertPos = Vector3.zero;
            convertPos.x = -5 + (dataList[0].position.x * 0.029296875f);
            convertPos.y = 7 - (dataList[0].position.y * 0.02057291666666666666666666666667f);
            convertPos.z = 1;
            if (dataList[0].objectType == ObjectType.Circle)
            {
                GameObject circleInstance = Instantiate(Circle, Vector3.zero, Quaternion.identity, ParentTransform);
                circleInstance.transform.localPosition = convertPos;
            }
            else if (dataList[0].objectType == ObjectType.Slider)
            {
                GameObject sliderInstance = Instantiate(Slider, Vector3.zero, Quaternion.identity, ParentTransform);

                Transform pointsParent = sliderInstance.transform.Find("Point");
                foreach (var point in dataList[0].sliderPoints)
                {
                    float px = -7.5f + (point.x * 0.030078125f);
                    float py = 3.7f - (point.y * 0.02005208333333333333333333333333f);
                    Vector3 globalPosition = new Vector3(px, py, 0);
                    GameObject pointInstance = Instantiate(SliderPoint, globalPosition, Quaternion.identity, pointsParent);
                    pointInstance.transform.localPosition = globalPosition;

                }
            }
            dataList.RemoveAt(0);
        }
    }
    void ReadDataFromFile()
    {
        if (!File.Exists(filePath))
        {
            Debug.LogError("Fichier introuvable : " + filePath);
            return;
        }

        try
        {
            string[] lines = File.ReadAllLines(filePath);
            int lineNumber = 0;

            foreach (string line in lines)
            {
                lineNumber++;

                if (string.IsNullOrWhiteSpace(line)) continue;



                string[] values = line.Split(';');


                try
                {
                    var culture = CultureInfo.GetCultureInfo("fr-FR");

                    float x = float.Parse(values[0].Trim(), culture);
                    float y = float.Parse(values[1].Trim(), culture);
                    float timeCode = float.Parse(values[2].Trim(), culture);

                    Vector3 position = new Vector3(x , y , 0);

                    ObjectType objectType = (values.Length >= 4 && values[3].Trim().StartsWith("Sl")) ? ObjectType.Slider : ObjectType.Circle;
                    List<Vector3> sliderPoints = new List<Vector3>();
                    if (objectType == ObjectType.Slider)
                    {
                        string pointsString = values[3].Substring(2);

                        string[] pointPairs = pointsString.Split('|');

                        sliderPoints.Add(position);                        
                        foreach (var pointPair in pointPairs)
                        {
                            string[] coords = pointPair.Split(':');


                            if (coords.Length == 2)
                            {
                                try
                                {
                                    float px = float.Parse(coords[0].Trim(), culture);
                                    float py = float.Parse(coords[1].Trim(), culture);
                                    sliderPoints.Add(new Vector3(px, py, 0));

                                    Debug.Log($"Point ajouté : ({px}, {py})");
                                }
                                catch (FormatException ex)
                                {
                                    Debug.LogError("Erreur lors du parsing du point " + pointPair);
                                }
                            }
                            else
                            {
                                Debug.LogWarning("Point mal formaté : " + pointPair);
                            }
                        }
                    }


                    PositionData data = new PositionData(timeCode * 0.001f, position, objectType, sliderPoints);
                    dataList.Add(data);
                }
                catch (FormatException ex)
                {
                   Debug.LogError($"Erreur de formatage sur la ligne {lineNumber} : {line} | {ex.Message}");
                }
            }

        }
        catch (Exception ex)
        {
            Debug.LogError("Erreur lors de la lecture du fichier : " + filePath + " | " + ex.Message);
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
