using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : Bullet
{
    private bool isHit = false;
    public float homingSpeed = 0;
    new private void FixedUpdate() {
        if(rigidBody.velocity.magnitude > endVelocity.magnitude){
            rigidBody.velocity -= rigidBody.velocity*Time.deltaTime*0.01f;
        }else{
            rigidBody.velocity = endVelocity;
        }
        if(transform.position.y>globalVariables.Instance.paddleMovement.transform.position.y){
            Vector2 direction = ((Vector2)globalVariables.Instance.paddleMovement.transform.position - rigidBody.position).normalized;
            float rotateAmount = Vector3.Cross(direction, transform.right).z;

            rigidBody.angularVelocity = -rotateAmount * homingSpeed;
            rigidBody.velocity = transform.right * rigidBody.velocity.magnitude;
        }else{
            rigidBody.angularVelocity=0;
            rigidBody.velocity = transform.right * rigidBody.velocity.magnitude;
        }
    }
    override protected void OnTriggerEnter2D(Collider2D col)
    {
        dieTime =0.025f;
        if(col.gameObject.name == "Paddle" && !isHit){
            isHit = true;
            col.transform.GetComponent<PaddleHealth>().bulletHit(1,col);
            StartCoroutine(Die());
            Flash(transform.position,0.045f,new Vector3(3.5f,2f,2f));
        }
        if(col.tag == "Border" || col.tag == "Ball"){
            StartCoroutine(Die());
            Flash(transform.position,0.045f,new Vector3(3.5f,2f,2f));
            isHit = true;
        } 
    }
}
