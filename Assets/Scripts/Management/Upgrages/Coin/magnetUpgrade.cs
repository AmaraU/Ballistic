using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class magnetUpgrade : coinUpgradeType
{
    private void Awake() {
        reset();
    }
    public override void doUpgrade(SquareCoin coin)
    {
        coin.transform.Rotate(0,0,5);
        if(coin.transform.position.y<=-1 && Mathf.Abs(globalVariables.Instance.paddleHealth.transform.position.x-coin.transform.position.x)>globalVariables.Instance.paddleHealth.transform.localScale.x/4){
            coin.rigidBody.velocity = new Vector2(globalVariables.Instance.paddleHealth.transform.position.x-coin.transform.position.x,0).normalized * 10f/Mathf.Max(Mathf.Abs(globalVariables.Instance.paddleHealth.transform.position.y-coin.transform.position.y),.75f) + new Vector2(0,coin.rigidBody.velocity.y);
        }
        else{
            coin.rigidBody.velocity = new Vector2(coin.rigidBody.velocity.x*.9f,coin.rigidBody.velocity.y);
        }
    }
    public override void reset()
    {
    }
}
