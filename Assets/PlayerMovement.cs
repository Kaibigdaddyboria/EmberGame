using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerMovement : MonoBehaviour
{
    public float horizontal;
    public float rotation;
    private float speed = 8f;
    private float jumpingPower = 16f;
    private bool isFacingRight = true;
    public Animator m_animator;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private TrailRenderer tr;
    private bool CanDash = true;
    private bool isDashing;
    private float dashingPower = 24f;
    private float dashingTime = 0.2f;
    private float dashingCooldown = 1f;

    // Start is called before the first frame update
    void Update()
    {
        if(isDashing)
        {
            return;
        }
        Flip();
        bool grounded = IsGrounded();
        horizontal = Input.GetAxisRaw("Horizontal");
        rotation = transform.rotation.y;
        if(Input.GetButtonDown("Jump") && grounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
            grounded = false;
            m_animator.SetTrigger("Jump");
        }
        if(Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }
        //else if (Input.GetKeyDown("c"))
        //{
        //    m_animator.SetTrigger("Attack");
        //}
        else if (Mathf.Abs(horizontal) > Mathf.Epsilon && grounded)
            m_animator.SetInteger("AnimState", 2);
        else
            m_animator.SetInteger("AnimState", 0);
        if (Input.GetKeyDown(KeyCode.Escape) && CanDash)
        {
            StartCoroutine(Dash());
        }

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isDashing)
        {
            return;
        }
        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
        Vector2 velocity = rb.velocity;
    }

    private bool IsGrounded()
    {
        bool grounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
        m_animator.SetBool("Grounded", grounded);
        return grounded;
    }
    private void Flip()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f) 
        { 
            isFacingRight = !isFacingRight;
            transform.Rotate(0f, 180f, 0f);
        }
    }

    private IEnumerator Dash()
    {
        CanDash = false;
        isDashing = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.velocity = new Vector2(dashingPower, 0f);
        tr.emitting = true;
        yield return new WaitForSeconds(dashingTime);
        tr.emitting = false;
        rb.gravityScale = originalGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
        CanDash = true;
    }

}
