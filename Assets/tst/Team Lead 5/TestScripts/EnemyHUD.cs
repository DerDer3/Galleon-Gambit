using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public class EnemyHUD : MonoBehaviour
{
    public string enemyName = "james"; //will be text in the future
    public Slider hpSlider;


    public void SetHUD(Enemy enemy)
    {
        //enemyName.text = enemy.enemyName;
        hpSlider.maxValue = enemy.maxHealth;
        hpSlider.value = enemy.currentHealth;

    }

    public void UpdateHealth(int health)
    {
        hpSlider.value = health;
    }
}
