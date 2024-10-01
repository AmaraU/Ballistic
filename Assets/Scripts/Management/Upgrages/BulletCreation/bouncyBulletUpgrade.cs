using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bouncyBulletUpgrade : bulletCreateUpgradeType
{
    [SerializeField]private bool once = false;
    private void OnEnable(){
        reset();
    }

    public override void doUpgrade(BallBullet bullet,bool act)
    {
        if(!once){
            globalVariables.Instance.ballShoot.setCapacity(globalVariables.Instance.ballShoot.bulletCapacity-1);
            once  = true;
        }
        if(bullet != null){
            bullet.bounce+=2;
            bullet.bulletDamage = (int)(bullet.bulletDamage*.75f);
        }
    }

    public override void reset()
    {
        once  = false;
        priority = 9;
    }
}
