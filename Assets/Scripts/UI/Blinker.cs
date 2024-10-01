using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blinker : MonoBehaviour
{
    public float blinkTimer =5;
    private float blinkCap;
    public Vector3 scale;
    public Vector3 endScale;
    public Sprite sprite;
    public SpriteRenderer spriteRenderer;
    public float alpha=.25f;
    void Start()
    {
        blinkCap = blinkTimer;
        spriteRenderer.sprite=sprite;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(endScale== -Vector3.one){
            transform.localScale = scale;
        }else{
            transform.localScale = scale - (blinkCap-blinkTimer)/blinkCap * (scale-endScale);
        }
        spriteRenderer.sprite=sprite;
        spriteRenderer.color = new Color(spriteRenderer.color.r,spriteRenderer.color.g,spriteRenderer.color.b,0) + new Color(0,0,0,alpha*blinkTimer/blinkCap);
        blinkTimer-=Time.deltaTime;
        if(blinkTimer<=0){
            Destroy(gameObject);
        }
    }
}
