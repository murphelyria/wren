using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatistics : MonoBehaviour
{
    public static PlayerStatistics instance;

    public float moveSpeed = 5f;
    public float jumpSpeed = 5f;
    public float fallSpeed = 5f;
    public int numJumps = 0;
    public bool hasJumped;
    public bool hasDashed;
    public bool canMove;
    public bool canDash;
    public Vector2 lastDirection = Vector2.right;
    public float dashDistance;
    public float dashCooldown = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        canMove = true;
        canDash = true;
    }

    // Update is called once per frame
    void Update()
    {
        instance = this;

        UpdateLastDirection(New_PlayerController.instance.horizontalInput);

        if (!hasJumped)
        {
            if (PlayerCollisionDetection.instance.IsTouchingGround())
            {
                numJumps = 2;
            }
            else if (!PlayerCollisionDetection.instance.IsTouchingGround())
            {
                numJumps = 1;
            }
        }
    }

    public void UpdateLastDirection(float horizontalInput)
    {
        if (New_PlayerController.instance != null)
        {
            if (horizontalInput > 0)
            {
                lastDirection = Vector2.right;
            }
            else if (horizontalInput < 0)
            {
                lastDirection = Vector2.left;
            }
        }
    }
}