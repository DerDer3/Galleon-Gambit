using UnityEngine;
using System.Collections.Generic;

namespace GallionGambit
{
    [CreateAssetMenu(fileName = "New Card", menuName = "Card")]
    public class NewCard : ScriptableObject
    {
        public string cardName;

        public List<CardType> cardType;

        public int damage;

        public int heal;

        public int mana;

        public enum CardType
        {
            Heal,
            Damage,
            Mana
        }

    }
}