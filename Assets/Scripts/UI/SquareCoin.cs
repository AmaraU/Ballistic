using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareCoin : MonoBehaviour
{
    public Rigidbody2D rigidBody;
    public int coinCost;
    public bool massAttract,tampered = false;
    public AudioClip coinCollect;
    private AudioSource coinCollectAudioSource;
    void Start()
    {
        Flash(transform.position,0.05f,new Vector3(2f,2f,2f));
        rigidBody = GetComponent<Rigidbody2D>();
        coinCollectAudioSource = transform.GetComponent<AudioSource>();
        tampered = false;
        massAttract = false;
    }
    void FixedUpdate()
    {
        if(massAttract){
            transform.Rotate(0,0,10,Space.Self);
            rigidBody.velocity = new Vector2(globalVariables.Instance.paddleMovement.transform.position.x-transform.position.x,globalVariables.Instance.paddleMovement.transform.position.y-transform.position.y).normalized * 12.5f;
        }else{
            transform.Rotate(0,0,2,Space.Self);
            rigidBody.velocity = new Vector2(rigidBody.velocity.x*.95f,rigidBody.velocity.y);
        }
        foreach(Upgrade j in UpgradeManage.Instance.coinUpgrades){
            coinUpgradeType cUpgrade  = j as coinUpgradeType;
            cUpgrade.doUpgrade(this);
        }
    }
    public void setCost(int num){
        coinCost = num;
        transform.localScale = new Vector3(0.3f,.3f,.3f) + new Vector3(.025f,.025f,.025f)*(coinCost-1);
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.name == "Paddle"){
            col.transform.GetComponent<PaddleMovement>().addGold(coinCost,col);
            StartCoroutine(Die(.1f));
            Flash(transform.position,0.1f,new Vector3(2f,2f,2f));
        }else if(col.gameObject.tag == "ExtraPaddle"){
            FindObjectOfType<PaddleMovement>().removeGold(-coinCost);
            StartCoroutine(Die(.1f));
            Flash(transform.position,0.1f,new Vector3(2f,2f,2f));
        }
        if(col.gameObject.name == "Floor"){
            Destroy(gameObject);
        } 
    }
    void OnTriggerStay2D(Collider2D col)
    {
        if(col.gameObject.tag == "Border" && col.gameObject.name != "Roof"){
            rigidBody.AddForce((Vector2.zero -(Vector2)transform.position).normalized*.05f,ForceMode2D.Impulse);
        }
    }

    protected IEnumerator Die(float secs)
    {
        coinCollectAudioSource.PlayOneShot(coinCollect);
        massAttract = true;
        yield return new WaitForSeconds(secs);
        Destroy(gameObject);
    }

    public void Flash(Vector3 position, float timer,Vector3 scaler){
        Flasher flashed = Instantiate(globalVariables.Instance.flasher,position,transform.rotation,transform);
        flashed.sprite = GetComponent<SpriteRenderer>().sprite;
        flashed.scale = scaler;
        flashed.flashTimer = timer;
    }
}
