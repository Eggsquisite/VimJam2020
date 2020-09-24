using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupSpawn : MonoBehaviour
{
    [Header("Spawn Frequency")]
    public float minSpawnTime, maxSpawnTime;
    private float spawnTimer, spawnTime;

    [Header("Pickup Lists")]
    public GameObject bubbles;
    public List<Transform> pickupLocations;
    private List<GameObject> pickups = new List<GameObject>();

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
            PickupHeadsup();
            ResetSpawnTime();
        }
    }

    void ResetSpawnTime()
    {
        spawnTime = Random.Range(minSpawnTime, maxSpawnTime);
    }

    void PickupHeadsup()
    {
        Debug.Log("Spawning...");

        int temp = Random.Range(0, pickupLocations.Count);
        Instantiate(bubbles, pickupLocations[temp].position, Quaternion.identity);
    }

}
