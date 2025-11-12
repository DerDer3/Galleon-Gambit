using UnityEngine;
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
