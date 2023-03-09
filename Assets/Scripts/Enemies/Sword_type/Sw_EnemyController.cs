using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sw_EnemyController : MonoBehaviour
{
    // Declare a static instance of the enemy controller
    public static Sw_EnemyController Instance;

    // Declare various public variables that can be set in the Unity inspector
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float sprintSpeed = 5f;
    [SerializeField] private float detectionDistanceFront = 8f;
    [SerializeField] private float detectionDistanceBack = 3f;
    [SerializeField] private float attackDistance = 1.5f;
    [SerializeField] private GameObject attackPrefab;
    [SerializeField] private float preAttackDelay = 0.62f;
    [SerializeField] private float attackCooldown = 1f;

    // Declare various private variables for use within the script
    private bool canSeePlayer = false;
    private bool facingRight = true;
    private bool attacking = false;
    private GameObject attackObject = null;

    private float timeSinceLastAttack = 0f;

    // Define the update function that runs once per frame
    private void Update()
    {
        // Set the static instance to this script
        Instance = this;

        // Update the time since the last attack
        timeSinceLastAttack += Time.deltaTime;

        // Check if the player is in front of the enemy and not currently attacking
        if (DetectPlayerFront() && !attacking)
        {
            Move(sprintSpeed); // Move the enemy towards the player at sprint speed
        }
        // If the player is not in front of the enemy and the enemy is not currently attacking
        else if (!attacking)
        {
            Move(moveSpeed); // Move the enemy towards the player at normal speed
        }
        // If the enemy is currently attacking
        else
        {
            Move(0f); // Stop moving
        }

        // Check if there is a wall or player behind the enemy
        if (DetectPlayerBack() || DetectWall())
        {
            Flip(); // Flip the enemy around
        }

        // If the enemy can see the player and enough time has passed since the last attack
        if (canSeePlayer && timeSinceLastAttack >= attackCooldown)
        {
            // If the player is within attack distance
            if (Vector2.Distance(transform.position, PlayerController.Instance.transform.position) <= attackDistance)
            {
                Attack(); // Initiate an attack
            }
        }
    }

    // Define a function to move the enemy
    private void Move(float speed)
    {
        // Move the enemy based on the current facing direction and speed
        if (facingRight)
        {
            transform.Translate(Vector2.right * speed * Time.deltaTime);
        }
        else
        {
            transform.Translate(Vector2.left * speed * Time.deltaTime);
        }
    }

    private bool DetectPlayerFront()
    {
        // Get the raycast direction based on the current facing direction
        Vector2 direction = facingRight ? Vector2.right : Vector2.left;

        // Cast a ray in the direction of the player, and store the result in a RaycastHit2D object
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, detectionDistanceFront, LayerMask.GetMask("Player"));

        // Set the 'canSeePlayer' variable based on whether the raycast hit the player or not
        if (hit.collider != null)
        {
            canSeePlayer = true;
        }
        else
        {
            canSeePlayer = false;
        }

        // Return whether the player was detected by the raycast or not
        return hit.collider != null;
    }

    private bool DetectPlayerBack()
    {
        // Get the raycast direction based on the current facing direction
        Vector2 direction = facingRight ? Vector2.left : Vector2.right;

        // Cast a ray behind the enemy, and store the result in a RaycastHit2D object
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, detectionDistanceBack, LayerMask.GetMask("Player"));

        // Set the 'canSeePlayer' variable based on whether the raycast hit the player or not
        if (hit.collider != null)
        {
            canSeePlayer = true;
        }

        // Return whether the player was detected by the raycast or not
        return hit.collider != null;
    }

    private bool DetectWall()
    {
        // Get the raycast direction based on the current facing direction
        Vector2 direction = facingRight ? Vector2.right : Vector2.left;

        // Cast a ray in the direction of the enemy's movement, and store the result in a RaycastHit2D object
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, 0.6f, LayerMask.GetMask("Walls"));

        // Draw a debug line to show the raycast in the Scene view (useful for debugging)
        Debug.DrawLine(transform.position, transform.position + (Vector3)direction * 0.6f, Color.red);

        // Return whether the raycast hit a wall or not
        return hit.collider != null;
    }

    private void Attack()
    {
        // Start the delay coroutine
        StartCoroutine(AttackDelayCoroutine());
    }

    private IEnumerator AttackDelayCoroutine()
    {
        canSeePlayer = false;
        attacking = true;

        // Wait for the attack delay duration
        yield return new WaitForSeconds(preAttackDelay);

        // Only spawn a new attack object if one isn't already present
        if (attackObject == null)
        {
            Vector3 offset = facingRight ? Vector3.right : Vector3.left;
            attackObject = Instantiate(attackPrefab, transform.position + offset, Quaternion.identity);
            StartCoroutine(AttackCoroutine());
        }
    }

    private IEnumerator AttackCoroutine()
    {
        // Get the EnemyAttackController component from the attack object
        Sw_EnemyAttackController enemyAttackController = attackObject.GetComponent<Sw_EnemyAttackController>();

        // Set the direction of the attack based on which way the enemy is facing
        enemyAttackController.SetDirection(facingRight ? Vector2.right : Vector2.left);

        // Defines the new clone as a clone (slightly redundant technically, but I couldn't think of a better way to do this)
        enemyAttackController.SetIsClone(true);

        // Start the attack animation
        enemyAttackController.Attack();

        // Wait for the attack animation to finish
        yield return new WaitForSeconds(enemyAttackController.GetAttackDuration());

        // Destroy the attack object when the animation is finished
        Destroy(attackObject);
        attackObject = null;

        attacking = false;
    }

    private void Flip()
    {
        facingRight = !facingRight;
        transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
    }
}