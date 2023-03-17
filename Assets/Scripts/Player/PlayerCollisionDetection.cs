using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollisionDetection : MonoBehaviour
{
    public static PlayerCollisionDetection instance;

    public Vector2 wallDirection = Vector2.zero;

    public bool isTouchingGround;
    public bool isTouchingWall;
    public bool isTouchingEnemy;

    private void Start()
    {
        instance = this;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Platforms"))
        {
            isTouchingGround = true;
            PlayerStatistics.instance.hasJumped = false;
        }

        if (collision.gameObject.CompareTag("Walls"))
        {
            isTouchingWall = true;

            // Calculate the direction from the player to the wall
            Vector2 direction = collision.contacts[0].point - (Vector2)transform.position;
            direction.Normalize();

            // Store the direction in wallDirection
            wallDirection = direction;
        }

        if (collision.gameObject.CompareTag("Enemies"))
        {
            isTouchingEnemy = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Platforms"))
        {
            isTouchingGround = false;
        }

        if (collision.gameObject.CompareTag("Walls"))
        {
            isTouchingWall = false;
            wallDirection = Vector2.zero; // Clear the wall direction
        }

        if (collision.gameObject.CompareTag("Enemies"))
        {
            isTouchingEnemy = false;
        }
    }

    public bool IsTouchingGround()
    {
        return isTouchingGround;
    }

    public bool IsTouchingWall()
    {
        return isTouchingWall;
    }

    public bool IsTouchingEnemy()
    {
        return isTouchingEnemy;
    }

    public Vector2 GetWallDirection()
    {
        return wallDirection;
    }
}