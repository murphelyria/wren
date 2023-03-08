using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public static EnemyController Instance;

    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float sprintSpeed = 5f;
    [SerializeField] private float detectionDistanceFront = 8f;
    [SerializeField] private float detectionDistanceBack = 3f;
    [SerializeField] private float attackDistance = 1f;
    [SerializeField] private GameObject attackPrefab;
    [SerializeField] private float preAttackDelay = 0.62f;
    [SerializeField] private float attackCooldown = 1f;

    private bool canSeePlayer = false;
    private bool facingRight = true;
    private bool attacking = false;
    private GameObject attackObject = null;

    private float timeSinceLastAttack = 0f;

    private void Update()
    {
        Instance = this;

        timeSinceLastAttack += Time.deltaTime;

        if (DetectPlayerFront() && !attacking)
        {
            Move(sprintSpeed);
        }
        else if (!attacking)
        {
            Move(moveSpeed);
        }
        else
        {
            Move(0f);
        }

        if (DetectPlayerBack() || DetectWall())
        {
            Flip();
        }

        if (canSeePlayer && timeSinceLastAttack >= attackCooldown)
        {
            if (Vector2.Distance(transform.position, PlayerController.Instance.transform.position) <= attackDistance)
            {
                Attack();
            }
        }
    }

    private void Move(float speed)
    {
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
        Vector2 direction = facingRight ? Vector2.right : Vector2.left;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, detectionDistanceFront, LayerMask.GetMask("Player"));

        if (hit.collider != null)
        {
            canSeePlayer = true;
        }
        else
        {
            canSeePlayer = false;
        }

        return hit.collider != null;
    }

    private bool DetectPlayerBack()
    {
        Vector2 direction = facingRight ? Vector2.left : Vector2.right;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, detectionDistanceBack, LayerMask.GetMask("Player"));

        if (hit.collider != null)
        {
            canSeePlayer = true;
        }

        return hit.collider != null;
    }

    private bool DetectWall()
    {
        Vector2 direction = facingRight ? Vector2.right : Vector2.left;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, 0.6f, LayerMask.GetMask("Walls"));

        // Draw a debug line to show the raycast
        Debug.DrawLine(transform.position, transform.position + (Vector3)direction * 0.6f, Color.red);

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
        EnemyAttackController enemyAttackController = attackObject.GetComponent<EnemyAttackController>();

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