using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Components")]
    private Rigidbody2D rb;
    private Animator anim;

    [Header("Combat")]
    public LayerMask enemyLayer;
    private RaycastHit2D enemyInSight;
    private bool readyToAttack, attackReady, attacking;
    public float checkDistance, attackCooldown;

    [Header("Movement")]
    private GameObject currentWaypoint, setWaypoint;
    private float oldPos;
    private bool waypointSet, facingRight;
    public float moveSpeed;


    // Start is called before the first frame update
    void Start()
    {
        if (rb == null) rb = GetComponent<Rigidbody2D>();
        if (anim == null) anim = GetComponent<Animator>();

        attackReady = true;
        oldPos = transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        //if (readyToAttack)
            CheckEnemy();

        Flip();
    }
    private void FixedUpdate()
    {
        if (!attacking)
            MoveToWaypoint();
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
            anim.SetBool("enemyInSight", true);
            Debug.Log("Hit enemy: " + enemyInSight.collider.name);

            if (attackReady)
            {
                attackReady = false;

                attacking = true;
                anim.ResetTrigger("attack");
                anim.SetTrigger("attack");
                Invoke("ResetAttack", attackCooldown);
            }
        }
        else
            anim.SetBool("enemyInSight", false);
    }

    private void ResetAttack()
    {
        attacking = false;
        attackReady = true;
        Debug.Log("Attack ready...");
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
        anim.SetFloat("movement", Mathf.Sign(rb.velocity.x));
    }

    public void UpdateWaypoint(GameObject newWaypoint)
    {
        waypointSet = true;
        currentWaypoint = newWaypoint;
    }
}
