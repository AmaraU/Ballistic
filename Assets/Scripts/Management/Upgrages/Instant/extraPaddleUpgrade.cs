using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class extraPaddleUpgrade : instantUpgradeType
{
    public ExtraPaddle extraPaddle;
    private GameObject ePaddle = null;
    private void Awake() {
        reset();
    }

    public override void doUpgrade()
    {
        ePaddle = Instantiate(extraPaddle,new Vector3(0,-4.9f,0),Quaternion.identity).gameObject;
    }

    public override void reset()
    {
        if(ePaddle != null){
            Destroy(ePaddle);
        }
        ePaddle = null;
    }
}
