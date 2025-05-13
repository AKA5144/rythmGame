using System.Collections.Generic;
using UnityEngine;

public class BeatSpawner : MonoBehaviour
{
    public GameObject beatPrefab; 
    public Transform spawnPoint; 
    public Transform ParentTransform;

    private void Start()
    {
        SpawnBeat();
    }
    public void SpawnBeat()
    {
        if (beatPrefab != null && spawnPoint != null)
        {
            Instantiate(beatPrefab, spawnPoint.position, Quaternion.identity, ParentTransform);
        }
        else
        {
            Debug.LogWarning("BeatPrefab ou SpawnPoint non assigné !");
        }
    }
}
