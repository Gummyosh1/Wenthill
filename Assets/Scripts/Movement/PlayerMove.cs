using System;
using System.Collections;
using NUnit.Framework;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public Rigidbody2D rb;
    public Transform groundCheck;
    public LayerMask groundLayer;
    public TrailRenderer trailRenderer;

    //DASHING
    private bool canDash = true;
    private bool isDashing = false;
    private float dashPower = 24f;
    private float dashTime = 0.2f;
    private float dashCooldown = 1f;


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

        //FLIP HANDLING
        Flip();
    }

    public void FixedUpdate()
    {
        if (isDashing)
        {
            return;
        }

        //MOVEMENT PHYSICS
        rb.linearVelocity = new Vector2(horizontal * speed, rb.linearVelocity.y);
        if (horizontal != 0)
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
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
