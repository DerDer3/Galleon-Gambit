using UnityEngine;
using System.Collections.Generic;

namespace GallionGambit
{
    [CreateAssetMenu(fileName = "New Card", menuName = "Card")]
    public class Card : ScriptableObject
    {
        public string cardName;

        public List<CardType> cardType;

        public int damage;

        public int heal;

        public int manaCost;

        public List<CardEffect> effect;

        public enum CardType
        {
            Heal,
            Damage,
            other
        }

        public enum CardEffect
        {
            effect1,
            effect2,
            effect3
        }
    }
}