using UnityEngine;
using System;

public class MinorEnemy : EnemyObject
{
    public MinorEnemy(EnemyData data) : base(data)
    {
        attackDMG = data.attack;

    }

    public override int Attack()
    {
        Debug.Log($"Player took {attackDMG}");
        return 10;
    }
}
    


    

