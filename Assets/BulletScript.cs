using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public float speed;
    public Rigidbody2D rb;
    private bool facingRight = true;
    // Start is called before the first frame update
    void Start()
    {

    }
    
    // Update is called once per frame
    void Update()
    {
        float move = 

        if (move > 0 && !facingRight)
        {
            Flip();
            rb = GetComponent<Rigidbody2D>();
            rb.velocity = transform.right * speed;
        }
        else if (move < 0 && facingRight)
        {
            Flip();
            rb = GetComponent<Rigidbody2D>();
            rb.velocity = transform.right * -speed;
        }
    }
    void Flip()
    {
        facingRight = !facingRight;
        Vector2 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
