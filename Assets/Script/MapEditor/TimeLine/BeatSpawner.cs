using System.Collections.Generic;
using UnityEngine;

public class BeatSpawner : MonoBehaviour
{
    public GameObject beatPrefab;
    public Transform indicator;    
    public Transform parentTransform; 
    public float distanceFromIndicator = 10f;

    private void Start()
    {
        SpawnBeat();
    }
    public void SpawnBeat()
    {
        Vector3 spawnLocalPos = new Vector3(
            indicator.localPosition.x + distanceFromIndicator,
            indicator.localPosition.y,
            0f
        );

        GameObject newBeat = Instantiate(beatPrefab, parentTransform);
        newBeat.transform.localPosition = spawnLocalPos;
        newBeat.GetComponent<Beat>().indicator = indicator;
    }
}
