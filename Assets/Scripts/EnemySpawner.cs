using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] List<WaveConfig> waveConfigs = default;
    [SerializeField] int startingWave = 0; // the starting wave of our list
    [SerializeField] bool looping = false;
    [SerializeField] float timeBetweenWaves = 3f;
    [SerializeField] bool levelComplete = false;


    // Start is called before the first frame update
    // change the start method to a ienumerator to use as a coroutine
    IEnumerator Start()
    {
        // here we going to loop all the waves we created until the looping variable is true
        // we can set that through the inspector
        do
        {
            // here we start the coroutine to spawn the waves we set
            yield return StartCoroutine(SpawnAllWaves());
        }
        while (looping);       
    }

    public bool LevelComplete()
    {
        return levelComplete;
    }

    // coroutine responsible for spawing all the waves we set through the inspector
    private IEnumerator SpawnAllWaves()
    {

        // a for loop to spawn every wave we put in the inspector of our script
        for (int waveIndex = startingWave; waveIndex < waveConfigs.Count; waveIndex++)
        {
            // set the current wave the for loop are
            // putting Random.Range we can random through the waves we created but for now
            // we going through the normal cycle
            var currentWave = waveConfigs[waveIndex];

            // pass the current to start the coroutine of spawning enemies then it will only
            // spawn another wave when the coroutine of spawn enemies finished
            // because we put that in the yield
            yield return StartCoroutine(SpawnAllEnemiesInWave(currentWave));
            
        }
        levelComplete = true;
    }

    // coroutine responsible for spawing multiple enemies
    private IEnumerator SpawnAllEnemiesInWave(WaveConfig waveConfig)
    {
        // a for loop to spawn all enemies defined in the wave config
        for (int enemyCount = 0; enemyCount < waveConfig.GetNumberOfEnemies(); enemyCount++)
        {
            var newEnemy = Instantiate(
           // get the enemy prefab from the wave config
           waveConfig.GetEnemyPrefab(),
           // get the waypoints position from the wave config
           waveConfig.GetWaypoints()[0].transform.position,
           Quaternion.identity);

            // here we set the wave config the enemy pathing will follow
            // the wave config will be equals the parameter we receive from the coroutine
            // the paremeter it's set to 0 to make only the first wave config, but
            // we change this later
            newEnemy.GetComponent<EnemyPathing>().SetWaveConfig(waveConfig);

            yield return new WaitForSeconds(waveConfig.GetTimeBetweenSpawns());
        }

        yield return new WaitForSeconds(Random.Range(0f, timeBetweenWaves));

    }

}
