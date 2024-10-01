using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TacklerEnemy : Enemy
{
    protected override void onStartMod()
    {
        deathTime = 0;
        moveDirection = Vector2.zero;
        spriteRenderer = GetComponent<SpriteRenderer>();
        StartCoroutine(movement(Random.Range(3,6),true));
    }

    protected override IEnumerator movement(float time,bool again)
    {
        yield return new WaitForSeconds(time);
        if(globalVariables.Instance.ballMovement.transform.position.y>-1 && Vector3.Distance(transform.position,globalVariables.Instance.ballMovement.transform.position)<8){
            Vector2 predictedPoint = (Vector2)globalVariables.Instance.ballMovement.transform.position + globalVariables.Instance.ballMovement.GetComponent<Rigidbody2D>().velocity * Time.deltaTime;

            Vector2 targetVector = (predictedPoint - (Vector2)transform.position);
            float distance = targetVector.magnitude;
            targetVector = targetVector.normalized;
            rigidBody.AddForce(targetVector*distance*7.5f,ForceMode2D.Impulse);
        }
        else{
            moveDirection = Vector2.zero;
            while( moveDirection == Vector2.zero){
                Vector2 newDirection = new Vector2(Random.Range(-.8f,.8f),Random.Range(-.75f,.75f)).normalized;
                if (transform.position.y+newDirection.y > -1){
                    moveDirection = newDirection * 10f;
                }else{
                    newDirection = new Vector2(Random.Range(-.5f,.5f),1).normalized;
                    newDirection += (new Vector2(transform.position.x,-1) -(Vector2)transform.position)/10;
                    moveDirection = newDirection * 10f;
                }
            }
            rigidBody.AddForce(moveDirection,ForceMode2D.Impulse);
        }
        if(again){
            StartCoroutine(movement(Random.Range(6,9),again));
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
        yield return new WaitForSeconds(time);
    }

    protected override void fixedUpdateMod()
    {
    }
}
