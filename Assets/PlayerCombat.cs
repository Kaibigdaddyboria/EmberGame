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
    // Update is called once per frame
    void Update()
    {
        if(Time.time > nextAttackTime)
        {
            if (Input.GetKeyDown("c"))
            {
                Attack();
                nextAttackTime = Time.time + 1f/ attackRate;
            }
        }
        if (Input.GetKeyDown("k"))
        {
            RangedAttack();
        }
    }
    void RangedAttack()
    {
        GameObject FireBall = Instantiate(bulletPrefab, shootingPoint.position, transform.rotation);
        // fireball.getcomponent<projectile[script]>().direction = movedirection
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

    private void OnDrawGizmosSelected()
    {
        if (AttackPoint != null) 
            return;
        Gizmos.DrawWireSphere(AttackPoint.position, AttackRange);
    }
}
