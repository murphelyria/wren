using UnityEngine;

public class Sw_EnemyStats : MonoBehaviour
{
    public int enemyHealth = 15;
    public int enemyMaxHealth = 15;
    public float invulnerabilityTime = 2f;
    public bool isInvulnerable = false;

    private Sw_EnemyController enemyController;

    private void Start()
    {
        enemyController = GetComponent<Sw_EnemyController>();
    }

    public void DamageEnemy(int damageAmount)
    {
        if (!isInvulnerable)
        {
            enemyHealth -= damageAmount;
            if (enemyHealth <= 0)
            {
                enemyHealth = 0;
                Die();
            }
            else
            {
                isInvulnerable = true;
                Invoke("ResetInvulnerability", invulnerabilityTime);
            }
        }
    }

    private void ResetInvulnerability()
    {
        isInvulnerable = false;
    }

    private void Die()
    {
        // Disable the EnemyController script
        enemyController.enabled = false;
        // Attach the EnemyDeathController script
        gameObject.AddComponent<EnemyDeathController>();
        // Remove this script from the game object
        Destroy(this);
    }
}