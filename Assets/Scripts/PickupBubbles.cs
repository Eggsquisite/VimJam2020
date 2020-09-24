using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupBubbles : MonoBehaviour
{
    public List<GameObject> pickupPrefabs;

    [Header("Components")]
    private Collider2D coll;
    private SpriteRenderer sp;

    [Header("Duration")]
    public float bubblesDuration;
    private int rand;
    private GameObject tempObject;

    // Start is called before the first frame update
    void Start()
    {
        if (coll == null) coll = GetComponent<Collider2D>();
        if (sp == null) sp = GetComponent<SpriteRenderer>();

        sp.enabled = true;
        coll.enabled = true;
        rand = Random.Range(0, pickupPrefabs.Count);
        StartCoroutine(ReplaceBubbles());
    }

    IEnumerator ReplaceBubbles()
    {
        yield return new WaitForSeconds(bubblesDuration);
        if (pickupPrefabs != null)
        {
            sp.enabled = false;
            coll.enabled = false;

            tempObject = Instantiate(pickupPrefabs[rand], transform.position, Quaternion.identity);
            tempObject.transform.localScale = new Vector3(1, 1, 1);

            Destroy(gameObject);
            //StartCoroutine(DeletePickup());
        }
    }
}
