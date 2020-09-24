using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Components")]
    private Rigidbody2D rb;
    private Animator anim;
    public Healthbar healthbar;

    [Header("Combat")]
    public LayerMask enemyLayer;
    public Transform attackPoint;
    private RaycastHit2D enemyInSight;
    private bool attackReady = true, attacking;
    public float checkDistance, attackCooldown, attackRadius;

    [Header("Movement")]
    private GameObject currentWaypoint;
    private Vector3 setWaypoint;
    private float oldPos;
    private bool waypointSet, stopMovement, facingRight = true;
    public float moveSpeed;

    [Header("Health Management")]
    public int maxHealth;
    public int currentHealth;
    public int healthDecayValue, healthRegenValue;
    private bool inBaseRange = false, regenHealth, decayHealth;

    [Header("Adjustable Stats")]
    public float bonusMoveSpeed;
    public float attackSpeedMult;

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
        if (attacking)
            Attacking();

        HealthManagement();
        CheckEnemy();
        Flip();
        UpdateHealth();
    }
    private void FixedUpdate()
    {
        MoveToWaypoint();
    }

    private void HealthManagement()
    {
        if (!inBaseRange && !decayHealth && currentHealth > 0)
        {
            decayHealth = true;
            StartCoroutine(HealthDecay());
        }
        else if (inBaseRange && !regenHealth && currentHealth < maxHealth)
        {
            regenHealth = true;
            StartCoroutine(HealthRegen());
        }
    }

    IEnumerator HealthDecay()
    {
        currentHealth -= healthDecayValue;
        yield return new WaitForSeconds(1);
        decayHealth = false;
    }

    IEnumerator HealthRegen()
    {
        currentHealth += healthRegenValue;
        yield return new WaitForSeconds(1);
        regenHealth = false;
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
                attackReady = false;
                stopMovement = true;
                Invoke("ResetAttack", attackCooldown);
            }
        }
        else
            anim.SetBool("enemyInSight", false);
    }

    private void ResetAttack()
    {
        attackReady = true;
        stopMovement = false;
        //anim.SetBool("attackReady", true);
        //Debug.Log("Attack ready...");
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

    private void UpdateHealth()
    {
        healthbar.SetHealth(currentHealth, maxHealth);
    }


    void MoveToWaypoint()
    {
        if (stopMovement) 
            return;

        if (currentWaypoint != null && waypointSet)
        {
            waypointSet = false;
            setWaypoint = currentWaypoint.transform.position;
            currentWaypoint = null;
        }

        if (setWaypoint != Vector3.zero)
        {
            if (!stopMovement)
                Movement();

            WaypointCheck();
        }
    }

    void Movement()
    {
        anim.SetFloat("movement", 1f);
        var tmp = new Vector2(setWaypoint.x, transform.position.y);
        rb.MovePosition(Vector2.MoveTowards(transform.position, tmp, (moveSpeed + bonusMoveSpeed) * Time.deltaTime));
    }

    void WaypointCheck()
    {
        if (facingRight)
        {
            if (transform.position.x >= setWaypoint.x - 0.2f)
            {
                //Debug.Log("hit right dest: true == " + facingRight);
                StopMovement();
            }
        }
        else
        {
            if (transform.position.x <= setWaypoint.x + 0.2f)
            {
                //Debug.Log("hit left dest: false == " + facingRight);
                StopMovement();
            }
        }
    }

    void StopMovement()
    {
        setWaypoint = Vector3.zero;
        anim.SetFloat("movement", 0f);
    }

    public void UpdateWaypoint(GameObject newWaypoint)
    {
        waypointSet = true;
        currentWaypoint = newWaypoint;
    }

    public void PickupBonus(float moveSpeed, float attackSpeed, int health)
    {
        bonusMoveSpeed += moveSpeed;
        attackSpeedMult += attackSpeed;
        maxHealth += health;

        anim.SetFloat("attackSpeed", attackSpeedMult);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Base")
            inBaseRange = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Base")
            inBaseRange = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.grey;
        Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
    }

    private void AttackStatus(int status)
    {
        if (status > 0)
            attacking = true;
        else
            attacking = false;
    }

    private void Attacking()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRadius, enemyLayer);

        foreach (Collider2D enemy in hitEnemies)
            enemy.GetComponent<Enemy>().TakeDamage(100);
    }
}
