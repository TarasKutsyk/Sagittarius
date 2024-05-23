using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] List<WaveConfig> waveConfigs;
    [SerializeField] int startingWave;

    IEnumerator Start()
    {
        startingWave = GenerateWaveIndex(0);
        yield return StartCoroutine(SpawnAllWaves());
    }

    IEnumerator SpawnAllWaves()
    {
        for (int waveIndex = startingWave;; waveIndex = GenerateWaveIndex(waveIndex)) //infinite loop
        {
            yield return StartCoroutine(SpawnAllEnemiesInWave(waveConfigs[waveIndex]));
        }
    }

    int GenerateWaveIndex(int prevIndex)
    {
        if (waveConfigs.Count > 1)
        {
            do
            {
                int retIndex = UnityEngine.Random.Range(0, waveConfigs.Count);
                if (retIndex != prevIndex)
                    return retIndex;
            } while (true);
        }
        else return prevIndex;
    }

    private IEnumerator SpawnAllEnemiesInWave(WaveConfig currentWave)
    {
        for (int i = 0; i < currentWave.GetEnemiesNumber; i++)
        {
            var newEnemy = Instantiate(currentWave.GetEnemyPrefab, currentWave.GetWaypoints()[0].position, Quaternion.identity);
            newEnemy.GetComponent<EnemyPathing>().WaveConfig = currentWave;

            yield return new WaitForSeconds(currentWave.GetSpawnInterval);
        }
        yield return new WaitForSeconds(currentWave.WaveInterval);
    }
}


