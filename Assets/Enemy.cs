using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
// using static UnityEngine.RuleTile.TilingRuleOutput;

public class Enemy : MonoBehaviour
{
    public float delay = 0f;
    public Animator animator;
    public int maxHealth = 100;
    int currentHealth;
    public float speed = 1.0f;
    public ProximitySensorScript pss;
    public AttackSensorScript ass;
    bool isAttacking = false;
    public Transform AttackPoint;
    public float AttackRange = 0.5f;
    public LayerMask PlayerLayer;
    public int attackDamage = 40;
    bool isalive = true;
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    private void Update()
    {
        if (isalive == true) 
        {
            if (pss.InRange && !ass.InAttackRange)
            {
                Vector3 Playerposition = new Vector3(pss.playerx, transform.position.y);
                Vector3 enemyPosition = transform.position;
                Vector3 direction = Playerposition - enemyPosition;
                print(direction);
                if (direction.x > 0)
                {
                    transform.Rotate(0, 0, 0);
                }
                else if (direction.x < 0)
                {
                    transform.Rotate(0, 180, 0);
                }
                else 
                {
                    transform.Rotate(0, 180, 0);
                }
                direction = Vector3.Normalize(direction);
                float deltaTime = Time.deltaTime;
                Vector3 newPosition = enemyPosition + direction * speed * deltaTime;
                transform.position = newPosition;
            }
            if (ass.InAttackRange && isAttacking == false)
            {
                isAttacking = true;
                Invoke("ResetAttack", 1f);
                Invoke("Attack", 0);
                delay = 0f;
            }
        }
    }

    private void ResetAttack()
    {
        isAttacking = false;
    }
    void Attack()
    {
        print("delayed");
        animator.SetTrigger("Attack");
        Invoke("DelayedAttack", 0.5f);

    }
    void DelayedAttack()
    {
        
        Collider2D[] hitPlayer = Physics2D.OverlapCircleAll(AttackPoint.position, AttackRange, PlayerLayer);
        foreach (Collider2D Player in hitPlayer)
        {
            print(Player.gameObject.name);
            Player.GetComponent<PlayerCombat>().TakeDamage(attackDamage);
            // enemy.GetComponent<Enemy>().TakeDamage(attackDamage);
        }
    }
    public void TakeDamage(int damage) 
    {
        delay = 1f;
        currentHealth -= damage;
        animator.SetTrigger("Hurt");
        if(currentHealth <= 0)
        {
            isalive = false;
            animator.SetBool("Death", true);
            Invoke("Die", 1f);
        }
    }

    void Die()
    {
        Destroy(gameObject);       
    }

}
