using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;

    private Rigidbody2D rb;
    public PlayerStats playerStats;
    private bool isGrounded; // Whether the player is on the ground
    private bool hasJumped; // Whether the player has already jumped
    private bool isDashing; // Whether the player is currently dashing
    private bool facingRight; // Whether the player is facing right
    private bool hasDoubleJumped; // Whether the player has already done a double jump

    private int framesSinceDirectionChange = 0; // The number of frames since the player changed direction
    public int skidFrames = 15; // The number of frames the player should slide before stopping
    public float skidToStopMultiplierRelease = 0.5f; // The slowdown multiplier when the player releases a direction key
    public float skidToStopMultiplierSwap = 0.1f; // The slowdown multiplier when the player swaps direction
    public float speedUpRate = 0.2f; // The rate at which the player's speed increases
    public float slowDownRate = 0.5f; // The rate at which the player slows down
    public LayerMask wallLayer;
    public float wallDistance = 0.2f;

    public GameObject horizontalAttackPrefab; // The prefab for the horizontal attack
    public GameObject verticalAttackPrefab; // The prefab for the vertical attack
    public float attackCooldown = 0.5f; // The cooldown for attacks
    public int attackDurationFrames = 10; // The number of frames an attack should last
    private float attackTimer; // The amount of time left before the player can attack again

    public GameObject spellPrefab; // The prefab for the spell
    public float spellCooldown = 1.0f; // The cooldown for spells
    public float spellSpeed = 5.0f; // The speed of the spell
    public float spellDuration = 2.0f; // The duration of the spell
    private float spellTimer; // The amount of time left before the player can cast another spell

    public float healThreshold = 0.7f;
    public float manaDrainRate = 16.5f;

    public float moveSpeed; // The player's movement speed
    public float jumpForce; // The force of the player's jump
    public float dashDistance; // The distance the player should dash

    public float jumpTime = 0.25f; // The time the player can hold down the jump key for a higher jump
    public float jumpTimeCounter; // The amount of time left that the player can hold down the jump key for a higher jump
    public bool isJumping; // Whether the player is currently jumping
    public float jumpTimeMultiplier = 2f; // The amount the jump force is multiplied by when the player holds down the jump key for a higher jump

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>(); // Calls interactions from Rigidbody2D component for platform/ground interactions
        playerStats = GetComponent<PlayerStats>(); // get the reference to the PlayerStats component of the player game object
    }

    void Update()
    {
        Instance = this;

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

                // Reset frame counter
                framesSinceDirectionChange = 0;
            }
            else if (moveInput == 0 && isGrounded)
            {
                // Player has released direction, use slower slow down time
                rb.velocity = new Vector2(rb.velocity.x * skidToStopMultiplierRelease, rb.velocity.y);

                // Reset frame counter
                framesSinceDirectionChange = 0;
            }
            else
            {
                // Increment frame counter
                framesSinceDirectionChange++;
            }

            // Flip player if necessary
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

            // Check for wall collision
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right * moveInput, wallDistance, wallLayer);
            if (hit.collider != null)
            {
                rb.velocity = new Vector2(0f, rb.velocity.y);
            }
        }
    }

    void Jump()
    {
        if (Input.GetButtonDown("Jump") && !hasJumped) // Check if the player has pressed the jump button and hasn't already jumped
        {
            // Apply initial jump velocity to the rigidbody and set up the jump time counter
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpTimeCounter = jumpTime;
            hasJumped = true;
        }

        if (Input.GetButton("Jump") && jumpTimeCounter > 0 && hasJumped) // If the player is holding down the jump button and the jump time counter hasn't run out
        {
            // Apply an additional jump force to the rigidbody, and decrement the jump time counter
            rb.velocity = new Vector2(rb.velocity.x, jumpForce * jumpTimeMultiplier);
            jumpTimeCounter -= Time.deltaTime;
        }
        else
        {
            // If the player has stopped holding down the jump button, or the jump time counter has run out, reset the jump time counter
            jumpTimeCounter = 0;
        }
    }

    void Dash()
    {
        if (Input.GetButtonDown("Dash") && !isDashing)
        {
            // Check if the player is currently touching a wall
            bool isTouchingWall = Physics2D.Raycast(transform.position, Vector2.right * (facingRight ? 1.0f : -1.0f), wallDistance, wallLayer);

            // Determine the direction the player should dash based on input or wall touch
            float dashDirection;
            if (isTouchingWall)
            {
                dashDirection = -(facingRight ? 1.0f : -1.0f);
            }
            else if (Input.GetAxisRaw("Horizontal") != 0.0f)
            {
                dashDirection = Input.GetAxisRaw("Horizontal");
            }
            else
            {
                dashDirection = facingRight ? 1.0f : -1.0f;
            }

            // Start dashing
            isDashing = true;

            // Stop all motion
            rb.velocity = Vector2.zero;

            // Move horizontally in the direction the player was facing or the opposite direction of the wall they are touching
            StartCoroutine(DoDash(dashDirection));
        }
    } 

    // This is a coroutine that handles the dash action, by effecting motion while regular movement is suspended.
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
        if (Input.GetButtonDown("Jump") && !hasDoubleJumped && !isGrounded) // Check if the player is performing a double jump
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpTimeCounter = jumpTime;
            hasDoubleJumped = true; // Mark that the player has used the double jump
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
        // Check if the player has pressed the attack button and if the attack is off cooldown
        if (Input.GetButtonDown("Attack") && attackTimer >= attackCooldown)
        {
            attackTimer = 0;

            GameObject attackInstance;

            // Determine the direction of the attack based on whether the player is pressing up, down, or left/right
            if (Input.GetAxisRaw("Vertical") > 0) // Up
            {
                attackInstance = Instantiate(verticalAttackPrefab, transform.position + Vector3.up, Quaternion.identity, transform);
                StartCoroutine(DestroyAfterFrames(attackInstance, attackDurationFrames));
            }
            else if (Input.GetAxisRaw("Vertical") < 0) // Down
            {
                attackInstance = Instantiate(verticalAttackPrefab, transform.position + Vector3.down, Quaternion.identity, transform);
                StartCoroutine(DestroyAfterFrames(attackInstance, attackDurationFrames));
            }
            else // Horizontal
            {
                Vector3 offset = facingRight ? Vector3.right : Vector3.left;
                attackInstance = Instantiate(horizontalAttackPrefab, transform.position + offset, Quaternion.identity, transform);
                StartCoroutine(DestroyAfterFrames(attackInstance, attackDurationFrames));
            }
        }

        // Increment the attack timer
        attackTimer += Time.deltaTime;
    }

    void Spell()
    {
        // Check if the player has pressed the spell button and if the spell is off cooldown
        if (Input.GetButtonDown("Spell") && spellTimer <= 0 && playerStats.playerMana >= 33)
        {
            StartCoroutine(CastSpell());
        }
        else if (Input.GetButtonDown("Spell") && spellTimer <= 0 && playerStats.playerMana < 33)
        {
            Debug.Log("Player doesn't have enough mana.");
        }

        if (spellTimer > 0)
        {
            // Decrease spell timer
            spellTimer -= Time.deltaTime;
        }
    }

    IEnumerator CastSpell()
    {
        // Determine how long the spell button has been held down
        float spellHoldTime = 0f;
        while (Input.GetButton("Spell") && spellHoldTime < healThreshold)
        {
            spellHoldTime += Time.deltaTime;
            yield return null;
        }

        if (spellHoldTime >= healThreshold)
        {
            // Start the drain over time coroutine
            yield return StartCoroutine(DrainManaOverTime(playerStats));

            // Start the spell cooldown
            spellTimer = spellCooldown;
        }
        else
        {
            // Spawn the spell prefab at the player's position
            GameObject spellObject = Instantiate(spellPrefab, transform.position, Quaternion.identity);

            // Determine the direction the spell should travel based on player facing
            int spellDirection = facingRight ? 1 : -1;

            // Set the spell's initial velocity to move in the direction the player is facing
            Rigidbody2D spellRB = spellObject.GetComponent<Rigidbody2D>();
            spellRB.velocity = new Vector2(spellSpeed * spellDirection, 0);

            // Set the spell's despawn timer
            Destroy(spellObject, spellDuration);

            // Start the spell cooldown
            spellTimer = spellCooldown;

            // Drain mana for the spell
            playerStats.playerMana -= playerStats.spellManaCost;
        }
    }

    IEnumerator DrainManaOverTime(PlayerStats playerStats)
    {
        // Initialize variables for mana draining
        float manaDrained = 0f;
        float newManaValue = playerStats.playerMana;

        // Continue draining mana until the required amount has been drained
        while (manaDrained < playerStats.spellManaCost)
        {
            // Calculate the new mana value based on the current rate of mana drain
            newManaValue -= manaDrainRate * Time.deltaTime;

            // Track the total amount of mana drained
            manaDrained += manaDrainRate * Time.deltaTime;

            // Update the player's mana value
            playerStats.playerMana = newManaValue;

            // Wait for the next frame
            yield return null;
        }

        // Round the final mana value to an integer
        playerStats.playerMana = Mathf.RoundToInt(newManaValue);

        // Heal the player
        playerStats.HealPlayer();
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




    // ----------------------   Misc. Functions Code   ---------------------- //

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

    // This is a coroutine that destroys a game object after a certain number of frames.
    private IEnumerator DestroyAfterFrames(GameObject obj, int frames)
    {
        // Wait for the specified number of frames before destroying the object.
        yield return new WaitForSeconds(frames * Time.deltaTime);

        // Destroy the object.
        Destroy(obj);
    }
}