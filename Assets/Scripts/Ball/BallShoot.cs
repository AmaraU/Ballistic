using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Utils;

public class BallShoot : MonoBehaviour
{
    public TMP_Text bulletText,shopBulletText;
    public Rigidbody2D projectile;
    public int bulletCapacity;
    public int bulletCount;
    public float shootPower,fireRate,reloadRate;
    public bool canShoot = true;
    private Rigidbody2D rigidBody;
    public float recoilFactor;
    public int bulletDamage;
    private bool isReloading = false;

    public AudioClip ballShot,ballReload;
    private AudioSource ballShootingAudioSource;
    public Transform pointer;

    void Start()
    {
        shootPower = 15;
        fireRate = .15f;
        reloadRate = .3f;
        recoilFactor = .175f;
        bulletDamage = 15;

        isReloading = false;
        bulletCount = bulletCapacity;
        rigidBody = GetComponent<Rigidbody2D>();
        barBullets = new List<RectTransform>(bulletCapacity);
        shopBarBullets = new List<RectTransform>(bulletCapacity);
        setCapacity(6);
        ballShootingAudioSource = transform.GetComponent<AudioSource>();
    }    

    public RectTransform barBullet,bulletTank,shopBulletTank;
    private List<RectTransform> barBullets,shopBarBullets;

    public void setCapacity(int num){
        foreach(var j in barBullets){
            Destroy(j.gameObject);
        }
        barBullets.Clear();

        foreach(var j in shopBarBullets){
            Destroy(j.gameObject);
        }
        shopBarBullets.Clear();
        

        bulletCapacity = num;
        float space =40/(bulletCapacity+1);
        for(int i = 0;i<bulletCapacity;i++){
            RectTransform bul = Instantiate(barBullet);
            bul.SetParent(bulletTank.transform,false);
            bul.localPosition = new Vector3(space*(i+1)+(347-space*(bulletCapacity+1))/bulletCapacity*i,0,0);
            bul.sizeDelta = new Vector2((347-space*(bulletCapacity+1))/bulletCapacity,-4);
            barBullets.Add(bul);
        }

        for(int i = 0;i<bulletCapacity;i++){
            RectTransform bul = Instantiate(barBullet);
            bul.SetParent(shopBulletTank.transform,false);
            bul.localPosition = new Vector3(space*(i+1)+(347-space*(bulletCapacity+1))/bulletCapacity*i,0,0);
            bul.sizeDelta = new Vector2((347-space*(bulletCapacity+1))/bulletCapacity,-4);
            shopBarBullets.Add(bul);
        }
        bulletCount = bulletCapacity;
        colorBarBullets();
    }

