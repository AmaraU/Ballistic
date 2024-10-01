using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class ballShootUpgrade : Upgrade
{
    abstract override public void reset();
    abstract public void doUpgrade();
}
