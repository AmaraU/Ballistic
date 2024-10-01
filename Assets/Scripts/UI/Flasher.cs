using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flasher : MonoBehaviour
{
    public float flashTimer =5;
    private float flashCap;
    public Vector3 scale;
    public Sprite sprite;
    public SpriteRenderer spriteRenderer;
    void Start()
    {
        flashCap = flashTimer;

        transform.localScale = new Vector3(1,1,1);
        spriteRenderer.sprite=sprite;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.localScale = new Vector3(1,1,1) +(flashCap-flashTimer)/flashCap * (scale-new Vector3(1,1,1));
        spriteRenderer.sprite=sprite;
        flashTimer-=Time.deltaTime;
        if(flashTimer<=0){
            Destroy(gameObject);
        }
    }
}
