using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    [Header("Spawn Frequency")]
    public float minSpawnTime;
    public float maxSpawnTime;
    public float decreaseSpawnTime; 
    public float addedSpeed;
    public float upgradeFreq;
    private float spawnTimer, spawnTime;
    private bool endGame;
    private int maxUpgradeLimit = 5, numOfUpgrades;

    [Header("Pickup Lists")]
    public List<GameObject> enemy;
    public List<Transform> pickupLocations;

    private void OnEnable()
    {
        End.OnEnd += StopSpawn;
    }

    private void OnDisable()
    {
        End.OnEnd += StopSpawn;
    }

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("DecreaseSpawnTime", 0f, upgradeFreq);
        minSpawnTime += decreaseSpawnTime;
        maxSpawnTime += decreaseSpawnTime;

        ResetSpawnTime();
    }

    // Update is called once per frame
    void Update()
    {
        if (endGame)
            return; 

        if (spawnTimer < spawnTime)
            spawnTimer += Time.deltaTime;
        else if (spawnTimer >= spawnTime)
        {
            spawnTimer = 0;
            SpawnEnemy();
            ResetSpawnTime();
        }
    }

    void DecreaseSpawnTime()
    {
        numOfUpgrades++;
        if (numOfUpgrades >= maxUpgradeLimit)
            return;

        minSpawnTime -= decreaseSpawnTime;
        maxSpawnTime -= decreaseSpawnTime;

        if (minSpawnTime < 1f)
            minSpawnTime = 1f;

        if (maxSpawnTime < 3f)
            maxSpawnTime = 3f;
    }

    void ResetSpawnTime()
    {
        spawnTime = Random.Range(minSpawnTime, maxSpawnTime);
    }

    void SpawnEnemy()
    {
        int tempDest = Random.Range(0, pickupLocations.Count);
        int tempEnemy = Random.Range(0, enemy.Count);
        var tmp = Instantiate(enemy[tempEnemy], pickupLocations[tempDest].position, Quaternion.identity);
        tmp.GetComponent<Enemy>().UpdateSpeed(addedSpeed);
    }

    void StopSpawn()
    {
        endGame = true;
    }
}
