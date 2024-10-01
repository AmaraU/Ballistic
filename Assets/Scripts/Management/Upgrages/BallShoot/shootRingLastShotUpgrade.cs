using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shootRingLastShotUpgrade : ballShootUpgrade
{
    public override void doUpgrade()
    {
        if(globalVariables.Instance.ballShoot.bulletCount==0){
            StartCoroutine(ringShot());
        }
    }

    IEnumerator ringShot(){
        for(int i=0;i<6;i++){
            float rads = 2* Mathf.PI/6*i;
            float vert = Mathf.Sin(rads);
            float horz = Mathf.Cos(rads);
            Vector2 direction = new Vector2(horz,vert).normalized;
            globalVariables.Instance.ballShoot.Shoot(direction);
            yield return new WaitForSeconds(.05f);
        }
    }
    public override void reset()
    {
    }
}
