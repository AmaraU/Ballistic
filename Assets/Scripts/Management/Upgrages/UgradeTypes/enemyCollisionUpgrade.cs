using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class enemyCollisionUpgrade : Upgrade
{
    abstract override public void reset();
    abstract public void doUpgrade(Enemy enemy,Collision2D col);
}
