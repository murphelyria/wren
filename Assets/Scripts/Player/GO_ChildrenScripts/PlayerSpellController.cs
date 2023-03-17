using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpellController : MonoBehaviour
{
    public PlayerStatistics playerStatistics;

    void Awake()
    {
        playerStatistics = FindObjectOfType<PlayerStatistics>(); // get the reference to the PlayerStats component of the player game object
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        int damage = playerStatistics.playerSpellDamage; // defines spell damage from PlayerStats

        // Applies damage when SpellPrefab collides with any enemy
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
