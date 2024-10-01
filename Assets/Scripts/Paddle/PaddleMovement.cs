using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PaddleMovement : MonoBehaviour
{
    public float moveSpeed = 3f;
    private float actMoveSpeed;
    public float slowPercentage;
    private Rigidbody2D rigidBody;
    private SpriteRenderer spriteRenderer;
    public Collider2D coinCollider;
    private bool isShrunk;
    public float desiredGoldAmount;
    [SerializeField]private float initialGoldAmount,currentGoldAmount = 0;
    public TMP_Text goldText,shopGoldText;
    void Start()
    {
        currentGoldAmount = 0;
        desiredGoldAmount = 0;
        initialGoldAmount = 0;
        isShrunk = false;
        actMoveSpeed = moveSpeed;
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidBody = GetComponent<Rigidbody2D>();
    }
    void FixedUpdate()
    {
        if(Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)){
            rigidBody.velocity = Vector2.right*actMoveSpeed;

        }
        else if(Input.GetKey(KeyCode.LeftArrow)|| Input.GetKey(KeyCode.A)){
            rigidBody.velocity = Vector2.left*actMoveSpeed;

        }
        else {
            rigidBody.velocity -=rigidBody.velocity*slowPercentage;
        }
        if(isShrunk){
            transform.localScale = new Vector3(2,.2f,1);
        }else{
            transform.localScale = new Vector3(4,.2f,1);
        }
    }

    public void addGold(float gold,Collider2D col){
        if(currentGoldAmount <= 999999 && col == coinCollider)
        {
            if (999999 - currentGoldAmount < gold)
            {
                gold = 999999 - currentGoldAmount;
            }
            initialGoldAmount = currentGoldAmount;
            desiredGoldAmount += gold;
        }
    }

    public void removeGold(float gold)
    {
        if(currentGoldAmount >= 0)
        {
            gold = Mathf.Min(gold,currentGoldAmount);
            initialGoldAmount = currentGoldAmount;
            desiredGoldAmount  -= gold;
        }
        
    }

    private void Update() {
        if(Input.GetMouseButton(1)){
            isShrunk = true;
        }else{
            isShrunk = false;
        }

        if(currentGoldAmount != desiredGoldAmount){
            if(initialGoldAmount < desiredGoldAmount){
                currentGoldAmount += (2f * Time.unscaledDeltaTime) * (desiredGoldAmount - initialGoldAmount);
                currentGoldAmount = Mathf.Min(currentGoldAmount,desiredGoldAmount);
            }else{
                currentGoldAmount -= (2f * Time.unscaledDeltaTime) * (initialGoldAmount - desiredGoldAmount);
                currentGoldAmount = Mathf.Max(currentGoldAmount,desiredGoldAmount);
            }
        }
        goldText.text = currentGoldAmount.ToString("0");
        shopGoldText.text = currentGoldAmount.ToString("0");
    }

    public void Flash(Vector3 position, float timer,Vector3 scaler){
        Flasher flashed = Instantiate(globalVariables.Instance.flasher,position,Quaternion.identity,transform);
        flashed.sprite = spriteRenderer.sprite;
        flashed.scale = scaler;
        flashed.flashTimer = timer;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.transform.tag != "BallBullet"){
            Flash(transform.position,0.075f,new Vector3(1.15f,1.15f,1.15f));
        }
    }
    public void reset(){
        currentGoldAmount = 0;
        desiredGoldAmount = 0;
        initialGoldAmount = 0;
        isShrunk = false;
        moveSpeed = 15;
        actMoveSpeed = moveSpeed;
        globalVariables.Instance.ballShoot.reset();
    }
}
