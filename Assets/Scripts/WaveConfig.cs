using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// the name of our new object we can create via project bar
[CreateAssetMenu(menuName = "Enemy Wave Config")]
public class WaveConfig : ScriptableObject
{
    // all the fields that a wave configuration should hold
    [SerializeField] GameObject enemyPrefab = default;
    [SerializeField] GameObject pathPrefab = default;
    [SerializeField] float timeBetweenSpawns = 0.5f;
    [SerializeField] float spawnRandomFactor = 0.3f;
    [SerializeField] int numberOfEnemies = 8;
    [SerializeField] float moveSpeed = 2f;

    // getters for the fields so other scripts can acess the data
    public GameObject GetEnemyPrefab() {return enemyPrefab; }

    public List<Transform> GetWaypoints()
    {
        // creating a list of transform to hold the waypoints transform information
        var waveWaypoints = new List<Transform>();

        //creating a foreach loop to feed our list with the waypoints from our path
        foreach (Transform childWaypoint in pathPrefab.transform)
        {
            waveWaypoints.Add(childWaypoint);
        }

        return waveWaypoints;
    }

    public float GetTimeBetweenSpawns() { return timeBetweenSpawns; }

    public float GetSpawnRandomFactor() { return spawnRandomFactor; }

    public int GetNumberOfEnemies() { return numberOfEnemies; }

    public float GetMoveSpeed() { return moveSpeed; }
}
