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
    public bool canAttack;
    public Vector2 lastDirection = Vector2.right;
    public bool facingRight = true;
    public float dashDistance;
    public float dashCooldown = 0.5f;
    public int playerAttackDamage = 5;
    public float attackOffset = 1f;
    public float attackScale = 1f;
    public float attackCooldown = 0.5f;
    public Vector2 attackDirection = Vector2.right;
    public bool yAttack = false;
    public int attackDurationFrames = 15;
    public float attackDelay = 0.7f;
    public int playerSpellDamage = 15;
    public float playerMana = 99;
    public float playerMaxMana = 99;
    public int spellManaCost = 33;
    public float spellCooldown = 1f;
    public float spellSpeed = 5f;
    public float spellDuration = 2f;
    public float healThreshold = 0.7f;
    public float manaDrainRate = 3.3f;
    public int playerHealth = 5;
    public int playerMaxHealth = 5;
    public bool isInvulnerable = false;

    // Start is called before the first frame update
    void Start()
    {
        canMove = true;
        canDash = true;
        canAttack = true;
    }

    // Update is called once per frame
    void Update()
    {
        instance = this;

        UpdateLastDirection(PlayerController.instance.horizontalInput);

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
        if (PlayerController.instance != null)
        {
            if (horizontalInput > 0)
            {
                lastDirection = Vector2.right;
                facingRight = true;
            }
            else if (horizontalInput < 0)
            {
                lastDirection = Vector2.left;
                facingRight = false;
            }
            else
            {
                // Leave blank to allow for lastDirection to maintain it's value.
            }
        }
    }

    public void UpdateAttackDirection()
    {
        if (Input.GetAxisRaw("Vertical") > 0.3) // Up
        {
            attackDirection = Vector2.up;
            yAttack = true;
        }
        else if (Input.GetAxisRaw("Vertical") < -0.3 && !PlayerCollisionDetection.instance.IsTouchingGround()) // Down
        {
            attackDirection = Vector2.down;
            yAttack = true;
        }
        else // Horizontal
        {
            attackDirection = lastDirection;
            yAttack = false;
        }
    }

    public void DamagePlayer(int damage)
    {

    }
}

//      Needs a DamagePlayer() function and
//      invulnerability periods but I don't know if
//      that should be in PlayerStatistics or
//      PlayerController, to allow for the knockback
//      when touching or being hit by an enemy.