using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public PlayerMovement PM;
    public float speed;
    public Rigidbody2D rb;
    public float direction;
    public float rotation;
    // Start is called before the first frame update
    void Start()
    {
        PM = GameObject.Find("Player").GetComponent<PlayerMovement>();
        if(PM.rotation == 0)
        {
            direction = PM.horizontal + 1;
        }
        else if (PM.rotation  == -180)
        {
            direction = PM.horizontal - 1; 
        
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(direction * speed, rb.velocity.y);
    }
    private void OnTriggerEnter2D(Collider2D hitinfo)
    {
        Destroy(gameObject);
        Enemy enemy = hitinfo.GetComponent<Enemy>();
        if (enemy != null )
        {
            enemy.GetComponent<Enemy>().TakeDamage(40);
        }
    }
}
