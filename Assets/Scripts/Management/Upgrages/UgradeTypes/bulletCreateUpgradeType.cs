using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class bulletCreateUpgradeType : Upgrade
{
    public int priority;
    abstract override public void reset();
    abstract public void doUpgrade(BallBullet bullet,bool act);
}
