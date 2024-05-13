using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
// using static UnityEngine.RuleTile.TilingRuleOutput;

public class Enemy : MonoBehaviour
{
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
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    private void Update()
    {
        print(ass.InAttackRange);
        if (pss.InRange && !ass.InAttackRange)
        {
            Vector3 Playerposition = new Vector3(pss.playerx, transform.position.y);
            Vector3 enemyPosition = transform.position;
            Vector3 direction = Playerposition - enemyPosition;
            direction = Vector3.Normalize(direction);
            float deltaTime = Time.deltaTime;
            Vector3 newPosition = enemyPosition + direction * speed * deltaTime;           
            transform.position = newPosition;
            print(enemyPosition);
        }
        if (ass.InAttackRange && isAttacking == false)
        {
            isAttacking = true;
            Attack();
            Invoke("ResetAttck", 1f);
        }
    }

    private void ResetAttck()
    {
        isAttacking = false;
    }
    void Attack()
    {
        animator.SetTrigger("Attack");
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(AttackPoint.position, AttackRange, PlayerLayer);
        foreach (Collider2D player in hitEnemies)
        {
            player.GetComponent<Enemy>().TakeDamage(attackDamage);
        }
    }

    public void TakeDamage(int damage) 
    { 
        currentHealth -= damage;
        animator.SetTrigger("Hurt");
        if(currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("EnemyDied");
        animator.SetBool("Death", true);
        GetComponent<Collider>().enabled = false;
        this.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        
    }

    void Find()
    {
    }
}
