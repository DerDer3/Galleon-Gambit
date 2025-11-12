using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using GallionGambit;
using Unity.VisualScripting;

public class CardDisplay : MonoBehaviour
{
    public NewCard cardData;

    public Image cardImage;

    public TMP_Text nameText;
    public TMP_Text healthText;
    public TMP_Text manaText;
    public TMP_Text damageText;

    public Image[] typeImages;
    //0 - Heal; 1 - Mana; 2 - Damage;

    private Color[] typeColors =
    {
        Color.red, //Heal
        Color.orange, //Damage
        Color.blue //Mana
    };

    public void ApplyCardEffects()
    {
        if (cardData == null)
        {
            Debug.LogError("Card data is missing!");
            return;
        }

        // Apply Heal to the player (assuming Player_health property)
        if (cardData.heal > 0)
        {
            GameManager2.Instance.Player_health += cardData.heal;
            Debug.Log($"Player Healed for {cardData.heal}. New Health: {GameManager2.Instance.Player_health}");
        }

        // Apply Mana effect
        if (cardData.mana != 0)
        {
            GameManager2.Instance.Player_mana += cardData.mana;
            Debug.Log($"Player Mana changed by {cardData.mana}. New Mana: {GameManager2.Instance.Player_mana}");
        }

    }

    public void UpdateCardDisplay()
    {
        cardImage.color = typeColors[(int)cardData.cardType[0]];

        nameText.text = cardData.cardName;

        if (cardData.heal == 0) { healthText.text = " "; } 
        else {
            healthText.text = cardData.heal.ToString();
            activateImages(0);
        }

        if (cardData.damage == 0) { damageText.text = " "; } 
        else {
            activateImages(1);
            damageText.text = cardData.damage.ToString();
        }

        if (cardData.mana == 0) { manaText.text = " "; } 
        else {
            manaText.text = cardData.mana.ToString();
            if(cardData.mana > 0) { activateImages(2);}
        }
        
    }
    
    public void activateImages(int img)
    { //TypeImages: 0 - Heal; 1 - Mana; 2 - Damage;

        switch (img)
        {
            case 0://Heal
                typeImages[0].gameObject.SetActive(true);
                typeImages[1].gameObject.SetActive(false);
                typeImages[2].gameObject.SetActive(false);
                break;
            case 1://Damage
                typeImages[0].gameObject.SetActive(false);
                typeImages[1].gameObject.SetActive(false);
                typeImages[2].gameObject.SetActive(true);
                break;
            case 2://Mana
                typeImages[0].gameObject.SetActive(false);
                typeImages[1].gameObject.SetActive(true);
                typeImages[2].gameObject.SetActive(false);
                break;
        }
    }
}
