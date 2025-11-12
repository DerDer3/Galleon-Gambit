using UnityEngine;
<<<<<<< HEAD

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
=======
using TMPro;
using System;
using Unity.IO.LowLevel.Unsafe;
public abstract class EnemyObject
{
    public TMP_Text enemyName;
    public TMP_Text enemyTitle;
    public float maxHealth;
    public float currentHealth;
    public float attackDMG;


    public virtual void Attack()
    {
        Debug.Log($"Attacked Player for {attackDMG} amt");
    }

    public void TakeDamage(int amt)
    {
        currentHealth -= amt;
        if (currentHealth <= 0)
        {
            Die();
        }
        Debug.Log("Enemy took damage!");
    }

    public void Die()
    {
        Debug.Log("Enemy Died");
        OnDeath?.Invoke(this);
    }
    
    public event Action<EnemyObject> OnDeath;

}
>>>>>>> 373c74afd004e1123c18fa3e3863f14566d8cd9f
