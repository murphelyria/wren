using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollisionDetection : MonoBehaviour
{
    public static PlayerCollisionDetection instance;
    
    private bool isTouchingGround;
    private bool isTouchingWall;
    private bool isTouchingEnemy;

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
}
