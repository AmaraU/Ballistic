using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fiveExtraHealthCapUpgrade : instantUpgradeType
{
    private void Awake() {
        reset();
    }

    public override void doUpgrade()
    {
        globalVariables.Instance.paddleHealth.setCapacity(globalVariables.Instance.paddleHealth.healthCapacity+5,globalVariables.Instance.paddleHealth.healthCount);
    }

    public override void reset()
    {
    }
}
