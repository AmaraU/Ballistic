using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtraPaddle : MonoBehaviour
{
    Rigidbody2D rigidBody;
    private SpriteRenderer spriteRenderer;
    Vector2 lastDirection = Vector2.right;
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidBody = GetComponent<Rigidbody2D>();
    }
    void FixedUpdate()
    {
        rigidBody.velocity = lastDirection*7.5f;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.tag == "Border"){
            lastDirection = -lastDirection;
        }
        Flash(transform.position,0.075f,new Vector3(1.15f,1.15f,1.15f));
    }

    public void Flash(Vector3 position, float timer,Vector3 scaler){
        Flasher flashed = Instantiate(globalVariables.Instance.flasher,position,Quaternion.identity,transform);
        flashed.sprite = spriteRenderer.sprite;
        flashed.scale = scaler;
        flashed.flashTimer = timer;
    }
}
