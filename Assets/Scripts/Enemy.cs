using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
// using static UnityEngine.RuleTile.TilingRuleOutput;

public class Enemy : MonoBehaviour
{
    public Animator animator;
    public int maxHealth;
    public int currentHealth;
    public float speed;
    public ProximitySensorScript pss;
    public AttackSensorScript ass;
    bool isAttacking = false;
    public Transform AttackPoint;
    public float radius;
    public LayerMask PlayerLayer;
    public int attackDamage;
    bool isalive = true;

    [SerializeField] private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        // Set current helth to maxhealth when the game starts
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    private void Update()
    {
        // Check if the enemy is alive
        if (isalive == true)
        {
            // Make sure the current health is not above the maximum health
            if (currentHealth > 100)
            {
                currentHealth = 100;
            }

            // Check if the player is in range but not in attack range
            if (pss.InRange && !ass.InAttackRange)
            {
                // Calculate the direction towards the player
                animator.SetInteger("AnimState", 2);
                Vector3 Playerposition = new Vector3(pss.playerx, transform.position.y);
                Vector3 enemyPosition = transform.position;
                Vector3 direction = Playerposition - enemyPosition;
                direction = Vector3.Normalize(direction);
                print(direction.x);
                // Rotate the enemy to face the player
                if (direction.x > 0)
                {
                    transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                }
                else if (direction.x < 0)
                {
                    transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
                }

                // Move the enemy towards the player
                float deltaTime = Time.deltaTime;
                Vector3 newPosition = enemyPosition + direction * speed * deltaTime;
                transform.position = newPosition;
            }

            // Check if the player is in attack range and the enemy is not already attacking
            if (ass.InAttackRange && isAttacking == false)
            {
                isAttacking = true;
                Invoke("Attack", 1f);
                Invoke("ResetAttack", 2f);
            }
        }
    }

    // Resets the attack state
    private void ResetAttack()
    {
        isAttacking = false;
    }

    // Initiates the attack animation and invokes the DelayedAttack method
    void Attack()
    {
        animator.SetTrigger("Attack");
        Invoke("DelayedAttack", 0.5f);
    }

    // Deals damage to the player after a delay
    void DelayedAttack()
    {
        Collider2D[] hitPlayer = Physics2D.OverlapCircleAll(AttackPoint.position, radius, PlayerLayer);
        // Deal damage to each player detected
        foreach (Collider2D Player in hitPlayer)
        {
            Player.GetComponent<PlayerCombat>().TakeDamage(attackDamage);
        }
    }

    // Reduces the enemy's health and handles death
    public void TakeDamage(int damage)
    {
        print("Enemy hit");
        currentHealth -= damage;
        animator.SetTrigger("Hurt");

        // Check if the enemy's health is below or equal to 0
        if (currentHealth <= 0)
        {
            isalive = false;
            animator.SetBool("Death", true);
            Invoke("Die", 1f);
        }
    }

    // Handles collision with the player
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerCombat playerCombat = collision.gameObject.GetComponent<PlayerCombat>();

            // Deal damage to the player on collision
            if (playerCombat != null)
            {
                playerCombat.TakeDamage(attackDamage);
            }

            // Stop the enemy's movement on collision
            rb.velocity = new Vector2(0, 0);
            transform.position = rb.position;
        }
    }

    // Kills the enemy
    void Die()
    {
        Destroy(gameObject);
    }

    // Draws the attack radius in the editor
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(AttackPoint.transform.position, radius);
    }
}