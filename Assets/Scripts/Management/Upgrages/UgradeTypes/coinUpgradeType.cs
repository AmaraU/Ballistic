using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class coinUpgradeType : Upgrade
{
    abstract override public void reset();
    abstract public void doUpgrade(SquareCoin coin);
}
