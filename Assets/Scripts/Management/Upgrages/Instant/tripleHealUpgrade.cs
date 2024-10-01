using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tripleHealUpgrade : instantUpgradeType
{
    private void Awake() {
        reset();
    }

    public override void doUpgrade()
    {
        globalVariables.Instance.paddleHealth.heal(3);
    }

    public override void reset()
    {
    }
}
