using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class Enemy : MonoBehaviour
{
    public int difficultyWorth;
    public int coinWorth;
    public FloatingHealthBar healthBar;

    public SpriteRenderer spriteRenderer;
    public int health;
    protected int healthCapacity;

    protected Rigidbody2D rigidBody;
    
    protected Vector2 moveDirection;
    protected bool generated;
    public AudioClip hitSound;
    public AudioSource audioSource;
    public float deathTime;
    public EnemyBullet bullet;

    public float friction;
    

    void Start()
    {
        onStart();
    }

    protected void onStart(){
        generated = false;
        rigidBody = GetComponent<Rigidbody2D>();
        healthCapacity = health;
        healthBar.setSlider((float)health/healthCapacity);
        audioSource = gameObject.AddComponent<AudioSource>();
        onStartMod();
        StartCoroutine(spawnInStart());
    }
    abstract protected void onStartMod();

    protected IEnumerator spawnInStart(){
        SpriteRenderer sprite  = GetComponent<SpriteRenderer>();
        GetComponent<Collider2D>().isTrigger = true;
        sprite.color = sprite.color - new Color(0,0,0,1);
        Canvas spawnAnim = Instantiate(globalVariables.Instance.spawnAnim,transform);
        yield return new WaitForSeconds(.25f);
        sprite.color = sprite.color + new Color(0,0,0,1);
        StartCoroutine(spawnInEnd(spawnAnim));

    }

    protected IEnumerator spawnInEnd(Canvas spawnAnim){
        yield return new WaitForSeconds(.25f);
        Destroy(spawnAnim.gameObject);
        GetComponent<Collider2D>().isTrigger = false;
    }
    public void receiveDamage(int num){
        health-=num;
        audioSource.PlayOneShot(hitSound);
        healthBar.setSlider((float)health/healthCapacity);
        health = Mathf.Max(0,health);
        healthBar.showHealth();
        Flash(transform.position,0.075f,new Vector3(1.15f,1.15f,1.15f));
        if(health==0 && !generated){
            generated = true;
            StartCoroutine(onDeath(deathTime));
            
        }
    }    

    protected void FixedUpdate() {
        rigidBody.velocity = rigidBody.velocity * friction;
        fixedUpdateMod();
    }

    abstract protected IEnumerator onDeathMod(float time);
    abstract protected void fixedUpdateMod();

    protected IEnumerator onDeath(float time){
        StartCoroutine(onDeathMod(time));
        yield return new WaitForSeconds(time);
        foreach(Upgrade j in UpgradeManage.Instance.enemyDeathUpgrades){
            enemyDeathUpgrade eUpgrade = j as enemyDeathUpgrade;
            eUpgrade.doUpgrade(this);
        }
        LevelManage.Instance.enemyDied(this);
        GenerateCoins();
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        foreach(Upgrade j in UpgradeManage.Instance.enemyCollisionUpgrades){
            enemyCollisionUpgrade eUpgrade = j as enemyCollisionUpgrade;
            eUpgrade.doUpgrade(this,col);
        }        
    }
    public void Flash(Vector3 position, float timer,Vector3 scaler){
        Flasher flashed = Instantiate(globalVariables.Instance.flasher,position,Quaternion.identity,transform);
        flashed.sprite = spriteRenderer.sprite;
        flashed.scale = scaler;
        flashed.flashTimer = timer;
    }

    protected void GenerateCoins(){
        while(coinWorth > 0){
            int i = Mathf.Min(Random.Range(1,15),coinWorth);
            coinWorth-=i;
            SquareCoin cash = Instantiate(globalVariables.Instance.coin,transform.position,Quaternion.identity);
            cash.setCost(i);
            cash.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-.8f,.8f),Random.Range(0.1f,.25f)).normalized*.4f,ForceMode2D.Impulse);
        }
        Destroy(gameObject);
    }

    abstract protected IEnumerator movement(float time,bool again);
    abstract protected IEnumerator shoot(float time,bool again);
    
    protected void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.name == "BallZone"){
            rigidBody.velocity = new Vector2(rigidBody.velocity.x,-rigidBody.velocity.y);
        }
    }
}
