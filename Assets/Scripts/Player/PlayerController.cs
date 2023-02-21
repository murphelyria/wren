using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private bool isGrounded;
    private bool hasJumped;
    private bool isDashing;
    private bool facingRight;
    private bool hasDoubleJumped;

    private int framesSinceDirectionChange = 0;
    public int skidFrames = 15;
    public float skidToStopMultiplierRelease = 0.5f;
    public float skidToStopMultiplierSwap = 0.1f;
    public float speedUpRate = 0.2f;
    public float slowDownRate = 0.5f;

    public float moveSpeed;
    public float jumpForce;
    public float dashDistance;

    public float jumpTime = 0.25f;
    public float jumpTimeCounter;
    public bool isJumping;
    public float jumpTimeMultiplier = 2f;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>(); // Calls interactions from Rigidbody2D component for platform/ground interactions
    }

    void Update()
    {
        Move(); // Left/right movement
        Jump(); // Vertical jump
        Dash(); // Stop all vertical movement and quickly move in a direction the player is facing (needs to be an unlock)
        DoubleJump(); // Allow the player one jump in the air, basically just the jump code without the isGrounded condition (needs to be an unlock)
        Attack(); // Spawn attack sprite in direction player is facing (needs to adjust for various weapons, but we'll start with a basic sword)
        Spell(); // Spawn spell sprite that moves in direction player is facing (needs to adjust for various builds, but we'll start with a basic blast)
    }

    // ----------------------   Movement Based Code   ---------------------- //

    void Move()
    {
        if (!isDashing) // Only allow movement if the player isn't dashing
        {
            float moveInput = Input.GetAxisRaw("Horizontal");
            float targetVelocityX = moveInput * moveSpeed;

            if (moveInput != 0 && Mathf.Sign(moveInput) != Mathf.Sign(rb.velocity.x))
            {
                // Player has swapped direction, use faster slow down time
                rb.velocity = new Vector2(rb.velocity.x * skidToStopMultiplierSwap, rb.velocity.y);
                framesSinceDirectionChange = 0;
            }
            else if (moveInput == 0 && isGrounded)
            {
                // Player has released direction, use slower slow down time
                rb.velocity = new Vector2(rb.velocity.x * skidToStopMultiplierRelease, rb.velocity.y);
                framesSinceDirectionChange = 0;
            }
            else
            {
                framesSinceDirectionChange++;
            }

            if (moveInput > 0 && !facingRight)
            {
                Flip();
            }
            else if (moveInput < 0 && facingRight)
            {
                Flip();
            }

            // Gradually increase/decrease velocity to target velocity
            if (Mathf.Abs(rb.velocity.x) < Mathf.Abs(targetVelocityX))
            {
                rb.velocity = new Vector2(Mathf.MoveTowards(rb.velocity.x, targetVelocityX, speedUpRate), rb.velocity.y);
            }
            else if (framesSinceDirectionChange >= skidFrames)
            {
                rb.velocity = new Vector2(Mathf.MoveTowards(rb.velocity.x, targetVelocityX, slowDownRate), rb.velocity.y);
            }
        }
    }

    void Jump()
    {
        if (Input.GetButtonDown("Jump") && !hasJumped) // Only allow jump if player hasn't jumped yet
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpTimeCounter = jumpTime;
            hasJumped = true;
        }

        if (Input.GetButton("Jump") && jumpTimeCounter > 0 && hasJumped)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce * jumpTimeMultiplier);
            jumpTimeCounter -= Time.deltaTime;
        }
        else
        {
            jumpTimeCounter = 0;
        }
    }

    void Dash()
    {
        if (Input.GetButtonDown("Dash") && !isDashing)
        {
            // Start dashing
            isDashing = true;

            // Stop all motion
            rb.velocity = Vector2.zero;

            // Determine the direction the player should dash based on input
            float dashDirection = facingRight ? 1.0f : -1.0f;

            // Move horizontally in the direction the player was facing for a set distance
            StartCoroutine(DoDash(dashDirection));
        }
    }

    private IEnumerator DoDash(float dashDirection)
    {
        float distanceTraveled = 0;
        while (distanceTraveled < dashDistance)
        {
            // Calculate the velocity for this frame based on the dash direction and speed
            float frameVelocity = dashDirection * (dashDistance / 0.25f) * Time.deltaTime;
            rb.velocity = new Vector2(frameVelocity, 0);

            // Update the distance traveled so far
            distanceTraveled += Mathf.Abs(frameVelocity);

            yield return null;
        }

        // End the dash and allow movement again
        isDashing = false;
    }

    void DoubleJump()
    {
        if (Input.GetButtonDown("Jump") && !hasDoubleJumped && !isGrounded) // Only allow jump if player hasn't jumped yet
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpTimeCounter = jumpTime;
            hasDoubleJumped = true;
        }

        if (Input.GetButton("Jump") && jumpTimeCounter > 0 && hasJumped)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce * jumpTimeMultiplier);
            jumpTimeCounter -= Time.deltaTime;
        }
        else
        {
            jumpTimeCounter = 0;
        }
    }


    // ----------------------   Action Based Code   ---------------------- //


    void Attack()
    {

    }


    void Spell()
    {

    }

    // ----------------------   Detection Based Code   ---------------------- //

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground") || collision.gameObject.layer == LayerMask.NameToLayer("Platform"))
        {
            // Set grounded condition to true and reset Jump bool
            isGrounded = true;
            hasJumped = false;
            hasDoubleJumped = false;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground") || collision.gameObject.layer == LayerMask.NameToLayer("Platform"))
        {
            // Set grounded condition to false
            isGrounded = false;
        }
    }

    // ----------------------   Animation Based Code   ---------------------- //

    void Flip()
    {
        // Switch the value of facingRight
        facingRight = !facingRight;

        // Get the local scale of the player sprite
        Vector3 scale = transform.localScale;

        // Flip the x-axis of the local scale to change the direction the player is facing
        scale.x *= -1;

        // Set the local scale of the player sprite to the new value
        transform.localScale = scale;
    }
}