using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class EditorCircleManager : MonoBehaviour
{
    [SerializeField] BeatManager beatManager;
    [SerializeField] Transform parentTransform;
    [SerializeField] GameObject circlePrefab;
    [SerializeField] GameObject propsPrefab;
    [SerializeField] GameObject SliderPrefab;
    [SerializeField] GameObject SliderProps;
    [SerializeField] Transform propsParent;

    [SerializeField] float approachRate = 5f;

    private float scrollSpeed => beatManager.scrollSpeed;
    private Transform indicator => beatManager.indicator;
    private AudioSource audioSource => beatManager.song.audioSource;

    private List<CircleData> allCirclesData = new List<CircleData>();
    private Dictionary<CircleData, EditorCircle> activeCircles = new Dictionary<CircleData, EditorCircle>();
    private float visibleRange = 10f;

    public List<SliderData> allSlidersData = new List<SliderData>();
    private Dictionary<SliderData, EditorSlider> activeSliders = new Dictionary<SliderData, EditorSlider>();

    private List<GameObject> activeProps = new List<GameObject>();
    private Dictionary<GameObject, float> propTimeCodes = new Dictionary<GameObject, float>();

    public float speedSlider = 5f;
    private float timeToReachOne;

    void Start()
    {
        timeToReachOne = (2.5f - 1f) / (approachRate * 0.5f);
       // AddSliderData(2.5f, new Vector2(68, 200), new Vector2(188, 224));
    }

    void Update()
    {
        ConvertMousePos();
        if (Input.GetKeyDown(KeyCode.E))
        {
            AddCircleData(2.5f, new Vector2(32, 128));
        }
        float currentTime = audioSource.time;

        // Gestion des cercles visibles / destruction
        foreach (CircleData data in allCirclesData)
        {
            bool shouldBeVisible = (data.timeCode >= currentTime) && (data.timeCode - currentTime <= visibleRange);

            if (shouldBeVisible && !activeCircles.ContainsKey(data))
            {
                GameObject go = Instantiate(circlePrefab, parentTransform);
                EditorCircle circle = go.GetComponent<EditorCircle>();
                circle.timeCode = data.timeCode;
                circle.position = data.position;

                activeCircles[data] = circle;
            }
            else if (!shouldBeVisible && activeCircles.ContainsKey(data))
            {
                Destroy(activeCircles[data].gameObject);
                activeCircles.Remove(data);
            }
        }

        foreach (SliderData data in allSlidersData)
        {
            bool shouldBeVisible = (data.timeCode >= currentTime) && (data.timeCode - currentTime <= visibleRange);

            if (shouldBeVisible && !activeSliders.ContainsKey(data))
            {
                GameObject go = Instantiate(SliderPrefab, parentTransform);
                EditorSlider slider = go.GetComponent<EditorSlider>();
                slider.timeCode = data.timeCode;
                slider.beginPos.transform.position = new Vector3(data.begin.x, data.begin.y, 0f);

                activeSliders[data] = slider;
            }
            else if (!shouldBeVisible && activeSliders.ContainsKey(data))
            {
                Destroy(activeSliders[data].gameObject);
                activeSliders.Remove(data);
            }
        }
        // Mise à jour position & instanciation des props
        foreach (var kvp in activeCircles)
        {
            EditorCircle circle = kvp.Value;
            float timeUntilHit = circle.timeCode - currentTime;
            float xOffset = timeUntilHit * scrollSpeed;

            Vector3 basePosition = indicator.localPosition;
            Vector3 newPosition = basePosition + Vector3.right * xOffset;
            newPosition.y = indicator.localPosition.y;

            circle.transform.localPosition = newPosition;

            if (currentTime > circle.timeCode - timeToReachOne && !circle.circleSpawned)
            {
                Vector3 convertPos = Vector3.zero;
                convertPos.x = -5f + (circle.position.x * 0.029296875f);
                convertPos.y = 7f - (circle.position.y * 0.0205729167f);
                convertPos.z = 1f;
                GameObject newProp = Instantiate(propsPrefab, propsParent);
                newProp.transform.localPosition = convertPos;
                activeProps.Add(newProp);
                propTimeCodes[newProp] = circle.timeCode;

                ShrinkingEditorCIrcle pc = newProp.GetComponent<ShrinkingEditorCIrcle>();
                if (pc != null)
                    pc.SetOutlineScale(2.5f);

                circle.circleSpawned = true;
            }


            if (currentTime < circle.timeCode - timeToReachOne)
            {
                // Trouver le prop lié à ce cercle (par timeCode)
                GameObject propToRemove = null;
                foreach (var prop in activeProps)
                {
                    if (propTimeCodes.TryGetValue(prop, out float propTimeCode) && Mathf.Approximately(propTimeCode, circle.timeCode))
                    {
                        propToRemove = prop;
                        break;
                    }
                }

                if (propToRemove != null)
                {
                    Destroy(propToRemove);
                    activeProps.Remove(propToRemove);
                    propTimeCodes.Remove(propToRemove);

                    circle.circleSpawned = false; // Pour permettre de recréer le prop plus tard si besoin
                }
            }
        }

        foreach (var kvp in activeSliders)
        {
            EditorSlider slider = kvp.Value;
            float timeUntilHit = slider.timeCode - currentTime;
            float xOffset = timeUntilHit * scrollSpeed;

            Vector3 basePosition = indicator.localPosition;
            Vector3 newPosition = basePosition + Vector3.right * xOffset;
            newPosition.y = indicator.localPosition.y;

            slider.transform.localPosition  = newPosition;
        }
        // Mise à jour des scales des props en fonction du temps
        for (int i = activeProps.Count - 1; i >= 0; i--)
        {
            GameObject prop = activeProps[i];
            if (prop == null)
            {
                activeProps.RemoveAt(i);
                continue;
            }

            ShrinkingEditorCIrcle pc = prop.GetComponent<ShrinkingEditorCIrcle>();
            if (pc == null)
                continue;

            if (!propTimeCodes.TryGetValue(prop, out float propTimeCode))
                continue;

            float t = Mathf.InverseLerp(propTimeCode - timeToReachOne, propTimeCode, currentTime);
            float scale = Mathf.Lerp(2.5f, 1f, t);

            pc.SetOutlineScale(scale);

            if (currentTime > propTimeCode)
            {
                Destroy(prop);
                activeProps.RemoveAt(i);
                propTimeCodes.Remove(prop);
            }
        }
    }

    void ConvertMousePos()
    {
        Vector2 MousePos = Input.mousePosition;
        Vector2 convertPos = Input.mousePosition;
        convertPos.x = -5f + (MousePos.x * 0.029296875f);
        convertPos.y = 7f - (MousePos.y * 0.0205729167f);
        if (Input.GetMouseButtonDown(0))
        {
           // AddCircleData(2.5f, convertPos);
        }
        //Debug.Log(convertPos);
    }
    public void AddCircleData(float timeCode, Vector2 position)
    {
        allCirclesData.Add(new CircleData
        {
            timeCode = timeCode,
            position = position
        });
    }

    public void AddSliderData(float timeCode, Vector2 begin, Vector2 end)
    {
        allSlidersData.Add(new SliderData
        {
            timeCode = timeCode,
            begin = begin
        });
    }

    private class CircleData
    {
        public float timeCode;
        public Vector2 position;

        public override bool Equals(object obj)
        {
            if (obj is CircleData other)
                return Mathf.Approximately(timeCode, other.timeCode) && position == other.position;
            return false;
        }

        public override int GetHashCode()
        {
            return timeCode.GetHashCode() ^ position.GetHashCode();
        }
    }

    public void ReceiveData(List<MapFolderViewer.PositionTimeData> dataList)
    {
        ClearAllCircles(); // Nettoyage avant ajout

        Debug.Log($"EditorCircleManager reçoit {dataList.Count} données");

        foreach (var data in dataList)
        {
            float timeInSeconds = data.timecode / 1000f; // Conversion millisecondes -> secondes
            AddCircleData(timeInSeconds, new Vector2(data.positionX, data.positionY));
        }
    }

    public void ClearAllCircles()
    {
        allCirclesData.Clear();

        foreach (var kvp in activeCircles)
        {
            if (kvp.Value != null)
                Destroy(kvp.Value.gameObject);
        }
        activeCircles.Clear();

        Debug.Log("Tous les cercles et données ont été supprimés.");
    }

    public class SliderData
    {
        public float timeCode;
        public Vector2 begin;
    }
}
