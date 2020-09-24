using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Components")]
    public Transform attackPoint;
    private Animator anim;

    [Header("Combat")]
    public LayerMask player;
    public float attackRadius;
    public float checkDistance;
    public float attackCooldown;
    private RaycastHit2D playerInSight;
    private bool facingRight, stopMovement, attackReady = true;

    [Header("Stats")]
    public int maxHealth;
    public int damageValue;
    private int currentHealth;

    [Header("Movement")]
    private float oldPos;

    // Start is called before the first frame update
    void Start()
    {
        if (anim == null) anim = GetComponent<Animator>();
        currentHealth = maxHealth;
        oldPos = transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        Flip();
        CheckPlayer();
    }

    private void Flip()
    {
        if (oldPos < transform.position.x && !facingRight)
        {
            // facing/moving left
            facingRight = true;
            transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
        }
        else if (oldPos > transform.position.x && facingRight)
        {
            // facing/moving right
            facingRight = false;
            transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
        }

        oldPos = transform.position.x;
    }

    void CheckPlayer()
    {
        if (facingRight)
        {
            playerInSight = Physics2D.Raycast(transform.position, Vector2.right, checkDistance, player);
            Debug.DrawLine(transform.position, (Vector2)transform.position + Vector2.right * checkDistance, Color.red);
        }
        else
        {
            playerInSight = Physics2D.Raycast(transform.position, Vector2.left, checkDistance, player);
            Debug.DrawLine(transform.position, (Vector2)transform.position + Vector2.left * checkDistance, Color.red);
        }

        if (playerInSight.collider != null && attackReady)
        { 
            anim.ResetTrigger("attack");
            anim.SetTrigger("attack");
            stopMovement = true;
            attackReady = false;
        }
    }

    void AttackCooldown()
    {
        attackReady = true;
    }

    private void Attacking()
    {
        Collider2D[] hitPlayer = Physics2D.OverlapCircleAll(attackPoint.position, attackRadius, player);

        foreach (Collider2D player in hitPlayer)
            player.GetComponent<Player>().TakeDamage(damageValue, true);

        Invoke("AttackCooldown", attackCooldown);
    }

    public void TakeDamage(int dmg)
    {
        currentHealth -= dmg;

        if (currentHealth <= 0)
            Die();
    }

    private void Die()
    {
        // play death sound
        // play death anim
        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.grey;
        Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
    }
}
