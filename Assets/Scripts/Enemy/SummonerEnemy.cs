using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonerEnemy : Enemy
{
    protected override void onStartMod()
    {
        deathTime = 0;
        moveDirection = Vector2.zero;
        spriteRenderer = GetComponent<SpriteRenderer>();
        StartCoroutine(movement(Random.Range(3,6),true));
        StartCoroutine(shoot(Random.Range(4,8),true));
    }
    protected override void fixedUpdateMod()
    {
    }

    protected override IEnumerator movement(float time,bool again)
    {
        yield return new WaitForSeconds(time);
        moveDirection = Vector2.zero;
        while( moveDirection == Vector2.zero){
            Vector2 newDirection = new Vector2(Random.Range(-.8f,.8f),Random.Range(-.75f,.75f)).normalized;
            if (transform.position.y+newDirection.y > -1){
                moveDirection = newDirection * 5f;
            }else{
                newDirection = new Vector2(Random.Range(-.5f,.5f),Random.Range(0.5f,1)).normalized;
                moveDirection = newDirection * 5f;
            }
        }
        rigidBody.AddForce(moveDirection,ForceMode2D.Impulse);
        if(again){
            StartCoroutine(movement(Random.Range(2,5),again));
        }
    }

    protected override IEnumerator shoot(float time,bool again){
        yield return new WaitForSeconds(time);
        List<Enemy> enemies = new List<Enemy>();
        while(enemies.Count == 0){
            Enemy enemy = LevelManage.Instance.masterEnemyPool[(int)Random.Range(0,LevelManage.Instance.masterEnemyPool.Count)];
            if(enemy.difficultyWorth < difficultyWorth){
                enemies.Add(enemy);
            }
        }
        StartCoroutine(LevelManage.Instance.spawnEnemy(enemies));
        
        if(again){
            StartCoroutine(shoot(Random.Range(5,9),again));
        }
    }
    protected override IEnumerator onDeathMod(float time){
        yield return new WaitForSeconds(time);
    }
}
