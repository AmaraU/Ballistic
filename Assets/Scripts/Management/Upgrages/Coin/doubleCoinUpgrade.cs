using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doubleCoinUpgrade : coinUpgradeType
{
    private void Awake() {
        reset();
    }

    private void Start() {
    }
    public override void doUpgrade(SquareCoin coin)
    {
        if(!coin.tampered){
            coin.setCost(coin.coinCost*2);
            coin.tampered = true;
        }
    }
    public override void reset()
    {
    }
}
