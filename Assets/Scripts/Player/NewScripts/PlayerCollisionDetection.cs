using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollisionDetection : MonoBehaviour
{
    private bool isTouchingGround;
    private bool isTouchingWall;
    private bool isTouchingEnemy;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Platform"))
        {
            isTouchingGround = true;
        }

        if (collision.gameObject.CompareTag("Wall"))
        {
            isTouchingWall = true;
        }

        if (collision.gameObject.CompareTag("Enemy"))
        {
            isTouchingEnemy = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Platform"))
        {
            isTouchingGround = false;
        }

        if (collision.gameObject.CompareTag("Wall"))
        {
            isTouchingWall = false;
        }

        if (collision.gameObject.CompareTag("Enemy"))
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
}
