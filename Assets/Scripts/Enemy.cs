using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Components")]
    public Transform attackPoint;

    [Header("Combat")]
    private LayerMask player;
    public float attackRadius;

    [Header("Stats")]
    public int maxHealth;
    public int damageValue;
    private int currentHealth;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;   
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Attacking()
    {
        Collider2D[] hitPlayer = Physics2D.OverlapCircleAll(attackPoint.position, attackRadius, player);

        foreach (Collider2D player in hitPlayer)
            player.GetComponent<Player>().TakeDamage(damageValue, true);
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
}
