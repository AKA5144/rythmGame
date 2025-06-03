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
    private GameObject lastClickedCircle;
    private bool isDragging = false;

    void Start()
    {
        timeToReachOne = (2.5f - 1f) / (approachRate * 0.5f);
        // AddSliderData(2.5f, new Vector2(68, 200), new Vector2(188, 224));
    }

    void Update()
    {
        ConvertMousePos();
        DetectCircleUnderMouse();

        float currentTime = audioSource.time;

        // Gestion des cercles visibles / destruction
        foreach (CircleData data in allCirclesData)
        {
            bool shouldBeVisible = (data.timeCode >= currentTime) && (data.timeCode - currentTime <= visibleRange);

            if (shouldBeVisible && !activeCircles.ContainsKey(data))
            {
                GameObject go = Instantiate(circlePrefab, parentTransform);
                EditorCircle circle = go.GetComponent<EditorCircle>();

                circle.circleData = new CircleData();
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
                newProp.GetComponent<ShrinkingEditorCIrcle>().circle = circle;

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

    public void DetectCircleUnderMouse()
    {
        Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        // Détection du clic initial
        if (Input.GetMouseButtonDown(0))
        {
            Collider2D hit = Physics2D.OverlapPoint(mouseWorldPos);

            if (hit != null && hit.CompareTag("Circle"))
            {
                if (hit.gameObject != lastClickedCircle)
                {
                    // Réinitialiser l'ancien
                    if (lastClickedCircle != null)
                    {
                        SpriteRenderer oldSR = lastClickedCircle.GetComponent<SpriteRenderer>();
                        if (oldSR != null)
                            oldSR.color = Color.white;
                    }

                    // Sélectionner le nouveau
                    SpriteRenderer newSR = hit.GetComponent<SpriteRenderer>();
                    if (newSR != null)
                        newSR.color = Color.yellow;

                    lastClickedCircle = hit.gameObject;

                }

                isDragging = true;
            }
            else
            {
                // Si on clique ailleurs, désélectionner
                if (lastClickedCircle != null)
                {
                    SpriteRenderer sr = lastClickedCircle.GetComponent<SpriteRenderer>();
                    if (sr != null)
                        sr.color = Color.white;

                    lastClickedCircle = null;
                }

                isDragging = false;
            }
        }

        // Relâchement du clic
        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }
        if (lastClickedCircle != null && Input.GetKeyDown(KeyCode.Delete))
        {
            // Récupérer le component EditorCircle lié
            var shrinkingScript = lastClickedCircle.transform.parent.GetComponent<ShrinkingEditorCIrcle>();
            if (shrinkingScript != null)
            {
                EditorCircle editorCircle = shrinkingScript.circle;

                if (editorCircle != null)
                {
                    // Supprimer le CircleData correspondant
                    CircleData targetData = null;
                    foreach (var kvp in activeCircles)
                    {
                        if (kvp.Value == editorCircle)
                        {
                            targetData = kvp.Key;
                            break;
                        }
                    }

                    if (targetData != null)
                    {
                        allCirclesData.Remove(targetData);
                        activeCircles.Remove(targetData);
                    }

                    // Supprimer le prop et l'EditorCircle
                    Destroy(editorCircle.gameObject);
                }

                // Supprimer le parent (le prop)
                Destroy(lastClickedCircle.transform.parent.gameObject);
            }

            lastClickedCircle = null;

            Debug.Log("Props, EditorCircle et données supprimés !");
        }
        // Suivi du cercle tant qu'on maintient le clic
        if (isDragging && lastClickedCircle != null)
        {
            /* Vector2 _mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

             lastClickedCircle.transform.parent.position = _mouseWorldPos;

             var shrinkingScript = lastClickedCircle.transform.parent.GetComponent<ShrinkingEditorCIrcle>();
             if (shrinkingScript != null && shrinkingScript.circle != null)
             {
                 shrinkingScript.circle.position = _mouseWorldPos;

                 Debug.Log($"Position mise à jour : {shrinkingScript.circle.position} (Mouse World Pos : {_mouseWorldPos})");
             }
             else
             {
                 Debug.LogWarning("shrinkingScript ou shrinkingScript.circle est null !");
             }*/
        }
    }
    void ConvertMousePos()
    {
        Vector3 mousePos = Input.mousePosition;
        float invertedY = Screen.height - mousePos.y;

        Vector3 convertPos;
        convertPos.x = (mousePos.x * 0.3393f) - 87.624f;
        convertPos.y = (invertedY * 0.4819f) - 44.817f;
        convertPos.z = 1f;
        if (Input.GetMouseButtonDown(1))
        {
            //convertPos = propsParent.InverseTransformPoint(MousePos);
            AddCircleData(audioSource.time, convertPos);
            Debug.Log(convertPos);
        }
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
    public List<CircleData> GetAllCirclesData()
    {
        // Retourne une copie pour éviter la modification directe
        return new List<CircleData>(allCirclesData);
    }

}

public class CircleData
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