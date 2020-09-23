using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public ClickManager cm;
    private Rigidbody2D rb;

    public float moveSpeed;
    public float maxSpeed;
    public Vector3 offset;

    private bool waypointSet = false, movingPlayer = false;
    private GameObject currentWaypoint, setWaypoint;

    // Start is called before the first frame update
    void Start()
    {
        if (rb == null) rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        //currentWaypoint = cm.GetWaypoint();
        MoveToWaypoint();
    }

    void MoveToWaypoint()
    {
        if (currentWaypoint != null && waypointSet)
        {
            waypointSet = false;
            movingPlayer = true;
            setWaypoint = currentWaypoint;
            currentWaypoint = null;
        }

        if (setWaypoint != null && movingPlayer)
        {
            Debug.Log(setWaypoint.name + " Set waypoint");
            Movement();

            if (transform.position.x == setWaypoint.transform.position.x)
            {
                Debug.Log("Waypoint reached");
                setWaypoint = null;
                movingPlayer = false;
            }
        }
    }

    void Movement()
    {
        rb.MovePosition(Vector2.MoveTowards(transform.position, setWaypoint.transform.position, moveSpeed * Time.deltaTime));
    }

    public void UpdateWaypoint(GameObject newWaypoint)
    {
        waypointSet = true;
        currentWaypoint = newWaypoint;
    }
}
