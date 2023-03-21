using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    public List<GameObject> enemyPrefabs; // list of enemy prefabs
    public List<Transform> spawnPoints; // list of spawn point game objects
    public float timeBetweenWaves; // time between waves in seconds
    public int enemiesPerWave; // number of enemies to spawn per wave

    private bool isWaveOngoing; // flag to check if a wave is ongoing

    void Start()
    {
        isWaveOngoing = false; // set the flag to false at the beginning
    }

    void Update()
    {
        // check if the wave is ongoing and there are no more enemies left
        if (isWaveOngoing && GameObject.FindGameObjectsWithTag("Enemy").Length == 0)
        {
            isWaveOngoing = false; // set the flag to false to allow the next wave to start
            StartCoroutine(SpawnWaves()); // start the coroutine to spawn the next wave
        }
    }

    IEnumerator SpawnWaves()
    {
        yield return new WaitForSeconds(timeBetweenWaves); // wait for the time between waves

        isWaveOngoing = true; // set the flag to true to prevent starting new waves

        for (int i = 0; i < enemiesPerWave; i++)
        {
            int randomIndex = Random.Range(0, enemyPrefabs.Count); // choose a random enemy prefab from the list
            GameObject newEnemy = Instantiate(enemyPrefabs[randomIndex], spawnPoints[i % spawnPoints.Count].position, Quaternion.identity); // spawn the enemy at a random spawn point
            // add any other enemy properties as needed
        }
    }
}
