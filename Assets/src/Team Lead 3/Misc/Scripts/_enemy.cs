using UnityEngine;

namespace GallionGambit
{
    public class Enemy
    {
        public int health = 100;

        public void TakeDamage(int amount)
        {
            health -= amount;
            Debug.Log($"Enemy takes {amount} damage. HP: {health}");
        }

        public void Attack(Player player)
        {
            int damage = Random.Range(5, 15);
            Debug.Log($"Enemy attacks for {damage}!");
            player.TakeDamage(damage);
        }

        public bool IsDead() => health <= 0;
    }
}
