using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    public PlayerStatistics playerStatistics;
    public Image healthBar;
    public Image manaBar;

    void Update()
    {
        healthBar.fillAmount = (float)playerStatistics.playerHealth / playerStatistics.playerMaxHealth;
        manaBar.fillAmount = playerStatistics.playerMana / playerStatistics.playerMaxMana;
    }
}