using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sw_EnemyAttackController : MonoBehaviour
{
    [SerializeField] private int damage = 1; // damage that the enemy's attack will do to the player
    [SerializeField] private float attackDuration = 0.5f; // duration of the enemy's attack

    private bool isAttacking = false; // flag indicating whether the enemy is currently attacking
    private Vector2 direction; // direction the enemy is facing
    private bool isClone; // flag indicating whether this script is attached to a clone of the enemy

    private void Start()
    {
        if (isClone)
        {
            Destroy(gameObject, GetAttackDuration()); // destroy the clone object after the attack duration has elapsed
        }
    }

    // sets the direction the enemy is facing
    public void SetDirection(Vector2 direction)
    {
        this.direction = direction.normalized;
    }

    // gets the duration of the enemy's attack
    public float GetAttackDuration()
    {
        return attackDuration;
    }

    // called when the enemy's trigger collider collides with another collider
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // if the enemy is attacking and the collided object is the player
        if (isAttacking && collision.gameObject.CompareTag("Player"))
        {
            PlayerStatistics playerStatistics = collision.gameObject.GetComponent<PlayerStatistics>(); // get the player's stats component
            if (playerStatistics != null && !playerStatistics.isInvulnerable) // if the player has a stats component and is not invulnerable
            {
                playerStatistics.DamagePlayer(damage); // damage the player
            }
        }
    }

    // initiates the enemy's attack
    public void Attack()
    {
        isAttacking = true;
        StartCoroutine(StopAttackCoroutine()); // start coroutine to stop the attack after a set duration
    }

    // coroutine to stop the enemy's attack after a set duration
    private IEnumerator StopAttackCoroutine()
    {
        yield return new WaitForSeconds(GetAttackDuration()); // wait for the attack duration to elapse
        isAttacking = false; // stop attacking
    }

    // sets the isClone flag
    public void SetIsClone(bool isClone)
    {
        this.isClone = isClone;
    }
}
