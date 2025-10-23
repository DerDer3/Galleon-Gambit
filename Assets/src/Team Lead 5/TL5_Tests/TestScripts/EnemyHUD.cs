using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public class EnemyHUD : MonoBehaviour
{
    public string enemyName; //will be text in the future
    public Slider hpSlider;


    public void SetHUD(Enemy enemy)
    {
        if (enemy == null) return;

        if (enemyName != null)
            //string
            enemyName = "james";
        else
            //Debug.LogWarning("EnemyHUD.nameText not assigned.");

        if (hpSlider != null)
        {
            hpSlider.maxValue = enemy.maxHealth;
            hpSlider.value = enemy.currentHealth;
        }
        else
        {
            //Debug.LogWarning("EnemyHUD.healthSlider not assigned.");
        }

    }

    public void UpdateHealth(int health)
    {
        hpSlider.value = health;
    }
}
