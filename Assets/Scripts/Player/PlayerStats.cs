using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public int playerHealth = 5;
    public int playerMaxHealth = 5;
    public int playerAttackDamage = 5;
    public int playerSpellDamage = 15;
    public float playerMana = 99f;
    public int playerMaxMana = 99;
    public float manaRegainRate = 3.3f;
    public float invulnerabilityTime = 3f; // the duration of the invulnerability after taking damage
    public bool isInvulnerable = false; // flag to indicate if the player is currently invulnerable

    private int healAmount = 1;
    public int spellManaCost = 33;
    public int healManaCost = 33;

    void Start()
    {
        StartCoroutine(RegainManaOverTime());
    }


    public void DamagePlayer(int damageAmount)
    {
        if (!isInvulnerable) // only take damage if not already invulnerable
        {
            playerHealth -= damageAmount;
            if (playerHealth <= 0)
            {
                playerHealth = 0;
                // call function to handle player death
                // you can replace this with whatever function you want to call
                Debug.Log("Player died!");
            }
            else
            {
                isInvulnerable = true;
                Invoke("ResetInvulnerability", invulnerabilityTime);
            }
        }
    }

    public void HealPlayer()
    {
        playerHealth += healAmount;
    }


    private void ResetInvulnerability()
    {
        isInvulnerable = false;
    }

    IEnumerator RegainManaOverTime()
    {
        while (true)
        {
            if (playerMana < playerMaxMana)
            {
                float regainedMana = manaRegainRate * Time.deltaTime;
                playerMana = Mathf.Min(playerMana + regainedMana, playerMaxMana);
            }
            yield return null;
        }
    }
}