using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement instance;

    private Rigidbody2D _rb;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        instance = this;
    }

    public void HandleMovement(float horizontalInput)
    {
        if (PlayerStatistics.instance.canMove)
        {
            float horizontalMovement = horizontalInput * PlayerStatistics.instance.moveSpeed;

            // Check if player is touching a wall
            if (PlayerCollisionDetection.instance.IsTouchingWall())
            {
                // Get the direction of the wall
                Vector2 wallDirection = PlayerCollisionDetection.instance.GetWallDirection();

                // If moving in the direction of the wall, prevent movement
                if (Mathf.Sign(horizontalMovement) == Mathf.Sign(wallDirection.x))
                {
                    horizontalMovement = 0;
                }
            }

            Vector2 horzMovement = new(horizontalMovement, _rb.velocity.y);
            _rb.velocity = horzMovement;
        }
    }

    public void HandleJump()
    {
        if (PlayerStatistics.instance.numJumps > 0)
        {
            Vector2 jumpMovement = new(_rb.velocity.x, PlayerStatistics.instance.jumpSpeed);
            _rb.velocity = jumpMovement;

            PlayerStatistics.instance.numJumps--;
        }
    }

    public void HandleFall()
    {
        Vector2 fallMovement = new(_rb.velocity.x, -(PlayerStatistics.instance.fallSpeed));
        _rb.velocity = fallMovement;
    }

    public void HandleDash()
    {
        if (PlayerStatistics.instance.canDash)
        {
            float dashDirection = PlayerStatistics.instance.lastDirection.x;

            StartCoroutine(DoDash(dashDirection));
        }
    }

    private IEnumerator DoDash(float dashDirection)
    {
        PlayerStatistics.instance.canMove = false;
        PlayerStatistics.instance.canDash = false;

        _rb.constraints ^= RigidbodyConstraints2D.FreezePositionY;

        float distanceTraveled = 0;
        while (distanceTraveled < PlayerStatistics.instance.dashDistance)
        {
            float frameVelocity = dashDirection * (PlayerStatistics.instance.dashDistance * (3 * Time.deltaTime));
            _rb.velocity = new Vector2(frameVelocity, _rb.velocity.y);

            distanceTraveled += Mathf.Abs(frameVelocity);

            yield return null;
        }

        PlayerStatistics.instance.canMove = true;
        _rb.constraints &= ~RigidbodyConstraints2D.FreezePositionY;

        yield return new WaitForSeconds(PlayerStatistics.instance.dashCooldown);

        PlayerStatistics.instance.canDash = true;
    }
}