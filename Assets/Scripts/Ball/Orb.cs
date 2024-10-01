using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orb : MonoBehaviour
{
    public int damage =0;
    public SpriteRenderer spriteRenderer;

    void Start()
    {
        damage = 0;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void OnTriggerStay2D(Collider2D col)
    {
        if(col.gameObject.tag == "Enemy"){
            int instant = (int)(globalVariables.Instance.ballShoot.getBulletDamage()*.15f);
            damage += instant;
            if(col.gameObject.GetComponent<Enemy>().health>0){
                col.transform.GetComponent<Enemy>().receiveDamage(instant);
            }
        }
        if(col.gameObject.name != "BallZone" && col.gameObject.tag != "Coin"){
            Flash(transform.position,0.075f,new Vector3(1.15f,1.15f,1.15f));
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if(col.gameObject.tag == "Enemy"){
            DamageText dText = Instantiate(globalVariables.Instance.damageText,transform.position,Quaternion.identity);
            dText.setText(damage);
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        damage = 0;
        if(col.gameObject.tag == "Enemy"){
            int instant = (int)(globalVariables.Instance.ballShoot.getBulletDamage()*.15f);
            damage += instant;
            if(col.gameObject.GetComponent<Enemy>().health>0){
                col.transform.GetComponent<Enemy>().receiveDamage(instant);
            }
        }
    }

    public void Flash(Vector3 position, float timer,Vector3 scaler){
        Flasher flashed = Instantiate(globalVariables.Instance.flasher,position,Quaternion.identity,transform);
        flashed.sprite = spriteRenderer.sprite;
        flashed.scale = scaler;
        flashed.flashTimer = timer;
    }
}
