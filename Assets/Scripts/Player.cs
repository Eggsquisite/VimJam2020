using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Components")]
    private Rigidbody2D rb;
    private Animator anim;
    private Collider2D coll;
    public Healthbar healthbar;
    public PickupUI pickupUI;

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
    private bool endGame;

    [Header("Health Management")]
    public float maxHealth;
    public float currentHealth;
    public float healthDecayValue, healthRegenValue;
    public float deathMaxTime;
    private float deathTimer, invincibleTimer;
    private bool inBaseRange = false, regenHealth, decayHealth, dead, invincible;

    [Header("Adjustable Stats")]
    public float bonusMoveSpeed;
    public float attackSpeedMult;
    public float maxPickups;
    private int healthPickups, speedPickups, attackPickups;

    private void OnEnable()
    {
        End.OnEnd += EndGame;
    }

    private void OnDisable()
    {
        End.OnEnd -= EndGame;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (rb == null) rb = GetComponent<Rigidbody2D>();
        if (anim == null) anim = GetComponent<Animator>();
        if (coll == null) coll = GetComponent<Collider2D>();

        currentHealth = maxHealth;
        oldPos = transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        if (endGame)
            return;

        UpdateHealth();
        if (dead) {
            Resurrecting();
            return;
        }

        if (attacking)
            Attacking();

        if (invincible)
            InvincibleReset();

        HealthManagement();
        CheckEnemy();
        Flip();
    }
    private void FixedUpdate()
    {
        MoveToWaypoint();
    }

    private void Resurrecting()
    {
        if (currentHealth < maxHealth && !regenHealth)
        {
            regenHealth = true;
            StartCoroutine(HealthRegen(-deathMaxTime));
        }

        if (deathTimer < deathMaxTime)
            deathTimer += Time.deltaTime;
        else if (deathTimer >= deathMaxTime)
        {
            deathTimer = 0;
            dead = false;
            stopMovement = false;
            coll.enabled = true;
            currentHealth = maxHealth;
        }
    }

    private void InvincibleReset()
    {
        if (invincibleTimer < 1f)
            invincibleTimer += Time.deltaTime;
        else if (invincibleTimer >= 1f)
        {
            invincibleTimer = 0;
            invincible = false;
        }
    }

    private void HealthManagement()
    {
        if (!inBaseRange && !decayHealth && currentHealth > 6)
        {
            decayHealth = true;
            StartCoroutine(HealthDecay(healthDecayValue));
        }
        else if (inBaseRange && !regenHealth && currentHealth < maxHealth)
        {
            regenHealth = true;
            StartCoroutine(HealthRegen(-healthRegenValue));
        }
    }

    IEnumerator HealthDecay(float decay)
    {
        TakeDamage(decay, false);
        yield return new WaitForSeconds(.1f);
        decayHealth = false;
    }

    IEnumerator HealthRegen(float regen)
    {
        TakeDamage(regen - attackSpeedMult, false);
        yield return new WaitForSeconds(0.1f);
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
                //Invoke("ResetAttack", attackCooldown);
            }
        }
        else
            anim.SetBool("enemyInSight", false);
    }

    private void AttackFinished()
    {
        attackReady = true;
        stopMovement = false;
        anim.ResetTrigger("hit");
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
        if (currentHealth < 6)
            currentHealth = 6;

        healthbar.SetHealth(currentHealth, maxHealth);
    }


    void MoveToWaypoint()
    {

        if (currentWaypoint != null && waypointSet)
        {
            waypointSet = false;
            setWaypoint = currentWaypoint.transform.position;
            currentWaypoint = null;
        }

        if (stopMovement) 
            return;

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
        //setWaypoint = Vector3.zero;
        anim.SetFloat("movement", 0f);
    }

    public void UpdateWaypoint(GameObject newWaypoint)
    {
        waypointSet = true;
        currentWaypoint = newWaypoint;
    }

    public void PickupBonus(float moveSpeed, float attackSpeed, float health, int ID)
    {
        if (ID == 0)
            healthPickups++;
        else if (ID == 1)
            speedPickups++;
        else if (ID == 2)
            attackPickups++;

        if (healthPickups <= maxPickups) {
            maxHealth += health;
            currentHealth += health;
        }
        if (speedPickups <= maxPickups) {
            bonusMoveSpeed += moveSpeed;
        }
        if (attackPickups <= maxPickups) { 
            attackSpeedMult += attackSpeed;
        }

        //anim.SetFloat("attackSpeed", attackSpeedMult);
        pickupUI.UpdatePickups(ID);
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

    public void TakeDamage(float dmg, bool hitByEnemy)
    {
        if (invincible)
            return;

        invincible = true;
        currentHealth -= dmg;
        if (currentHealth <= 6)
            Death();
        else if (currentHealth > 6 && hitByEnemy)
        {
            anim.ResetTrigger("hit");
            anim.SetTrigger("hit");
        }
    }

    private void Death()
    {
        StopMovement();
        stopMovement = true;
        coll.enabled = false;
        anim.ResetTrigger("dead");
        anim.SetTrigger("dead");
    }

    private void DeathReset()
    {
        dead = true;
    }

    private void EndGame()
    {
        Death();
        endGame = true;
        //anim.ResetTrigger("dead");
        anim.SetBool("endGame", true);
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
}
