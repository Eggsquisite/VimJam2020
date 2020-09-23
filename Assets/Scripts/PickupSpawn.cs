using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupSpawn : MonoBehaviour
{
    [Header("Spawn Frequency")]
    public float minSpawnTime, maxSpawnTime, bubblesDuration, pickupDuration;
    private float spawnTimer, spawnTime;
    private bool spawnReady;

    [Header("Pickup Lists")]
    public List<Transform> pickupLocations;
    public List<GameObject> pickupPrefabs;
    public GameObject bubbles;
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
        var tmpPickup = Instantiate(bubbles, pickupLocations[temp].position, Quaternion.identity);

        pickups.Add(tmpPickup);
        StartCoroutine(CreatePickup(temp));
    }

    IEnumerator CreatePickup(int temp)
    {
        yield return new WaitForSeconds(bubblesDuration);
        if (pickups != null)
        {
            Destroy(pickups[0]);
            Debug.Log("pickups[0]: " + pickups[0]);
            pickups.RemoveAt(0);

            var tmpPickup = Instantiate(pickupPrefabs[Random.Range(0, pickupPrefabs.Count)], pickupLocations[temp].position, Quaternion.identity);
            pickups.Add(tmpPickup);
            StartCoroutine(DeletePickup());
        }
    }

    IEnumerator DeletePickup()
    {
        yield return new WaitForSeconds(pickupDuration);
        if (pickups != null)
        {
            Destroy(pickups[0]);
            pickups.RemoveAt(0);
        }
    }
}
