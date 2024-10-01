using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelManage : MonoBehaviour
{
    public static LevelManage Instance;
    public GameObject spawnChecker;
    public bool spawnCheck;
    public int levelNum,shopCheckpoints;
    public TMP_Text levelText,highScoreText,scoreText;
    public int CDL;
    public int waves,currentWaveNum;
    public List<Enemy> masterEnemyPool;
    public List<Enemy> currentEnemies;

    void Awake(){
        LevelManage.Instance = this;
    }

    private void Start(){
        levelNum = 1;
        shopCheckpoints = 5;
        setLevelText();
        CDL = 20;
        waves = 1;
        currentWaveNum = waves;
        
        StartCoroutine(GenerateLevel(3));
    }

    private void setLevelText(){
        levelText.text = levelNum.ToString();
    }

    private int calls = 0;
    private List<Enemy> GenerateWaves(){
        calls+=1;
        if(calls >= 500){
            Debug.Log("permIssues 500");
            CDL = (int)(CDL*.9f);
            currentWaveNum = waves;
            calls = 0;
        }
        int waveDifficulty = (int)CDL/waves;
        int waveCost = 0;
        List<Enemy> waveEnemies = new List<Enemy>();
        while (waveCost < waveDifficulty){
            if(waveCost >= 0.8f*waveDifficulty || waveEnemies.Count>15){
                break;
            }
            Enemy attemptedEnemy = masterEnemyPool[(int)Random.Range(0,masterEnemyPool.Count)];
            waveEnemies.Add(attemptedEnemy);
            waveCost+=attemptedEnemy.difficultyWorth;
        }
        if(waveCost <= 1.2f*waveDifficulty && waveEnemies.Count<=15){
            calls = 0;
            return waveEnemies;
        }
        return GenerateWaves();    
    }
    public IEnumerator GenerateLevel(float relax){
        yield return new WaitForSeconds(relax);
        if(currentWaveNum == 0){
            levelNum+=1;
            foreach(Upgrade j in UpgradeManage.Instance.levelUpgrades){
                (j as levelUpgradeType).doUpgrade();
            }
            if(levelNum>shopCheckpoints){
                SceneManage.Instance.shopUp();
                shopCheckpoints+=5;
                waves = (levelNum/7) +1;
                CDL = (int)(CDL*0.9f);
                if(waves!=1){
                    waves = (int)Random.Range(waves-1,waves+1);
                }
                currentWaveNum = waves;
                setLevelText();
            }else{
                waves = (levelNum/7) +1;
                CDL = (int)(CDL*1.2f);
                if(waves!=1){
                    waves = (int)Random.Range(waves-1,waves+1);
                }
                currentWaveNum = waves;
                setLevelText();

                StartCoroutine(GenerateLevel(1));
            }          
        }else{
            List<Enemy> enemies = GenerateWaves();
            StartCoroutine(spawnEnemy(enemies));
            currentWaveNum-=1;
        }
    }
    public IEnumerator spawnEnemy(List<Enemy> enemies){
        foreach(Enemy i in enemies){
            Vector2 pointA = new Vector2(i.transform.localScale.x+.3f,i.transform.localScale.y+.3f)/2;
            Vector2 pointB = -pointA;

            Enemy enemy = Instantiate(i,attemptToSpawn(pointA,pointB,10),Quaternion.identity);
            currentEnemies.Add(enemy);
            yield return new WaitForSeconds(0.25f);
        }
    }
    public Vector2 attemptToSpawn(Vector2 a, Vector2 b,int tries){
        Vector2 originPoint = new Vector2(Random.Range(-8f,8f),Random.Range(-1f,4f));
        Collider2D[] spawnBlockers = Physics2D.OverlapAreaAll(a+originPoint,b+originPoint);
        if(tries > 0 && !isSpawnable(spawnBlockers)){
            if(spawnCheck){
                Instantiate(spawnChecker, new Vector3(originPoint.x,originPoint.y,0),Quaternion.identity);
            }
            return attemptToSpawn(a,b,tries-1);
        }
        return originPoint;
    }

    private bool isSpawnable(Collider2D[] pos){
        bool canSpawn = true;
        foreach(Collider2D i in pos){
            if(i.gameObject.tag != "Coin" && i.gameObject.tag != "BallBullet" && i.gameObject.tag != "EnemyBullet"){
                //Debug.Log(i.gameObject.tag);
                canSpawn = false;
                break;
            }
        }
        return canSpawn;
    }

    public void enemyDied(Enemy enemy){
        currentEnemies.Remove(enemy);
        if(currentEnemies.Count == 0){
            StartCoroutine(GenerateLevel(.5f));
        }
    }

    public void reset(){
        StopCoroutine("GenerateLevel");
        StopCoroutine("spawnEnemy");
        levelNum = 1;
        shopCheckpoints =5;
        CDL = 20;
        waves = 1;
        setLevelText();
        currentWaveNum = waves;
        foreach(var i in currentEnemies){
            Destroy(i.gameObject);
        }
        currentEnemies.Clear();
        StartCoroutine(GenerateLevel(3));
    }

    public void setScore(){
        scoreText.text = "Score\n"+levelNum.ToString();
        PlayerPrefs.SetInt("HighScore",Mathf.Max(levelNum,PlayerPrefs.GetInt("HighScore",0)));
        highScoreText.text = "HighScore\n"+PlayerPrefs.GetInt("HighScore",0).ToString();
    }
}
