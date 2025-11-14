using UnityEngine;
using TMPro;
using System;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine.UI;
public abstract class EnemyObject
{
    public string enemyName { get; protected set; }
    public string enemyTitle { get; protected set; }
    public float maxHealth { get; protected set; }
    public float currentHealth { get; protected set; }
    public float attackDMG { get; protected set; }

    public EnemyObject(EnemyData data)
    {
        enemyName = data.enemyName;
        enemyTitle = data.enemyTitle;
        maxHealth = data.health;
        currentHealth = maxHealth;
        attackDMG = data.attack;

    }


    public virtual int Attack()
    {
        Debug.Log($"Attacked Player for {attackDMG} amt");
        return 10;
    }

    public virtual void TakeDamage(int amt)
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
        Debug.Log("Enemy Died! Player won!");
        
    }
    
   

}
