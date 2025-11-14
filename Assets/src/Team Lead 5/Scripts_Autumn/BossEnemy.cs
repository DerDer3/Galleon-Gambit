using UnityEngine;
using System;
public class BossEnemy : EnemyObject
{

    public BossEnemy(EnemyData data) : base(data)
    {
        attackDMG = data.attack;
    }
    public override int Attack()
    {
        Debug.Log($"Player took {attackDMG}");

        return 20;
    }

}
