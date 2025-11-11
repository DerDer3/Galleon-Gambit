using UnityEngine;
using System.Collections.Generic;

// Renamed from NewCard to CardData
[CreateAssetMenu(fileName = "CardData_", menuName = "Card/Card Data")]
public class CardData : ScriptableObject
{
    public string cardName = "New Card";
    public string description = "Card effect description.";
    public CardEffectType effectType; // For grouping logic
    public List<CardTag> tags;       // E.g., Attack, Skill, Power

    [Header("Stats")]
    public int manaCost;
    public int damage;
    public int block;
    public int heal;

    public enum CardTag
    {
        Attack,
        Skill,
        Power,
        Exhaust
    }

    public enum CardEffectType
    {
        // Use this to differentiate card logic classes (e.g., AttackCard, DefenseCard)
        SimpleDamage,
        SimpleHeal,
        Block,
        DrawCards,
        ComplexEffect
    }
}