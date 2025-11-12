<<<<<<< HEAD
=======
using System.Data.Common;
using TMPro;
>>>>>>> 373c74afd004e1123c18fa3e3863f14566d8cd9f
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public class EnemyHUD : MonoBehaviour
{
<<<<<<< HEAD
    public string enemyName = "james"; //will be text in the future
    public Slider hpSlider;
=======
    public TextMeshProUGUI enemyName;
    public TextMeshProUGUI enemyTitle;
    public Slider hpSlider;
    public Image enemyImage;
>>>>>>> 373c74afd004e1123c18fa3e3863f14566d8cd9f


    public void SetHUD(EnemyObject enemy)
    {
<<<<<<< HEAD
        //enemyName.text = enemy.enemyName;
        hpSlider.maxValue = enemy.maxHealth;
        hpSlider.value = enemy.currentHealth;
=======
       
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
>>>>>>> 373c74afd004e1123c18fa3e3863f14566d8cd9f

    }

    public void UpdateHealth(int health)
    {
        hpSlider.value = health;
    }
}
