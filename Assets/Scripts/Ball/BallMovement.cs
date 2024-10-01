using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMovement : MonoBehaviour
{
    private Rigidbody2D rigidBody;
    public float ballSpeed = 5;
    public float dampeningFactor;
    private Color originalColor;
    private SpriteRenderer spriteRenderer;
    public AudioClip ballPaddle;
    private AudioSource ballCollisionsAudioSource;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
        rigidBody = GetComponent<Rigidbody2D>();
        ballCollisionsAudioSource = gameObject.AddComponent<AudioSource>();
    }

    void FixedUpdate()
    {

    }
    void OnCollisionEnter2D(Collision2D col)
    {
        Flash(transform.position,0.075f,new Vector3(1.15f,1.15f,1.15f));
        if(col.transform.tag == "Enemy"){
            DamageText damage = Instantiate(globalVariables.Instance.damageText,col.GetContact(0).point,Quaternion.identity);
            int colDamage = (int)Mathf.Pow(col.relativeVelocity.magnitude,1.35f);
            damage.setText(colDamage);
            if(col.gameObject.GetComponent<Enemy>().health>0){
                col.transform.GetComponent<Enemy>().receiveDamage(colDamage);
            }
        }

        rigidBody.velocity = rigidBody.velocity * (1-dampeningFactor);
        if(col.transform.tag == "Paddle"){
            rigidBody.velocity = new Vector2(rigidBody.velocity.x,(rigidBody.velocity.y+30f)/4);
        }
    }
    public void Flash(Vector3 position, float timer,Vector3 scaler){
        Flasher flashed = Instantiate(globalVariables.Instance.flasher,position,Quaternion.identity,transform);
        flashed.sprite = spriteRenderer.sprite;
        flashed.scale = scaler;
        flashed.flashTimer = timer;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.name == "DeathZone"){
            globalVariables.Instance.paddleHealth.damage(1);
            transform.position = globalVariables.Instance.paddleHealth.transform.position+ Vector3.up*.4f;
            rigidBody.velocity = Vector2.zero;
            StartCoroutine(globalVariables.Instance.ballShoot.Reload());
            //StartCoroutine(respawn());
        }
    }

    IEnumerator respawn(){
        Time.timeScale=.25f;
        yield return new WaitForSeconds(.25f);
        Time.timeScale = 1;
    }
}
