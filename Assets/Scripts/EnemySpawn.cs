using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    [Header("Spawn Frequency")]
    public float minSpawnTime, maxSpawnTime;
    private float spawnTimer, spawnTime;

    [Header("Pickup Lists")]
    public List<GameObject> enemy;
    public List<Transform> pickupLocations;

    // Start is called before the first frame update
    void Start()
    {
        ResetSpawnTime();
    }

    // Update is called once per frame
    void Update()
    {
        if (spawnTimer < spawnTime)
            spawnTimer += Time.deltaTime;
        else if (spawnTimer >= spawnTime)
        {
            spawnTimer = 0;
            SpawnEnemy();
            ResetSpawnTime();
        }
    }

    void ResetSpawnTime()
    {
        spawnTime = Random.Range(minSpawnTime, maxSpawnTime);
    }

    void SpawnEnemy()
    {
        int tempDest = Random.Range(0, pickupLocations.Count);
        int tempEnemy = Random.Range(0, enemy.Count);
        Instantiate(enemy[tempEnemy], pickupLocations[tempDest].position, Quaternion.identity);
    }
}
