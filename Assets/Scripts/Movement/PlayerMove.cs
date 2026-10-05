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


        if (canSlide && Input.GetKey(KeyCode.Tab))
        {
            if (slidingCoroutine == null)
            {
                slidingCoroutine = StartCoroutine(Slide());
            }
        }

        if (isSliding)
        {
            rb.linearVelocity = new Vector2(horizontal * slideSpeed, rb.linearVelocity.y);
        }
        else
        {
            //MOVEMENT PHYSICS
            rb.linearVelocity = new Vector2(horizontal * speed, rb.linearVelocity.y);
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
