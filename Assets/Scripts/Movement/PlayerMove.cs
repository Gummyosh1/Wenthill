using System;
using System.Collections;
using NUnit.Framework;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public Rigidbody2D rb;
    public Transform groundCheck;
    public Transform slideCheck;
    public LayerMask groundLayer;
    public TrailRenderer trailRenderer;

    //DASHING
    private bool canDash = true;
    private bool isDashing = false;
    private float dashPower = 24f;
    private float dashTime = 0.2f;
    private float dashCooldown = 1f;

    //SLIDING
    private float slideSpeed = 10f;
    private float slideTime = 0.5f;
    private float slideCooldown = 2f;
    private bool canSlide = false;
    [NonSerialized] public bool isSliding = false;
    private Coroutine slidingCoroutine = null;


    //RUNNING UP HILLS FIX
    private float groundProbeDistance = 0.35f;
    private float groundStickSpeed = 2f;
    private float maxSlopeAngle = 60f;
    private bool isJumping;


    //MOVEMENT
    private float horizontal;
    private float speed = 8f;
    private float jumpPower = 16f;
    private bool isFacingRight = true;
    [NonSerialized] public bool isMoving = false;


    public void Update()
    {
        if (isDashing)
        {
            return;
        }

        //MOVEMENT INPUT
        horizontal = Input.GetAxisRaw("Horizontal");


        //JUMP
        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            isJumping = true;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpPower);
        }


        //HALF JUMP
        if (Input.GetButtonUp("Jump") && rb.linearVelocity.y > 0f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            Coroutine dash = StartCoroutine(Dash());
        }

        //UPDATING IF WE CAN SLIDE OR NOT
        CanSlide();

        //FLIP HANDLING
        Flip();
    }

    public void FixedUpdate()
    {
        if (isDashing)
        {
            return;
        }
        
        if (isJumping && rb.linearVelocity.y <= 0f && IsGrounded())
        {
            isJumping = false;
        }


        if (canSlide && Input.GetKey(KeyCode.Tab))
        {
            if (slidingCoroutine == null)
            {
                slidingCoroutine = StartCoroutine(Slide());
            }
        }

        float moveSpeed = isSliding ? slideSpeed : speed;

        // Start slightly above the feet so the ray begins outside the ground.
        Vector2 probeOrigin =
            (Vector2)groundCheck.position + Vector2.up * 0.1f;

        RaycastHit2D groundHit = Physics2D.Raycast(
            probeOrigin,
            Vector2.down,
            groundProbeDistance,
            groundLayer
        );

        bool onWalkableGround =
            groundHit.collider != null &&
            Vector2.Angle(groundHit.normal, Vector2.up) <= maxSlopeAngle;

        if (onWalkableGround && !isJumping && !isSliding)
        {
            // A direction parallel to the ground, pointing right.
            Vector2 groundDirection = new Vector2(
                groundHit.normal.y,
                -groundHit.normal.x
            );

            rb.linearVelocity =
                groundDirection * horizontal * moveSpeed
                - groundHit.normal * groundStickSpeed;
        }
        else
        {
            // Preserve vertical velocity during jumps, falls, and slides.
            rb.linearVelocity = new Vector2(
                horizontal * moveSpeed,
                rb.linearVelocity.y
            );
        }

        isMoving = IsMoving();
    }

    private bool IsMoving()
    {
        if (horizontal != 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    private void CanSlide()
    {
        canSlide = Physics2D.OverlapCircle(slideCheck.position, 0.4f, groundLayer) && !IsGrounded() && !isSliding;
    }

    private void Flip()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    public bool getIsFacingRight()
    {
        return isFacingRight;
    }

    private IEnumerator Slide()
    {
        isSliding = true;
        canSlide = false;
        yield return new WaitForSeconds(slideTime);
        isSliding = false;
        yield return new WaitForSeconds(slideCooldown);
        canSlide = true;
        slidingCoroutine = null;
    }

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.linearVelocity = new Vector2(transform.localScale.x * dashPower, 0f);
        trailRenderer.emitting = true;
        yield return new WaitForSeconds(dashTime);
        trailRenderer.emitting = false;
        rb.gravityScale = originalGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }
}
