using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Components")]
    public Transform attackPoint;
    public Vector2 destination;
    private Animator anim;
    private Rigidbody2D rb;
    private AudioSource audioSource;

    [Header("Combat")]
    public LayerMask player;
    public GameObject explosion;
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
    public float minMoveSpeed, maxMoveSpeed;
    private float moveSpeed;
    private bool endGame;

    [Header("SFX")]
    public AudioClip deathSFX;

    private void OnEnable()
    {
        End.OnEnd += EndGame;
        Player.DestroyAll += Explode;
    }

    private void OnDisable()
    {
        End.OnEnd -= EndGame;
        Player.DestroyAll -= Explode;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (rb == null) rb = GetComponent<Rigidbody2D>();
        if (anim == null) anim = GetComponent<Animator>();
        if (audioSource == null) audioSource = Camera.main.GetComponent<AudioSource>();

        //moveSpeed = Random.Range(minMoveSpeed, maxMoveSpeed);
        currentHealth = maxHealth;
        oldPos = transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        Flip();
        //CheckPlayer();
    }

    private void FixedUpdate()
    {
        if (!stopMovement)
            Move();
    }

    private void Move()
    {
        var tmp = new Vector2(destination.x, transform.position.y);
        rb.MovePosition(Vector2.MoveTowards(transform.position, tmp, moveSpeed * Time.deltaTime));
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
        stopMovement = false;
    }

    private void Attacking()
    {
        Collider2D[] hit = Physics2D.OverlapCircleAll(attackPoint.position, attackRadius, player);

        foreach (Collider2D player in hit)
        {
            player.GetComponent<Player>().TakeDamage(damageValue, true);
        }

        Invoke("AttackCooldown", attackCooldown);
    }

    public void TakeDamage(int dmg)
    {
        currentHealth -= dmg;

        if (currentHealth <= 0)
            Die();
    }

    public void UpdateSpeed(float newVal)
    {
        minMoveSpeed += newVal;
        maxMoveSpeed += newVal;
        moveSpeed = Random.Range(minMoveSpeed, maxMoveSpeed);
    }

    private void Die()
    {
        audioSource.PlayOneShot(deathSFX);
        Destroy(gameObject);
    }

    private void EndGame()
    {
        Destroy(gameObject);
    }

    private void Explode()
    {
        Instantiate(explosion, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "EndGoal")
        {
            collision.GetComponent<End>().EndGame();
            Die();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.grey;
        Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
    }
}
