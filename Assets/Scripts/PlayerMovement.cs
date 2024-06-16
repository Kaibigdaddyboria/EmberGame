using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Variables for movement control
    public float speed = 8f;
    public float jumpingPower = 16f;
    private bool isFacingRight = true;
    private float yRotation;
    private float horizontal;

    // Variables for animation
    public Animator m_animator;

    // Variables for dashing
    public float dashingPower = 24f;
    public float dashingTime = 0.2f;
    public float dashingCooldown = 1f;
    private bool CanDash = true;
    private bool isDashing;
    private float dashDirection;

    // Variables for wall sliding and jumping
    public float wallJumpingDuration = 0.4f;
    public float wallJumpingTime = 0.2f;
    public float wallSlidingSpeed;
    private bool isWallSliding;
    private bool isWallJumping;
    private float wallJumpingDirection;
    private float wallJumpingCounter;
    private Vector2 wallJumpingPower = new Vector2(6f, 14f);

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private TrailRenderer tr;

    public PlayerCombat combat;

    // Update is called once per frame
    void Update()
    {
        // Checks if either bool is true
        if (combat.isAttacking || combat.isDamaged)
        {
            return;
        }

        // Runs these functions on update
        SetRoation();
        WallSlide();
        WallJump();
        if (!isWallJumping)
        {
            Flip();
        }
        DashTrigger();
        //S ets variables
        bool grounded = IsGrounded();
        horizontal = Input.GetAxisRaw("Horizontal");

        // Handle jumping
        if (Input.GetButtonDown("Jump") && grounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
            grounded = false;
            m_animator.SetTrigger("Jump");
        }

        // Handle jump release
        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }
        else if (Mathf.Abs(horizontal) > Mathf.Epsilon && grounded)
            m_animator.SetInteger("AnimState", 2);
        else
            m_animator.SetInteger("AnimState", 0);
    }

    // FixedUpdate is called at a fixed interval
    void FixedUpdate()
    {
        if (isDashing || combat.isAttacking || combat.isDamaged)
        {
            return;
        }

        // Checks if isWallJumping is true then handles horizontal movement
        if (!isWallJumping)
        {
            rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
        }
    }

    // Check if the player is grounded
    private bool IsGrounded()
    {
        bool grounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
        m_animator.SetBool("Grounded", grounded);
        return grounded;
    }

    // Flip the player's direction
    private void Flip()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            transform.Rotate(0f, 180f, 0f);
        }
    }

    // Set the player's rotation
    private void SetRoation()
    {
        yRotation = transform.eulerAngles.y;
        if ((Mathf.Abs(yRotation - 0) < 1))
        {
            yRotation = 1;
        }
        if (Mathf.Abs(yRotation - 180) < 1)
        {
            yRotation = -1;
        }
    }

    // Trigger the dash action
    private void DashTrigger()
    {
        if (!IsWalled())
        {
            dashDirection = yRotation;
        }
        if (Input.GetKeyDown(KeyCode.Escape) && CanDash && yRotation == 1)
        {
            StartCoroutine(Dash());
        }
        if (Input.GetKeyDown(KeyCode.Escape) && CanDash && yRotation == -1)
        {
            StartCoroutine(Dash());
        }
    }

    // Perform the dash action
    private IEnumerator Dash()
    {
        CanDash = false;
        isDashing = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.velocity = new Vector2(dashingPower * dashDirection, 0f);
        tr.emitting = true;
        yield return new WaitForSeconds(dashingTime);
        tr.emitting = false;
        rb.gravityScale = originalGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
        CanDash = true;
    }

    // Check if the player is touching a wall
    private bool IsWalled()
    {
        return Physics2D.OverlapCircle(wallCheck.position, 0.2f, wallLayer);
    }

    // Handle wall sliding
    private void WallSlide()
    {
        if (IsWalled() && !IsGrounded() && horizontal != 0f)
        {
            isWallSliding = true;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));
            dashDirection = -yRotation;
        }
        else
        {
            isWallSliding = false;
        }
    }

    // Handle wall jumping
    private void WallJump()
    {
        if (isWallSliding)
        {
            isWallJumping = false;
            wallJumpingDirection = -yRotation;
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

            if (yRotation != wallJumpingDirection)
            {
                isFacingRight = !isFacingRight;
                transform.Rotate(0f, 180f, 0f);
            }

            Invoke(nameof(StopWallJumping), wallJumpingDuration);
        }
    }

    // Stop wall jumping
    private void StopWallJumping()
    {
        isWallJumping = false;
    }
}