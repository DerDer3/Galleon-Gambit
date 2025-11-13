using UnityEngine;
using System;

public class MinorEnemy : EnemyObject
{
    public MinorEnemy(EnemyData data) : base(data)
    {
        attackDMG = data.attack;

    }

    public override void Attack()
    {
        Debug.Log($"Player took {attackDMG}");
    }
}
    


    

