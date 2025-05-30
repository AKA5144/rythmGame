using System.Collections.Generic;
using UnityEngine;

public class BeatManager : MonoBehaviour
{
    public metronome metronomeSound;
    public PlayerSongManager song;
    public GameObject beatPrefab;
    public Transform indicator;
    public Transform parentTransform;
    public Canvas canvas;

    [Header("Rythme et défilement")]
    public float bpm = 120f;
    public float scrollSpeed = 8f;

    private float interval; // en secondes
    private float beatDistance;

    private float lastTickTime = -999f;

    private GameObject movingBeat;
    private List<GameObject> staticBeats = new List<GameObject>();

    void Start()
    {
        UpdateBeatSpacing();
        SpawnBeats();
    }

    void Update()
    {
        if (!song.isPlaying) return;

        float songTime = song.audioSource.time();
        float timeSinceLastBeat = songTime % interval;

        // Distance entre beat mobile et indicator (droite)
        float offset = beatDistance * (1f - (timeSinceLastBeat / interval));
        Vector3 movingBeatPos = indicator.localPosition + Vector3.right * offset;
        movingBeat.GetComponent<RectTransform>().localPosition = movingBeatPos;

        // Statics derrière
        for (int i = 0; i < staticBeats.Count; i++)
        {
            float staticOffset = offset + beatDistance * (i + 1);
            Vector3 staticPos = indicator.localPosition + Vector3.right * staticOffset;
            staticBeats[i].GetComponent<RectTransform>().localPosition = staticPos;
        }

        // Tick quand le beat passe l'indicator
        if (songTime - lastTickTime >= interval)
        {
            lastTickTime = songTime - (songTime % interval);
            metronomeSound.PlayTick();
        }

        // Si le bpm change dynamiquement
        if (Mathf.Abs(60f / bpm - interval) > 0.001f)
        {
            UpdateBeatSpacing();
            SpawnBeats();
        }
    }

    void UpdateBeatSpacing()
    {
        interval = 60f / bpm;
        beatDistance = scrollSpeed * interval;
    }

    void SpawnBeats()
    {
        // Supprime anciens
        if (movingBeat != null) Destroy(movingBeat);
        foreach (var beat in staticBeats) Destroy(beat);
        staticBeats.Clear();

        // Beat mobile
        GameObject moving = Instantiate(beatPrefab, parentTransform);
        moving.name = "MovingBeat";
        movingBeat = moving;

        // Statics jusqu’au bord droit
        RectTransform canvasRect = canvas.GetComponent<RectTransform>();
        float rightEdge = canvasRect.rect.width * 0.5f;

        float currentOffset = beatDistance;

        while ((indicator.localPosition.x + currentOffset) < rightEdge)
        {
            GameObject beat = Instantiate(beatPrefab, parentTransform);
            staticBeats.Add(beat);
            currentOffset += beatDistance;
        }
    }
}
