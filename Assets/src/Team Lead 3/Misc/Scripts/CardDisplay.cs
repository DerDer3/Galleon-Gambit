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

    private Color[] typeColors =
    {
        Color.red, //Heal
        Color.orange, //Damage
        Color.blue //Mana
    };

    private void Start()
    {//UPDATE THIS TO BE ON GAME MANAGER
        UpdateCardDisplay();
    }

    public void UpdateCardDisplay()
    {
        cardImage.color = typeColors[(int)cardData.cardType[0]];

        nameText.text = cardData.cardName;
        healthText.text = cardData.ToString();
        damageText.text = cardData.ToString();
        manaText.text = cardData.ToString();

        for (int i = 0; i < typeImages.Length; i++)
        {
            if (i < cardData.cardType.Count)
            {
                typeImages[i].gameObject.SetActive(true);
            }
        }
    }


}
