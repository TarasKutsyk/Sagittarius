using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Enemy Wave Config")]
public class WaveConfig : ScriptableObject  
{
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] GameObject pathPrefab;
    [SerializeField] float spawnInterval = 0.5f;
    [SerializeField] float spawnRandomFactor = 0.3f;
    [SerializeField] int enemiesNumber = 5;
    [SerializeField] float moveSpeed = 3f;
    [SerializeField] float waveInterval = 2f;

    public GameObject GetEnemyPrefab { get => enemyPrefab; }
    public List<Transform> GetWaypoints()
    {
        var waveWaypoints = new List<Transform>();
        foreach (Transform child in pathPrefab.transform)
        {
            waveWaypoints.Add(child);
        }
        return waveWaypoints;
    }
    public float GetSpawnInterval { get => spawnInterval; }
    public float GetSpawnRandomFactor { get => spawnRandomFactor; }
    public int GetEnemiesNumber { get => enemiesNumber;  }
    public float GetMoveSpeed { get => moveSpeed;  }
    public float WaveInterval { get => waveInterval; set => waveInterval = value; }
}
