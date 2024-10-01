using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class healOnDeathUpgrade : enemyDeathUpgrade
{
    public override void reset()
    {
    }

    public override void doUpgrade(Enemy enemy)
    {
        int chance = Random.Range(1,101);
        if(chance <= 15){
            globalVariables.Instance.paddleHealth.heal(1);
        }
    }
}
