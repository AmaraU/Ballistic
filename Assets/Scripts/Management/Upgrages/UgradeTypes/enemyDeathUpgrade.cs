using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class enemyDeathUpgrade : Upgrade
{
    abstract override public void reset();
    abstract public void doUpgrade(Enemy enemy);
}
