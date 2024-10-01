using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class healUpgrade : Upgrade
{
    private void Awake() {
        reset();
    }
    public void doUpgrade()
    {
        PaddleHealth paddle  = FindObjectOfType<PaddleHealth>();
        if(paddle.healthCapacity != paddle.healthCount){
            paddle.heal(1);
            cost = (int)(cost*1.2f);
        }
    }
    public override void reset()
    {
        cost = 10;
    }
}
