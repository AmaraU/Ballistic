using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class Bullet : MonoBehaviour
{
    public Rigidbody2D rigidBody;
    public Vector2 endVelocity;
    public float dieTime;
    public int bounce = 0;
    protected void Start()
    {
        Flash(transform.position,0.05f,new Vector3(3f,2f,2f));
        rigidBody = GetComponent<Rigidbody2D>();
    }

    protected void FixedUpdate()
    {
        if(rigidBody.velocity.magnitude > endVelocity.magnitude){
            rigidBody.velocity -= rigidBody.velocity*Time.deltaTime*0.01f;
        }else{
            rigidBody.velocity = endVelocity;
        }
    }

    abstract protected void OnTriggerEnter2D(Collider2D col);

    public void Flash(Vector3 position, float timer,Vector3 scaler){
        Flasher flashed = Instantiate(globalVariables.Instance.flasher,position,transform.rotation,transform);
        flashed.sprite = GetComponent<SpriteRenderer>().sprite;
        flashed.scale = scaler;
        flashed.flashTimer = timer;
    }

    protected IEnumerator Die()
    {
        yield return new WaitForSeconds(dieTime);
        Destroy(gameObject);
    }
}
