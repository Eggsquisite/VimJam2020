using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public ClickManager cm;
    private Rigidbody2D rb;

    public float moveSpeed;
    public float maxSpeed;

    private bool waypointSet = false;
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
        currentWaypoint = cm.GetWaypoint();
        if (currentWaypoint != null && !waypointSet)
        {
            waypointSet = true;
            setWaypoint = currentWaypoint;
            currentWaypoint = null;
        }

        if (setWaypoint != null && waypointSet)
        {
            // move player
            Debug.Log(setWaypoint.name + " Set waypoint");
            if (setWaypoint.transform.position.x < transform.position.x)
            {
                Movement(-1);
            }
            else if (setWaypoint.transform.position.x > transform.position.x)
            {
                Movement(1);
            }

            if (transform.position == setWaypoint.transform.position)
            {
                setWaypoint = null;
                waypointSet = false;
            }
        }
    }

    void Movement(float direction)
    {
        rb.AddForce(Vector2.right * direction * moveSpeed);

        if (Mathf.Abs(rb.velocity.x) > maxSpeed)
            rb.velocity = new Vector2(Mathf.Sign(rb.velocity.x) * maxSpeed, rb.velocity.y);
    }
}
