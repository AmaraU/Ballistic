using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class upLastShotUpgrade : ballShootUpgrade
{
    public override void doUpgrade()
    {
        if(globalVariables.Instance.ballShoot.bulletCount==0){
            globalVariables.Instance.ballMovement.GetComponent<Rigidbody2D>().AddForce(Vector2.up*7.5f,ForceMode2D.Impulse);
        }
    }
    public override void reset()
    {
    }
}
