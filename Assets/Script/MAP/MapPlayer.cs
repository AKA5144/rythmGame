using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using static MapReader;

public class MapPlayer : MonoBehaviour
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
    private bool gamePause = false;
    private bool SongStarted = false;

    private List<PositionData> dataList = new List<PositionData>();
    public GameObject Circle;
    public GameObject SliderPoint;
    public GameObject Slider;

    void Start()
    {
        playOffsetMap = -2f;
        SetTimeBetweenBeat();
        lastBeatTime = Time.time;
        dataList = ReadDataFromFile(MapReader.mapPath + "/Info.txt");
        //dataList = ReadDataFromFile(filePath);
        timeToReachOne = (2.5f - 1f) / (AR * 0.5f);
        timeToReachOne = timeToReachOne - offset;
        ScoringManager.accuracy = 100;
        ScoringManager.score  = 0;
        ScoringManager.miss = 0;
        ScoringManager.Bad = 0;
        ScoringManager.Good = 0;
        ScoringManager.Perfect = 0;
    }

    void SetTimeBetweenBeat()
    {
        timeBetweenBeat = 60f / bpm;
    }

    void PlayPositionData(PositionData positionData)
    {
        Debug.Log(positionData.position);
        Vector3 convertPos = Vector3.zero;
        convertPos.x = -5f + (positionData.position.x * 0.029296875f);
        convertPos.y = 7f - (positionData.position.y * 0.0205729167f);
        convertPos.z = 1f;
        Debug.Log(convertPos);
        if (positionData.objectType == ObjectType.Circle)
        {
            GameObject circleInstance = Instantiate(Circle, Vector3.zero, Quaternion.identity, ParentTransform);
            circleInstance.transform.localPosition = convertPos;
        }
        else if (positionData.objectType == ObjectType.Slider)
        {
            GameObject sliderInstance = Instantiate(Slider, Vector3.zero, Quaternion.identity, ParentTransform);
            sliderInstance.GetComponent<BeginSlider>().totalTravel = positionData.repeatCount;
            Transform pointsParent = sliderInstance.transform.Find("Point");
            foreach (var point in positionData.sliderPoints)
            {
                float px = -7.5f + (point.x * 0.030078125f);
                float py = 3.7f - (point.y * 0.0200520833f);
                Instantiate(SliderPoint, new Vector2(px, py), Quaternion.identity, pointsParent);
            }
        }
    }

    void Update()
    {
        if (playOffsetMap < 0f)
        {
            if(!gamePause)
            {
                playOffsetMap += Time.deltaTime;
                if (dataList.Count > 0 && playOffsetMap + timeToReachOne > dataList[0].timeCode)
                {
                    PlayPositionData(dataList[0]);
                    dataList.RemoveAt(0);
                }
            }
        }
        else
        {
            if (!SongStarted)
            {
                KeepSong.instance.PlayAudio();
                SongStarted = true;
            }
            float currentTime = Time.timeSinceLevelLoad;
            currentTime = currentTime - 2;
            if (dataList.Count > 0 && currentTime >= dataList[0].timeCode - timeToReachOne)
            {
                PlayPositionData(dataList[0]);
                dataList.RemoveAt(0);
            }
            EndMap();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            gamePause = !gamePause;
            if (gamePause)
            {
                KeepSong.instance.PauseAudio();
            }
            else
            { if (SongStarted == true)
                KeepSong.instance.PlayAudio();
            }
        }
        Time.timeScale = gamePause ? 0f : 1f;
    }
    void EndMap()
    {

        if (dataList.Count <= 0 && ParentTransform.childCount <= 0)
        {
            timerEnd += Time.deltaTime;
            if (timerEnd >= 2f)
            {
                SceneManager.LoadScene("Scenes/Scoreboard");
            }
        }
    }

    /* void DebugCountTimeBetweenBeat()
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
     }*/
}