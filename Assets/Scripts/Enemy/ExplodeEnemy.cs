using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodeEnemy : Enemy
{
    protected override void onStartMod()
    {
        deathTime = 0;
        moveDirection = Vector2.zero;
        spriteRenderer = GetComponent<SpriteRenderer>();
        StartCoroutine(movement(Random.Range(4,6),true));
    }

    protected override IEnumerator movement(float time,bool again)
    {
        yield return new WaitForSeconds(time);
        moveDirection = Vector2.zero;
        while( moveDirection == Vector2.zero){
            Vector2 newDirection = new Vector2(Random.Range(-.8f,.8f),Random.Range(-.75f,.75f)).normalized;
            if (transform.position.y+newDirection.y > -1){
                moveDirection = newDirection * 7.5f;
            }else{
                newDirection = new Vector2(Random.Range(-.5f,.5f),Random.Range(0.5f,1)).normalized;
                moveDirection = newDirection * 7.5f;
            }
        }
        rigidBody.AddForce(moveDirection,ForceMode2D.Impulse);
        if(again){
            StartCoroutine(movement(Random.Range(5,7),again));
        }
    }

        protected override IEnumerator shoot(float time,bool again){
        yield return new WaitForSeconds(time);
        int i = Random.Range(0,100);
        Vector2 shootVector = Vector2.down;
        if(i < 30){
            Vector2 paddleVector  = (Vector2)(globalVariables.Instance.paddleHealth.transform.position - transform.position).normalized;
            shootVector = new Vector2(Random.Range(paddleVector.x,0),Random.Range(-1,paddleVector.y)).normalized;
        }
        EnemyBullet bul = Instantiate(bullet,(Vector2)transform.position+shootVector,Quaternion.identity);
        bul.endVelocity = shootVector*5;
        bul.transform.eulerAngles = new Vector3(0,0,Mathf.Atan2(shootVector.y,shootVector.x)*180/Mathf.PI);
        Flash(transform.position,0.075f,new Vector3(1.15f,1.15f,1.15f));
        if(again){
            StartCoroutine(shoot(Random.Range(3,7),again));
        }
    }
    protected override IEnumerator onDeathMod(float time){
        for(int i=0;i<16;i++){
            EnemyBullet bul = Instantiate(bullet,(Vector2)transform.position,Quaternion.identity);
            float rads = 2* Mathf.PI/16*i;
            float vert = Mathf.Sin(rads);
            float horz = Mathf.Cos(rads);
            Vector2 direction = new Vector2(horz,vert);
            bul.endVelocity = direction*7.5f;
            bul.transform.eulerAngles = new Vector3(0,0,Mathf.Atan2(vert,horz)*180/Mathf.PI);
        }
        yield return new WaitForSeconds(time);
    }

    new protected void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.name == "BallZone"){
            rigidBody.velocity = new Vector2(rigidBody.velocity.x,-rigidBody.velocity.y);
        }
        if(col.gameObject.tag == "EnemyBullet"){
            receiveDamage(healthCapacity);
        }
    }
    protected override void fixedUpdateMod()
    {
    }
}
