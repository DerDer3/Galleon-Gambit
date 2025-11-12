using UnityEngine;
using System;
public class BossEnemy : EnemyObject
{

    public BossEnemy(EnemyData data) : base(data)
    {
        attackDMG = data.attack;
    }
    public override void Attack()
    {
        Debug.Log($"Player took {attackDMG}");
    }

}
