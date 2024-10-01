using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class extraAmmoUpgrade : bulletCreateUpgradeType
{
    [SerializeField]private bool once = false;
    private void OnEnable(){
        reset();
    }

    public override void doUpgrade(BallBullet bullet,bool act)
    {
        if(!once){
            globalVariables.Instance.ballShoot.setCapacity(globalVariables.Instance.ballShoot.bulletCapacity+3);
            once  = true;
        }
        if(bullet != null){
            bullet.bulletDamage = (int)(bullet.bulletDamage*.8f);
        }
    }

    public override void reset()
    {
        once  = false;
        priority = 5;
    }
}
