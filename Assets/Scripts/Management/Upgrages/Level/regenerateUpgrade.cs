using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class regenerateUpgrade : levelUpgradeType
{
    private void Awake() {
        reset();
    }
    public override void doUpgrade()
    {
        globalVariables.Instance.paddleHealth.heal(1);
    }
    public override void reset()
    {
    }
}
