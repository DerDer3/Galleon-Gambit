using UnityEngine;

public class EnemyObject : MonoBehaviour
{
    public string enemyName;
    public int unitLevel;
    public int damage;
    public int maxHealth;

    public int currentHealth;


    public bool TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public int get_health() { return currentHealth; }
    public void set_health(int x) { currentHealth = x; }

}