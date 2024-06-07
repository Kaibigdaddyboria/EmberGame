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
    bool isStaggered = false;
    public float knockbackForce = 5f; // Force of the knockback
    [SerializeField] private Rigidbody2D rb; // Reference to the Rigidbody2D component
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    private void Update()
    {
        if (isalive == true && !isStaggered) // Check if the enemy is not staggere
        {
            if (pss.InRange && !ass.InAttackRange)
            {
                Vector3 Playerposition = new Vector3(pss.playerx, transform.position.y);
                Vector3 enemyPosition = transform.position;
                Vector3 direction = Playerposition - enemyPosition;
                direction = Vector3.Normalize(direction);
                print(direction.x);
                if (direction.x > 0)
                {
                    transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                }
                else if (direction.x < 0)
                {
                    transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
                }
                float deltaTime = Time.deltaTime;
                Vector3 newPosition = enemyPosition + direction * speed * deltaTime;
                transform.position = newPosition;
            }
            //Attack System: If the player is within a certain range of the enemy the enemy should move toward the player.
            //When the enemy is close enough to the player the enemy should pause for a second, then attck the player 
            //The enemy should continue to attack with 1 second inbetween until the player moves out of the enemies attack range.
            //If the enemy is hit it should cause a stagger where the enemy can't attack for a couple seconds and is knocked backwards.
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
        else
        {
            StartCoroutine(Stagger()); // Start the stagger coroutine
        }
    }
    void Die()
    {
        Destroy(gameObject);       
    }
    IEnumerator Stagger()
    {
        isStaggered = true; // Set staggered state to true

        // Apply knockback force
        Vector2 knockbackDirection = (transform.position).normalized;
        rb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);

        yield return new WaitForSeconds(1f); // Wait for 1 second

        isStaggered = false; // Reset staggered state

    }
}
