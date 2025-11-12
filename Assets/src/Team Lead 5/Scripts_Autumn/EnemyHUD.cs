using System.Data.Common;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public class EnemyHUD : MonoBehaviour
{
    public TextMeshProUGUI enemyName;
    public TextMeshProUGUI enemyTitle;
    public Slider hpSlider;
    public Image enemyImage;


    public void SetHUD(EnemyObject enemy)
    {
       
        hpSlider.maxValue = enemy.maxHealth;
        hpSlider.value = enemy.currentHealth;
    }

    public void SetUp(EnemyData data)
    {
        enemyName.text = data.enemyName;
        enemyTitle.text = data.enemyTitle;
        hpSlider.maxValue = data.health;
        hpSlider.value = data.health;
        
        if (!string.IsNullOrEmpty(data.imagePath))
        {
            Sprite sprite = Resources.Load<Sprite>(data.imagePath);
            if (sprite != null)
                enemyImage.sprite = sprite;
            else
                Debug.LogWarning($"Sprite not found at path: {data.imagePath}");
        }

    }

    public void UpdateHealth(int health)
    {
        hpSlider.value = health;
    }
}
