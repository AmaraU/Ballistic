using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class blackHoleOnDeathUpgrade : enemyDeathUpgrade
{
    public Sprite circle;
    public override void reset()
    {
    }

    public override void doUpgrade(Enemy enemy)
    {
        Vector2 originPoint = enemy.transform.position;
        Collider2D[] satellites = Physics2D.OverlapAreaAll(originPoint+Vector2.one*2.5f,originPoint-Vector2.one*2.5f);
        blackHole(originPoint);
        foreach(Collider2D i in satellites){
            if(i.gameObject.tag == "Enemy"){
                Vector2 direction = (originPoint - (Vector2)i.transform.position).normalized * 10f;
                i.GetComponent<Rigidbody2D>().AddForce(direction,ForceMode2D.Impulse);
            }
        }
    }

    void blackHole(Vector2 pos){
        Blinker blinker = Instantiate(globalVariables.Instance.blinker,pos,Quaternion.identity);
        blinker.sprite = circle;
        blinker.scale = new Vector3(5,5,0);
        blinker.blinkTimer = .5f;
        blinker.spriteRenderer.color = new Color(0,0,0);
        blinker.alpha = 1f;
        blinker.endScale = new Vector3(.25f,.25f,.25f);
    }
}
