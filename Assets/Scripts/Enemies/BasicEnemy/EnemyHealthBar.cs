using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    public Image healthBar;
    public Transform target;
    public static EnemyHealthBar instance;

    private void Awake()
    {
        GameObject canvas = GameObject.Find("EnemyHealth"); // Find the UI canvas game object
        healthBar = canvas.transform.Find("EnemyHealthBar").GetComponent<Image>(); // Find the EnemyHealthBar child image
    }

    private void Update()
    {
        instance = this;

        EnemyStats enemy = target.GetComponent<EnemyStats>();
        if (enemy != null)
        {
            // Position the health bar above the target
            Vector3 screenPos = Camera.main.WorldToScreenPoint(target.position);
            screenPos.y += 50f;
            healthBar.transform.position = screenPos;

            // Update the fill amount based on the target's health
            float fillAmount = (float)enemy.enemyHealth / enemy.enemyMaxHealth;
            healthBar.fillAmount = fillAmount;
        }
        else
        {
            Destroy(healthBar.transform.parent.gameObject);

            instance.enabled = false;
        }
    }
}
