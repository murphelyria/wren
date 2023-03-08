using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackController : MonoBehaviour
{
    public PlayerStats playerStats;

    void Awake()
    {
        playerStats = FindObjectOfType<PlayerStats>(); // get the reference to the PlayerStats component of the player game object
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        int damage = playerStats.playerAttackDamage;

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
