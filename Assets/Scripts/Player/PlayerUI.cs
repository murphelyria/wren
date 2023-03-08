using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    public PlayerStats playerStats;
    public Image healthBar;
    public Image manaBar;

    void Update()
    {
        healthBar.fillAmount = (float)playerStats.playerHealth / playerStats.playerMaxHealth;
        manaBar.fillAmount = playerStats.playerMana / playerStats.playerMaxMana;
    }
}