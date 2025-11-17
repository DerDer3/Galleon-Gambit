using UnityEngine;
using System;
public class BossEnemy : EnemyObject
{

    public BossEnemy(EnemyData data) : base(data)
    {
        attackDMG = data.attack;
        enemyName = data.enemyName;
        enemyTitle = data.enemyTitle;
        maxHealth = data.health;
        currentHealth = maxHealth;
        attackDMG = data.attack;

    }
    public override int Attack()
    {
        Debug.Log($"Player took {attackDMG}");

        return attackDMG;
    }

}
