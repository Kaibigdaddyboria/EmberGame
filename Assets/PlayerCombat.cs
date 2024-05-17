using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public Animator animator;
    public Transform AttackPoint;
    public float AttackRange = 0.5f;
    public LayerMask EnemyLayer;
    public int attackDamage = 40;
    public float attackRate = 2f;
    float nextAttackTime = 0f;
    public Transform shootingPoint;
    public GameObject bulletPrefab;
    public int maxHealth = 100;
    int currentHealth;

    // Update is called once per frame
    void Update()
    {
        if (Time.time > nextAttackTime)
        {
            if (Input.GetKeyDown("c"))
            {
                Attack();
                nextAttackTime = Time.time + 1f / attackRate;
            }
        }
        if (Time.time > nextAttackTime)
        {
            if (Input.GetKeyDown("k"))
            {
                RangedAttack();
                nextAttackTime = Time.time + 1f / attackRate;
            }
        }
    }

    void Attack()
    {
        animator.SetTrigger("Attack");
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(AttackPoint.position, AttackRange, EnemyLayer);
        foreach(Collider2D enemy in  hitEnemies) 
        { 
            enemy.GetComponent<Enemy>().TakeDamage(attackDamage);
        }
    }
    void RangedAttack()
    {
        GameObject FireBall = Instantiate(bulletPrefab, shootingPoint.position, transform.rotation);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        animator.SetTrigger("Hurt");
        if (currentHealth <= 0)
        {
            Die();
        }
    }
    void Die()
    {
        Debug.Log("PlayerDied");
        animator.SetBool("Death", true);
        GetComponent<Collider>().enabled = false;
        this.enabled = false;
    }
}
