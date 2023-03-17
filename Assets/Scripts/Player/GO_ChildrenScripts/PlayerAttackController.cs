using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackController : MonoBehaviour
{
    public PlayerStatistics playerStatistics;

    void Awake()
    {
        playerStatistics = FindObjectOfType<PlayerStatistics>(); // get the reference to the PlayerStats component of the player game object
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        int damage = playerStatistics.playerAttackDamage; // define damage based on stats in PlayerStats

        // If AttackPrefab interacts with any enemy, deal damage to them
        if (collision.gameObject.CompareTag("Enemies"))
        {
            EnemyStats enemyStats = collision.gameObject.GetComponent<EnemyStats>();
            if (enemyStats != null)
            {
                enemyStats.DamageEnemy(damage);
            }
        }
    }
}
