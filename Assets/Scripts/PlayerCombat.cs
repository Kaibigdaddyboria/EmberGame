using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerCombat : MonoBehaviour
{
    // Variables for combat and health
    public Animator animator;
    public Transform AttackPoint;
    public float radius; 
    public LayerMask EnemyLayer; 
    public int attackDamage; 
    public float attackRate; 
    float nextAttackTime; 
    public Transform shootingPoint; 
    public GameObject bulletPrefab; 
    public float maxHealth; 
    public float currentHealth; 
    public Image healthBar; 
    public bool isAttacking; 
    public bool isDamaged;

    [SerializeField] private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth; // Initialize current health to max health
    }

    // Update is called once per frame
    void Update()
    {
        // Update the health bar UI
        healthBar.fillAmount = Mathf.Clamp(currentHealth / maxHealth, 0, 1);

        // Prevents player from spaming attack
        if (Time.time > nextAttackTime)
        {
            // Handle melee attack
            if (Input.GetKeyDown("c"))
            {
                rb.velocity = new Vector2(0, 0);
                isAttacking = true;
                Invoke(nameof(StopAttacking), 0.7f);
                nextAttackTime = Time.time + 1f / attackRate;
                Attack();
            }
        }

        // Prevents player from spaming attack
        if (Time.time > nextAttackTime)
        {
            // Handle ranged attack
            if (Input.GetKeyDown("k"))
            {
                rb.velocity = new Vector2(0, 0); 
                isAttacking = true; 
                Invoke(nameof(StopAttacking), 0.7f);
                nextAttackTime = Time.time + 1f / attackRate;
                RangedAttack(); 
            }
        }
    }

    // Perform melee attack
    void Attack()
    {
        animator.SetTrigger("Attack"); 
        Invoke("DelayedAttack", 0.5f);
    }

    // Stop the attacking state
    void StopAttacking()
    {
        isAttacking = false; 
    }

    // Stop the damaged state
    void NotDamaged()
    {
        isDamaged = false; 
    }

    // Perform the actual melee attack after a delay
    void DelayedAttack()
    {
        // Detect enemies in the attack range
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(AttackPoint.position, radius, EnemyLayer);
        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<Enemy>().TakeDamage(attackDamage); 
        }
    }

    // Perform ranged attack
    void RangedAttack()
    {
        
        GameObject FireBall = Instantiate(bulletPrefab, shootingPoint.position, transform.rotation);
    }

    // Handle taking damage
    public void TakeDamage(int damage)
    {
        rb.velocity = new Vector2(0, 0); 
        isDamaged = true;
        Invoke(nameof(NotDamaged), 0.3f);
        currentHealth -= damage; 
        animator.SetTrigger("Hurt");
        if (currentHealth <= 0)
        {
            Die(); 
        }
    }

    // Handle player death
    void Die()
    {
        animator.SetBool("Death", true);
        Invoke("LoadScene", 1f); 
    }

    // Load the scene (restart the game)
    void LoadScene()
    {
        SceneManager.LoadSceneAsync(0); 
    }

    // Draw the attack range in the editor
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(AttackPoint.transform.position, radius);
    }
}