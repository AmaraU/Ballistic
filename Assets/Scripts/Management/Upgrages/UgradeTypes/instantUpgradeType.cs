using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class instantUpgradeType : Upgrade
{
    abstract override public void reset();
    abstract public void doUpgrade();
}
