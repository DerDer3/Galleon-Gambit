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
        maxHealth -= amt;
        Debug.Log("Enemy took damage!");
    } 

}
