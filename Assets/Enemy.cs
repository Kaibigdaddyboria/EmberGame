using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Animator animator;
    public int maxHealth = 100;
    int currentHealth;
    public float speed = 1.0f;
    public ProximitySensorScript pss;
    public AttackSensorScript ass;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    private void Update()
    {
        if (pss.InRange)
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
        if (ass.InAttackRange)
        {
            
            animator.SetTrigger("Attack");
            ass.InAttackRange = false;
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