    void colorBarBullets(){
        for(int i = 0;i<bulletCapacity;i++){
            if(i>bulletCount-1){
                barBullets[i].GetComponent<Image>().color = Color.grey;
            }
            else{
                barBullets[i].GetComponent<Image>().color = Color.white;
            }
        }
        bulletText.text = bulletCount.ToString();

        for(int i = 0;i<bulletCapacity;i++){
            if(i>bulletCount-1){
                shopBarBullets[i].GetComponent<Image>().color = Color.grey;
            }
            else{
                shopBarBullets[i].GetComponent<Image>().color = Color.white;
            }
        }
        shopBulletText.text = bulletCount.ToString();
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            attemptShoot();
        }
        else if(Input.GetMouseButton(0))
        {
            attemptShoot();
        }
        Vector2 shootDirection = (GetMousePos() - new Vector2(transform.position.x,transform.position.y)).normalized;
        pointer.transform.eulerAngles = new Vector3(0,0,-90+ Mathf.Atan2(shootDirection.y,shootDirection.x)*180/Mathf.PI);
    }

    Vector2 GetMousePos()
    {
        Vector3 mousePos = Input.mousePosition;   
        mousePos.z=Camera.main.nearClipPlane;
        Vector3 Worldpos=Camera.main.ScreenToWorldPoint(mousePos);  
        Vector2 Worldpos2D=new Vector2(Worldpos.x,Worldpos.y);
        return Worldpos2D;
    }

    void attemptShoot()
    {
        if(canShoot && bulletCount>0 && SceneManage.Instance.currentState == SceneManage.STATE.PLAY)
        {
            Vector2 shootDirection = (GetMousePos() - new Vector2(transform.position.x,transform.position.y)).normalized;
            Shoot(shootDirection);
            rigidBody.AddForce(-shootDirection*shootPower*recoilFactor,ForceMode2D.Impulse);
            bulletCount-=1;
            foreach(Upgrade j in UpgradeManage.Instance.ballShootUpgrades){
                ballShootUpgrade bUpgrade = j as ballShootUpgrade;
                bUpgrade.doUpgrade();
            }
            colorBarBullets();
        }
    }

    public void Shoot(Vector2 shootDirection){
        Vector2 shootPoint = new Vector2(transform.position.x+shootDirection.x*transform.localScale.x*.95f,
        transform.position.y+shootDirection.y*transform.localScale.y*.95f);

        Rigidbody2D bullet = Instantiate(projectile,shootPoint,Quaternion.identity);
        float angle = Mathf.Atan2(shootDirection.y,shootDirection.x)*180/Mathf.PI;
        bullet.transform.localEulerAngles = new Vector3(0,0,angle);


        ballShootingAudioSource.PlayOneShot(ballShot);
        
        
        bullet.velocity = new Vector2(shootDirection.x * shootPower*1.5f, shootDirection.y * shootPower*1.5f);
        canShoot = false;
        bullet.GetComponent<BallBullet>().endVelocity = new Vector2(shootDirection.x*shootPower,shootDirection.y*shootPower);
        bullet.GetComponent<BallBullet>().bulletDamage = bulletDamage;

        iterateBulletCreation(bullet.GetComponent<BallBullet>(),true);

        StartCoroutine(FireCooldown());
        globalVariables.Instance.ballMovement.Flash(transform.position,0.075f,new Vector3(1.15f,1.15f,1.15f));
    }

    public int getBulletDamage(){
        BallBullet bullet = Instantiate(projectile).GetComponent<BallBullet>();
        bullet.bulletDamage = bulletDamage;
        iterateBulletCreation(bullet,false);
        int damage = bullet.bulletDamage;
        Destroy(bullet.gameObject);
        return damage;
    }

    private void iterateBulletCreation(BallBullet bullet,bool act){
        PriorityQueue<Upgrade,int> BCU = new PriorityQueue<Upgrade, int>();
        while(UpgradeManage.Instance.bulletCreationUpgrades.Count>0){
            bulletCreateUpgradeType upgrade = UpgradeManage.Instance.bulletCreationUpgrades.Dequeue() as bulletCreateUpgradeType;
            upgrade.doUpgrade(bullet,act);
            BCU.Enqueue(upgrade,upgrade.priority);
        }
        UpgradeManage.Instance.bulletCreationUpgrades = BCU;
    }

    public IEnumerator FireCooldown()
    {
        canShoot= false;
        yield return new WaitForSeconds(fireRate);
        canShoot= true;
    }

    public IEnumerator Reload()
    {
        canShoot= false;
        if(!isReloading && bulletCount < bulletCapacity){
            ballShootingAudioSource.PlayOneShot(ballReload);
            isReloading = true;
        

            for(int i = 0;i<bulletCapacity;i++)
            {
                if(bulletCount<bulletCapacity){
                    bulletCount+=1;
                    bulletCount=Mathf.Min(bulletCount,bulletCapacity);
                    colorBarBullets();
                    yield return new WaitForSeconds(reloadRate/bulletCapacity);
                }
            }
            if(bulletCount==bulletCapacity){
                
                canShoot= true;
            }
            isReloading = false;
        }else{
            canShoot= true;
            yield return new WaitForSeconds(0);
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.transform.tag == "Paddle" || col.transform.tag == "ExtraPaddle"){
            StartCoroutine(Reload());
        }
    }

    public void reset(){
        isReloading = false;
        canShoot = true;
        setCapacity(6);
        transform.position = globalVariables.Instance.paddleHealth.transform.position+ Vector3.up*.4f;
        rigidBody.velocity = Vector2.zero;

        shootPower = 15;
        fireRate = .15f;
        reloadRate = .3f;
        recoilFactor = .175f;
        bulletDamage = 15;
    }

    public void softReset(){
        isReloading = false;
        canShoot = true;
        transform.position = globalVariables.Instance.paddleHealth.transform.position+ Vector3.up*.4f;
        rigidBody.velocity = Vector2.zero;
    }
}
