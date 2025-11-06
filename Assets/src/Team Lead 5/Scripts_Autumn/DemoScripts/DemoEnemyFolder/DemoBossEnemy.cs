using UnityEngine;
using System;
public class DemoBossEnemy : DemoEnemyObject
{

    public DemoBossEnemy(DemoEnemyData data)
    {
        attackDMG = data.attack;
    }
    public override void Attack()
    {
        Debug.Log($"Player took {attackDMG}");
    }

}
