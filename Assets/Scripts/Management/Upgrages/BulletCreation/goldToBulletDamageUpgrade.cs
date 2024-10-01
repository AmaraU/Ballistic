using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class goldToBulletDamageUpgrade : bulletCreateUpgradeType
{
    private void Awake() {
        reset();
    }

    public override void doUpgrade(BallBullet bullet,bool act)
    {
        if(bullet != null){
            bullet.bulletDamage = Mathf.Min((int)(.075f*globalVariables.Instance.paddleMovement.desiredGoldAmount),50);
            if(act){
                globalVariables.Instance.paddleMovement.removeGold((int)(0.01f*globalVariables.Instance.paddleMovement.desiredGoldAmount));
            }
        }
    }

    public override void reset()
    {
        priority = 8;
    }
}
