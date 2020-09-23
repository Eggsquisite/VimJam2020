using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;

    public Vector3 offset;
    public LayerMask enemyLayer;
    public float moveSpeed, checkDistance;

    private GameObject currentWaypoint, setWaypoint;
    private RaycastHit2D enemyInSight;
    private bool waypointSet, readyToAttack, facingRight;
    private float oldPos;

    // Start is called before the first frame update
    void Start()
    {
        if (rb == null) rb = GetComponent<Rigidbody2D>();
        oldPos = transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        //if (readyToAttack)
            CheckEnemy();

        Flip();
    }

    private void CheckEnemy()
    {
        if (facingRight)
        {
            enemyInSight = Physics2D.Raycast(transform.position, Vector2.right, checkDistance, enemyLayer);
            Debug.DrawLine(transform.position, (Vector2)transform.position + Vector2.right * checkDistance, Color.red);
        }
        else
        {
            enemyInSight = Physics2D.Raycast(transform.position, Vector2.left, checkDistance, enemyLayer);
            Debug.DrawLine(transform.position, (Vector2)transform.position + Vector2.left * checkDistance, Color.red);
        }

        if (enemyInSight.collider != null)
        {
            Debug.Log("Hit enemy: " + enemyInSight.collider.name);
        }
    }

    private void Flip()
    {
        if (oldPos < transform.position.x && !facingRight)
        {
            // facing/moving right
            Debug.Log("moving right");
            facingRight = true;
            transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
        }
        else if (oldPos > transform.position.x && facingRight)
        {
            // facing/moving left
            Debug.Log("moving left");
            facingRight = false;
            transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
        }

        oldPos = transform.position.x;
    }

    private void FixedUpdate()
    {
        MoveToWaypoint();
    }

    void MoveToWaypoint()
    {
        if (currentWaypoint != null && waypointSet)
        {
            waypointSet = false;
            setWaypoint = currentWaypoint;
            currentWaypoint = null;
        }

        if (setWaypoint != null)
        {
            Movement();
            readyToAttack = false;

            if (transform.position.x == setWaypoint.transform.position.x)
            {
                setWaypoint = null;
                readyToAttack = true;
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
