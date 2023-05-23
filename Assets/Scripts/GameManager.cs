using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public PowerupSpawner powerupSpawner;

    public List<Transform> spawnLocations = new List<Transform>();

    public Vector3 GetRandomSpawnLocation() {
        return spawnLocations[Random.Range(0, spawnLocations.Count)].position;
    }
}
