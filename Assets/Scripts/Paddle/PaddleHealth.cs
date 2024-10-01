using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PaddleHealth : MonoBehaviour
{
    public int healthCapacity,healthCount;
    public TMP_Text healthText,shopHealthText;
    public Slider slider,shopSlider;
    public Collider2D hitRegion;
    private float iFrames;
    private Rigidbody2D rigidBody;
    private SpriteRenderer spriteRenderer;
    void Start()
    {
        iFrames = 0.1f;
        healthCount = healthCapacity;
        showHealth();
        rigidBody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void setCapacity(int cap, int hnum){
        healthCapacity = cap;
        healthCount = hnum;
        heal(0);
    }

    void FixedUpdate()
    {
        iFrames-=Time.deltaTime;
        iFrames = Mathf.Max(iFrames,0);
    }

    public void heal(int amount){
        if(healthCount<healthCapacity){
            flashHeal(new Color(.2f,1,0));
        }
        healthCount+=amount;
        healthCount = Mathf.Min(healthCapacity,healthCount);
        showHealth();
    }

    public void damage(int amount){
        if(iFrames == 0){
            flashHeal(new Color(1,0,0));
            iFrames = 0.075f;
            healthCount-=amount;
            healthCount = Mathf.Max(0,healthCount);
            showHealth();
            if(healthCount<=0){
                SceneManage.Instance.overIn();
            }
            globalVariables.Instance.paddleMovement.Flash(transform.position,0.075f,new Vector3(1.15f,1.15f,1.15f));
        }
    }

    public void bulletHit(int amount,Collider2D col){
        if(col == hitRegion){
            damage(amount);
        }
    }

    private void showHealth(){
        healthText.text = healthCount.ToString()+"/"+healthCapacity.ToString();
        slider.value = (float)healthCount/healthCapacity;
        shopHealthText.text = healthCount.ToString()+"/"+healthCapacity.ToString();
        shopSlider.value = (float)healthCount/healthCapacity;
    }

    public void reset(){
        rigidBody.velocity =Vector2.zero;
        transform.position = new Vector3(0,-4,0);
        healthCapacity = 12;
        healthCount = healthCapacity;
        showHealth();
        globalVariables.Instance.paddleMovement.reset();
    }

    public void softReset(){
        rigidBody.velocity =Vector2.zero;
        transform.position = new Vector3(0,-4,0);
        globalVariables.Instance.ballShoot.softReset();
    }

    public void flashHeal(Color color){
        Blinker blinker = Instantiate(globalVariables.Instance.blinker,new Vector3(0,-.5f,0),Quaternion.identity);
        blinker.sprite = spriteRenderer.sprite;
        blinker.scale = new Vector3(16,9,1);
        blinker.blinkTimer = .5f;
        blinker.spriteRenderer.color = color;
        blinker.alpha=.25f;
    }
}
