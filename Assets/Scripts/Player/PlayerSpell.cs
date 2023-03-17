using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpell : MonoBehaviour
{
    public GameObject spellPrefab;

    private float spellTimer;
    private float spellHoldTime;

    private void Update()
    {
        if (spellTimer > 0)
        {
            spellTimer -= Time.deltaTime;
        }
    }

    public void HandleSpell()
    {
        if (spellTimer <= 0 && PlayerStatistics.instance.playerMana >= 33)
        {
            StartCoroutine(CastSpell());
        }
        else if (spellTimer <= 0 && PlayerStatistics.instance.playerMana < 33)
        {
            Debug.Log("Player doesn't have enough mana!");
        }
    }

    IEnumerator CastSpell()
    {
        spellHoldTime = 0f;
        while (Input.GetButton("Spell") && spellHoldTime < PlayerStatistics.instance.healThreshold)
        {
            spellHoldTime += Time.deltaTime;
            yield return null;
        }

        if (spellHoldTime >= PlayerStatistics.instance.healThreshold && PlayerStatistics.instance.playerHealth < PlayerStatistics.instance.playerMaxHealth)
        {
            yield return StartCoroutine(DrainManaOverTime());
        }
        else
        {
            GameObject spellObject = Instantiate(spellPrefab, transform.position, Quaternion.identity);
            int spellDirection = PlayerStatistics.instance.facingRight ? 1 : -1;
            Rigidbody2D spellRB = spellObject.GetComponent<Rigidbody2D>();
            spellRB.velocity = new Vector2(PlayerStatistics.instance.spellSpeed * spellDirection, 0);
            Destroy(spellObject, PlayerStatistics.instance.spellDuration);
            PlayerStatistics.instance.playerMana -= PlayerStatistics.instance.spellManaCost;
        }

        spellHoldTime = 0f;
        yield return null;
        spellTimer = PlayerStatistics.instance.spellCooldown;
    }

    IEnumerator DrainManaOverTime()
    {
        float manaDrained = 0f;
        float newManaValue = PlayerStatistics.instance.playerMana;

        while (manaDrained < PlayerStatistics.instance.spellManaCost)
        {
            newManaValue -= PlayerStatistics.instance.manaDrainRate * Time.deltaTime;
            manaDrained += PlayerStatistics.instance.manaDrainRate * Time.deltaTime;
            PlayerStatistics.instance.playerMana = newManaValue;
            yield return null;
        }

        PlayerStatistics.instance.playerMana = Mathf.RoundToInt(newManaValue);
        HealPlayer();
    }

    void HealPlayer()
    {
        PlayerStatistics.instance.playerHealth++;
    }
}

//      Needs damage dealing code