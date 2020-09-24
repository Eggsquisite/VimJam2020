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
    public Transform attackPoint;
    private RaycastHit2D enemyInSight;
    private bool attackReady = true, attacking;
    public float checkDistance, attackCooldown;

    [Header("Movement")]
    private GameObject currentWaypoint;
    private Vector3 setWaypoint;
    private float oldPos;
    private bool waypointSet, facingRight = true;
    public float moveSpeed;

    [Header("Health Management")]
    public int maxHealth;
    private int currentHealth;
    private bool inHealthRange = false;

    [Header("Adjustable Stats")]
    public float bonusMoveSpeed;
    public float bonusAttackSpeed;
    public int bonusHealth;
    public float bonusDownTime;

    // Start is called before the first frame update
    void Start()
    {
        if (rb == null) rb = GetComponent<Rigidbody2D>();
        if (anim == null) anim = GetComponent<Animator>();

        currentHealth = maxHealth;
        oldPos = transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        if (!inHealthRange)
            currentHealth -= 1;

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

            if (attackReady)
            {
                attacking = true;
                attackReady = false;

                //anim.SetBool("attackReady", false);
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
        //anim.SetBool("attackReady", true);
        Debug.Log("Attack ready...");
    }

    private void Flip()
    {
        if (oldPos < transform.position.x && !facingRight)
        {
            // facing/moving right
            facingRight = true;
            transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
        }
        else if (oldPos > transform.position.x && facingRight)
        {
            // facing/moving left
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
            setWaypoint = currentWaypoint.transform.position;
            currentWaypoint = null;
        }

        if (setWaypoint != Vector3.zero)
        {
            Movement();
            anim.SetFloat("movement", 1f);
            if (facingRight)
            {
                if (transform.position.x >= setWaypoint.x - 0.2f)
                {
                    setWaypoint = Vector3.zero;
                    anim.SetFloat("movement", 0f);
                }
            }
            else
            {
                if (transform.position.x <= setWaypoint.x + 0.2f)
                {
                    setWaypoint = Vector3.zero;
                    anim.SetFloat("movement", 0f);
                }
            }
        }
    }

    void Movement()
    {
        rb.MovePosition(Vector2.MoveTowards(transform.position, setWaypoint, (moveSpeed + bonusMoveSpeed) * Time.deltaTime));
    }

    public void UpdateWaypoint(GameObject newWaypoint)
    {
        waypointSet = true;
        currentWaypoint = newWaypoint;
    }

    public void PickupBonus(float moveSpeed, float attackSpeed, int health)
    {
        bonusMoveSpeed += moveSpeed;
        bonusAttackSpeed += attackSpeed;
        bonusHealth = health;

        UpdateStats();
    }

    void UpdateStats()
    {
        maxHealth += bonusHealth;
        anim.SetFloat("attackSpeed", bonusAttackSpeed);
    }
}
