using UnityEngine;
using System;

public class DemoMinorEnemy : DemoEnemyObject
{
    public DemoMinorEnemy(DemoEnemyData data)
    {
       
        attackDMG = data.attack;
    }

    public override void Attack()
    {
        Debug.Log($"Player took {attackDMG}");
    }
}
    


    

