using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerMovement : MonoBehaviour
{
    public float horizontally = 1;
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
    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask wallLayer;
    private bool CanDash = true;
    private bool isDashing;
    private float dashingPower = 24f;
    private float dashingTime = 0.2f;
    private float dashingCooldown = 1f;
    private bool isWallSliding;
    private float wallSlidingSpeed;

    private bool isWallJumping;
    private float wallJumpingDirection;
    private float wallJumpingTime = 0.2f;
    private float wallJumpingCounter;
    private float wallJumpingDuration = 0f;
    private Vector2 wallJumpingPower = new Vector2(16f, 16f);

    // Start is called before the first frame update
    void Update()
    {
        
        if (isDashing)
        {
            return;
        }
        WallSlide();
        WallJump();
        
        if (!isWallJumping)
        {
            Flip();
        }
        float yRotation = transform.eulerAngles.y;
        if (Input.GetKeyDown(KeyCode.Escape) && CanDash && (Mathf.Abs(yRotation - 0) < 1))
        {
            horizontally = 1;
            StartCoroutine(Dash());
        }
        if (Input.GetKeyDown(KeyCode.Escape) && CanDash && (Mathf.Abs(yRotation - 180) < 1))
        {
            horizontally = -1;
            StartCoroutine(Dash());
        }
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
        else if (Mathf.Abs(horizontal) > Mathf.Epsilon && grounded)
            m_animator.SetInteger("AnimState", 2);
        else
            m_animator.SetInteger("AnimState", 0);

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isDashing)
        {
            return;
        }
        if (!isWallJumping)
        {
            rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
        }
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
            horizontally = -1;
        }
    }

    private IEnumerator Dash()
    {
        CanDash = false;
        isDashing = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.velocity = new Vector2(dashingPower * horizontally, 0f);
        tr.emitting = true;
        yield return new WaitForSeconds(dashingTime);
        tr.emitting = false;
        rb.gravityScale = originalGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
        CanDash = true;      
    }
    private bool IsWalled()
    {
        return Physics2D.OverlapCircle(wallCheck.position, 0.2f, wallLayer);
    }

    private void WallSlide()
    {
        if(IsWalled() && !IsGrounded() && horizontal !=0f)
        {
            isWallSliding = true;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));
        }
        else
        {
            isWallSliding = false;
        }
    }

    private void WallJump()
    {
        if (isWallSliding)
        {
            isWallSliding = false;
            wallJumpingDirection = horizontally;
            wallJumpingCounter = wallJumpingTime;

            CancelInvoke(nameof(StopWallJumping));
        }
        else
        {
            wallJumpingCounter -= Time.deltaTime;
        }

        if (Input.GetButtonDown("Jump") && wallJumpingCounter > 0f)
        {
            isWallJumping = true;
            rb.velocity = new Vector2(wallJumpingDirection * wallJumpingPower.x, wallJumpingPower.y);
            wallJumpingCounter = 0f;

            if (horizontally != wallJumpingDirection)
            {
                isFacingRight = !isFacingRight;
                transform.Rotate(0f, 180f, 0f);
                horizontal = -1;
            }

            Invoke(nameof(StopWallJumping), wallJumpingDuration);
        }
    }

    private void StopWallJumping()
    {
        isWallJumping = false;
    }
}
