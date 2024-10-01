using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

public class ShopItem : MonoBehaviour
{
    public Upgrade upgrade;
    public bool isBought;
    public AudioSource audioSource;
    public AudioClip itemBuy;
    public void buy(){
        if(globalVariables.Instance.paddleMovement.desiredGoldAmount>=upgrade.cost && !isBought){
            globalVariables.Instance.paddleMovement.removeGold(upgrade.cost);
            audioSource.PlayOneShot(itemBuy);
            if(upgrade is healUpgrade){
                healUpgrade hUpgrade = upgrade as healUpgrade;
                hUpgrade.doUpgrade();
            }
            else{
                isBought = true;
                if(upgrade is instantUpgradeType){
                    instantUpgradeType iUpgrade = upgrade as instantUpgradeType;
                    iUpgrade.doUpgrade();
                }
                else if(upgrade is coinUpgradeType){
                    UpgradeManage.Instance.coinUpgrades.Add(upgrade);
                }else if(upgrade is bulletCreateUpgradeType){
                    (upgrade as bulletCreateUpgradeType).doUpgrade(null,false);
                    bulletCreateUpgradeType bul = upgrade as bulletCreateUpgradeType;
                    UpgradeManage.Instance.bulletCreationUpgrades.Enqueue(upgrade,bul.priority);
                }else if(upgrade is levelUpgradeType){
                    UpgradeManage.Instance.levelUpgrades.Add(upgrade);
                }else if(upgrade is enemyCollisionUpgrade){
                    UpgradeManage.Instance.enemyCollisionUpgrades.Add(upgrade);
                }else if(upgrade is enemyDeathUpgrade){
                    UpgradeManage.Instance.enemyDeathUpgrades.Add(upgrade);
                }
                else if(upgrade is ballShootUpgrade){
                    UpgradeManage.Instance.ballShootUpgrades.Add(upgrade);
                }
            }
        }
    }
}
