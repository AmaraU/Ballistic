using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallBullet : Bullet
{
    public int bulletDamage;
    override protected void OnTriggerEnter2D(Collider2D col)
    {
        if(col.transform.tag == "Enemy"){
            DamageText damage = Instantiate(globalVariables.Instance.damageText,transform.position,Quaternion.identity);
            damage.setText(bulletDamage);
            if(col.gameObject.GetComponent<Enemy>().health>0){
                col.transform.GetComponent<Enemy>().receiveDamage(bulletDamage);
            }
            col.gameObject.GetComponent<Rigidbody2D>().AddForce(rigidBody.velocity/10,ForceMode2D.Impulse);
            StartCoroutine(Die());
            Flash(transform.position,0.045f,new Vector3(3.5f,2.5f,2.5f));
        }else if(col.transform.tag == "Border" || col.transform.tag == "Paddle" || col.transform.tag == "ExtraPaddle"){
            StartCoroutine(Die());
            Flash(transform.position,0.045f,new Vector3(3.5f,2.5f,2.5f));
        } 
    }
    private void OnCollisionEnter2D(Collision2D col) {
        if(col.transform.tag == "Enemy"){
            DamageText damage = Instantiate(globalVariables.Instance.damageText,transform.position,Quaternion.identity);
            damage.setText(bulletDamage);
            if(col.gameObject.GetComponent<Enemy>().health>0){
                col.transform.GetComponent<Enemy>().receiveDamage(bulletDamage);
            }
            //col.gameObject.GetComponent<Rigidbody2D>().AddForce(rigidBody.velocity/10,ForceMode2D.Impulse);
            attemptBounce(col);
            Flash(transform.position,0.045f,new Vector3(3.5f,2.5f,2.5f));
        }else{
            attemptBounce(col);
            Flash(transform.position,0.045f,new Vector3(3.5f,2.5f,2.5f));
        } 
    }

    private void attemptBounce(Collision2D col){
        if(bounce == 0){
            StartCoroutine(Die());
        }else{
            bounce-=1;
            Vector2 normal = col.contacts[0].normal;
            Vector2 vel = -col.relativeVelocity;
            vel = vel.normalized*endVelocity.magnitude;
            vel = vel-2*Vector2.Dot(vel,normal)*normal;
            rigidBody.velocity = vel;
        }
    }

    new protected void FixedUpdate()
    {
        if(rigidBody.velocity.magnitude > endVelocity.magnitude){
            rigidBody.velocity -= rigidBody.velocity*Time.deltaTime*0.5f;
        }
        Vector2 dir = rigidBody.velocity.normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.eulerAngles = new Vector3(0,0,angle);
    }
}
