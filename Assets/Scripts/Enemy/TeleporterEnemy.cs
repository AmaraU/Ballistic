using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleporterEnemy : Enemy
{
    protected override void onStartMod()
    {
        deathTime = 0;
        moveDirection = Vector2.zero;
        spriteRenderer = GetComponent<SpriteRenderer>();
        StartCoroutine(movement(Random.Range(4,6),true));
    }
    protected override void fixedUpdateMod()
    {
    }

    protected IEnumerator teleport(){
        SpriteRenderer sprite  = GetComponent<SpriteRenderer>();
        GetComponent<Collider2D>().isTrigger = false;
        Canvas spawnAnim = Instantiate(globalVariables.Instance.spawnAnim,transform);
        yield return new WaitForSeconds(.25f);
        sprite.color = sprite.color - new Color(0,0,0,1);
        StartCoroutine(teleportOut(spawnAnim));

    }

    protected IEnumerator teleportOut(Canvas spawnAnim){
        GetComponent<Collider2D>().isTrigger = true;
        yield return new WaitForSeconds(.25f);
        Destroy(spawnAnim.gameObject);
    }

    protected override IEnumerator movement(float time,bool again)
    {
        yield return new WaitForSeconds(time);
        GetComponent<Collider2D>().isTrigger = false;
        StartCoroutine(teleport());
        StartCoroutine(movementMid(again));
    }

    protected IEnumerator movementMid(bool again){
        Vector2 pointA = new Vector2(transform.localScale.x,transform.localScale.y)/2;
        Vector2 pointB = -pointA;
        Vector2 newPos = LevelManage.Instance.attemptToSpawn(pointA,pointB,10);
        yield return new WaitForSeconds(.5f);
        transform.position = new Vector3(0,20,0);
        rigidBody.velocity = Vector2.zero;
        StartCoroutine(movementEnd(newPos,1,again));

    }

    protected IEnumerator movementEnd(Vector2 pos,float time, bool again){

        yield return new WaitForSeconds(time);
        SpriteRenderer sprite  = GetComponent<SpriteRenderer>();
        transform.position = pos;
        GetComponent<Collider2D>().isTrigger = true;
        sprite.color = sprite.color + new Color(0,0,0,1);
        StartCoroutine(spawnInStart());
        if(again){
            StartCoroutine(movement(Random.Range(2,3.5f),again));
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
}
