using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using GallionGambit;
using Unity.VisualScripting;
public struct CardStats
{
    public string Name;
    public GallionGambit.NewCard.CardType Type;
    public int Damage;
    public int Heal;
    public int ManaCost;
    public int ManaGain; // Separate value for positive mana effects

    public CardStats(GallionGambit.NewCard cardData)
    {
        Name = cardData.cardName;
        Type = cardData.cardType[0];
        Damage = cardData.damage;
        Heal = cardData.heal;

        // Mana field holds cost (negative) or gain (positive)
        if (cardData.mana < 0)
        {
            ManaCost = Mathf.Abs(cardData.mana);
            ManaGain = 0;
        }
        else
        {
            ManaCost = 0;
            ManaGain = cardData.mana;
        }
    }
}
