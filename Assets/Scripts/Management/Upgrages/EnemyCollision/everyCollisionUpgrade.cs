using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class everyCollisionUpgrade : enemyCollisionUpgrade
{
    public override void reset()
    {
    }

    public override void doUpgrade(Enemy enemy, Collision2D col)
    {
        if(col.gameObject.name != "Ball" && col.gameObject.tag != "BallBullet"){
            DamageText damageText = Instantiate(globalVariables.Instance.damageText,col.GetContact(0).point,Quaternion.identity);
            int colDamage = (int)Mathf.Pow(col.relativeVelocity.magnitude,1.35f);
            damageText.setText(colDamage);
            if(enemy.health>0){
                enemy.receiveDamage(colDamage);
            }
        }
    }
}
