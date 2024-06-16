using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerCombat : MonoBehaviour
{
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
    void Start()
    {
        currentHealth = maxHealth;
    }
    // Update is called once per frame
    void Update()
    {
        healthBar.fillAmount = Mathf.Clamp(currentHealth / maxHealth, 0, 1);
        if (Time.time > nextAttackTime)
        {
            if (Input.GetKeyDown("c"))
            {
                rb.velocity = new Vector2(0,0);
                isAttacking = true;
                Invoke(nameof(StopAttacking), 0.7f);
                nextAttackTime = Time.time + 1f / attackRate;
                print("cant spam");
                Attack();
            }
        }
        if (Time.time > nextAttackTime)
        {
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

    void Attack()
    {
        animator.SetTrigger("Attack");
        Invoke("DelayedAttack", 0.5f);
    }
    void StopAttacking()
    {
        isAttacking = false;
    }
    void NotDamaged()
    {
        isDamaged = false;
    }
    void DelayedAttack()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(AttackPoint.position, radius, EnemyLayer);
        foreach (Collider2D enemy in hitEnemies)
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
        rb.velocity = new Vector2(0, 0);
        isDamaged = true;
        Invoke(nameof(NotDamaged), 0.5f);
        currentHealth -= damage;
        animator.SetTrigger("Hurt");
        print(currentHealth);
        if (currentHealth <= 0)
        {
            Die();
        }
    }
    void Die()
    {
        animator.SetBool("Death", true);
        Invoke("LoadScene", 1f);
    }
    void LoadScene()
    {
        SceneManager.LoadSceneAsync(0);
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(AttackPoint.transform.position, radius);
    }
}
