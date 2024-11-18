using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using static MapReader;

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
    private float timerEnd;

    private List<PositionData> dataList = new List<PositionData>();
    public GameObject Circle;
    public GameObject SliderPoint;
    public GameObject Slider;

    void Start()
    {
        playOffsetMap = -2f;
        SetTimeBetweenBeat();
        lastBeatTime = Time.time;
        dataList = ReadDataFromFile(filePath);
        timeToReachOne = (2.5f - 1f) / (AR * 0.5f);
        timeToReachOne = timeToReachOne - offset;
    }

    void SetTimeBetweenBeat()
    {
        timeBetweenBeat = 60f / bpm;
    }

    void PlayData(ref PositionData positionData)
    {
        Vector3 convertPos = Vector3.zero;
        convertPos.x = -5f + (positionData.position.x * 0.029296875f);
        convertPos.y = 7f - (positionData.position.y * 0.0205729167f);
        convertPos.z = 1f;
        if (positionData.objectType == ObjectType.Circle)
        {
            GameObject circleInstance = Instantiate(Circle, Vector3.zero, Quaternion.identity, ParentTransform);
            circleInstance.transform.localPosition = convertPos;
        }
        else if (positionData.objectType == ObjectType.Slider)
        {
            GameObject sliderInstance = Instantiate(Slider, Vector3.zero, Quaternion.identity, ParentTransform);

            Transform pointsParent = sliderInstance.transform.Find("Point");
            foreach (var point in positionData.sliderPoints)
            {
                float px = -7.5f + (point.x * 0.030078125f);
                float py = 3.7f - (point.y * 0.0200520833f);
                Instantiate(SliderPoint, new Vector3(px, py, 0), Quaternion.identity, pointsParent);
            }
        }
    }

    void Update()
    {
        if (playOffsetMap < 0f)
        {
            playOffsetMap += Time.deltaTime;
            if (dataList.Count > 0 && playOffsetMap + timeToReachOne > dataList[0].timeCode)
            {
                Vector3 convertPos = Vector3.zero;
                convertPos.x = -5f + (dataList[0].position.x * 0.029296875f);
                convertPos.y = 7f - (dataList[0].position.y * 0.0205729167f);
                convertPos.z = 1f;
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
                        float py = 3.7f - (point.y * 0.0200520833f);
                        Instantiate(SliderPoint, new Vector2(px, py), Quaternion.identity, pointsParent);
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
            convertPos.y = 7 - (dataList[0].position.y * 0.0205729167f);
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
                    float py = 3.7f - (point.y * 0.0200520833f);
                    Vector3 globalPosition = new Vector2(px, py);
                    GameObject pointInstance = Instantiate(SliderPoint, globalPosition, Quaternion.identity, pointsParent);
                    pointInstance.transform.localPosition = globalPosition;

                }
            }
            dataList.RemoveAt(0);
        }
        EndMap();
    }
    void EndMap()
    {
        
        if (dataList.Count <= 0 && ParentTransform.childCount <= 0)
        {
            timerEnd += Time.deltaTime;
            if(timerEnd >= 2f)
            {
                SceneManager.LoadScene("Scenes/Scoreboard");
            }
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
