using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public float speed;
    public Rigidbody2D rb;
    public float direction = 1;
    public float rotation;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // Sets the bullet's velocity to move it in the right direction
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.right * speed;
    }

    // This function is called when the bullet collides with another object
    private void OnTriggerEnter2D(Collider2D hitinfo)
    {
        // Checks wthat the collided object is not the ProximitySensor, AttackSensor, or Player
        if (!hitinfo.CompareTag("ProximitySensor") && !hitinfo.CompareTag("AttackSensor") && !hitinfo.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
        else
        {

        }

        // Get the Enemy component from the collided object
        Enemy enemy = hitinfo.GetComponent<Enemy>();

        // Checks if the collided object is an Enemy
        if (enemy != null)
        {
            enemy.GetComponent<Enemy>().TakeDamage(40);
        }
    }
}