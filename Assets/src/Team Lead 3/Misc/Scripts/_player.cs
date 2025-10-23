using UnityEngine;

namespace GallionGambit
{
    public class Player
    {
        public int health = 100;
        public int defense = 0;
        public int mana = 5;

        public void Heal(int amount)
        {
            health = Mathf.Min(health + amount, 100);
            Debug.Log($"Player heals {amount}. HP: {health}");
        }

        public void TakeDamage(int amount)
        {
            int dmg = Mathf.Max(amount - defense, 0);
            health -= dmg;
            defense = 0;
            Debug.Log($"Player takes {dmg} damage. HP: {health}");
        }

        public void AddDefense(int amount)
        {
            defense += amount;
            Debug.Log($"Player gains {amount} defense.");
        }

        public bool IsDead() => health <= 0;

        public void SpendMana(int cost)
        {
            mana = Mathf.Max(0, mana - cost);
        }

        public void RefillMana()
        {
            mana = 5;
        }
    }
}
