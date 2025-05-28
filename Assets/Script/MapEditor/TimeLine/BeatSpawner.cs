using System.Collections.Generic;
using UnityEngine;

public class BeatSpawner : MonoBehaviour
{
    public metronome metronomeSound;
    public GameObject beatPrefab;
    public Transform indicator;
    public Transform parentTransform;

    [Header("Rythme et défilement")]
    public float bpm = 120f;
    public float scrollSpeed = 8f;
    public int extraBeats = 4; // Nombre de beats statiques derrière

    private float spawnInterval;
    private float distanceFromIndicator;
    private Beat movingBeat;
    private List<GameObject> staticBeats = new List<GameObject>();

    void Start()
    {
        UpdateBeatSpacing();
        SpawnMovingBeat();
        SpawnStaticBeats();
    }

    void Update()
    {
        // Bouger uniquement le beat mobile
        if (movingBeat != null)
        {
            movingBeat.Move();

            if (movingBeat.BeatComplete(indicator))
            {
                metronomeSound.PlayTick();
                movingBeat.transform.localPosition = movingBeat.startPos;
            }

            // Met à jour la position des beats statiques derrière le movingBeat
            UpdateStaticBeatsPosition();
        }

        // Mise à jour dynamique du bpm (optionnel)
        if (Mathf.Abs(60f / bpm - spawnInterval) > 0.001f)
        {
            UpdateBeatSpacing();
            SpawnStaticBeats();
            ResetBeatsPositions();
        }
    }

    void ResetBeatsPositions()
    {
        Vector3 newStartPos = indicator.localPosition + Vector3.right * distanceFromIndicator;

        // Reset position et vitesse du movingBeat
        movingBeat.startPos = newStartPos;
        movingBeat.transform.localPosition = newStartPos;
        movingBeat.speed = scrollSpeed;

        // Reset positions des beats statiques
        for (int i = 0; i < staticBeats.Count; i++)
        {
            Vector3 staticPos = indicator.localPosition + Vector3.right * distanceFromIndicator * (i + 2);
            // (i+2) car static beats commencent après le movingBeat (qui est i=0)
            staticBeats[i].transform.localPosition = staticPos;
        }
    }
    void UpdateBeatSpacing()
    {
        spawnInterval = 60f / bpm;
        distanceFromIndicator = scrollSpeed * spawnInterval;
        
    }

    void SpawnMovingBeat()
    {
        Vector3 spawnPos = indicator.localPosition + Vector3.right * distanceFromIndicator;
        Vector3 worldPos = parentTransform.TransformPoint(spawnPos);
        GameObject newBeat = Instantiate(beatPrefab, worldPos, Quaternion.identity, parentTransform);

        movingBeat = newBeat.GetComponent<Beat>();
        movingBeat.startPos = spawnPos;
        movingBeat.speed = scrollSpeed;
    }

    void SpawnStaticBeats()
    {
        staticBeats.Clear();

        // On récupère la position écran (bord droit) avec une profondeur valide
        float zDistance = Mathf.Abs(Camera.main.transform.position.z - parentTransform.position.z);

        Vector3 screenRightWorld = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, zDistance));
        Vector3 screenRightLocal = parentTransform.InverseTransformPoint(screenRightWorld);

        Debug.Log($"screenRightWorld.x = {screenRightWorld.x}, screenRightLocal.x = {screenRightLocal.x}");
        Debug.Log($"indicator.localPosition.x = {indicator.localPosition.x}, distanceFromIndicator = {distanceFromIndicator}");

        float currentX = indicator.localPosition.x + distanceFromIndicator;

        while (currentX <= screenRightLocal.x)
        {
            Vector3 spawnPos = new Vector3(currentX, indicator.localPosition.y, indicator.localPosition.z);
            Vector3 worldPos = parentTransform.TransformPoint(spawnPos);

            GameObject beat = Instantiate(beatPrefab, worldPos, Quaternion.identity, parentTransform);
            staticBeats.Add(beat);

            Debug.Log($"Spawn beat at local x = {currentX}");

            currentX += distanceFromIndicator;
        }
    }


    void UpdateStaticBeatsPosition()
    {
        if (movingBeat == null) return;

        Vector3 basePos = movingBeat.transform.localPosition;

        // Décale chaque beat statique derrière movingBeat, espacés par distanceFromIndicator
        for (int i = 0; i < staticBeats.Count; i++)
        {
            Vector3 targetPos = basePos + Vector3.right * distanceFromIndicator * (i + 1);
            staticBeats[i].transform.localPosition = targetPos;
        }
    }
}
