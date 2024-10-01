using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

public class UpgradeManage : MonoBehaviour
{
    public static UpgradeManage Instance;
    public List<Upgrade> masterUpgrades, remainingUpgrades;
    public Upgrade healthUpgrade;

    public List<Upgrade> coinUpgrades,levelUpgrades,enemyDeathUpgrades,enemyCollisionUpgrades,ballShootUpgrades;

    public PriorityQueue<Upgrade,int> bulletCreationUpgrades;

    void Awake()
    {
        UpgradeManage.Instance = this;
        bulletCreationUpgrades = new PriorityQueue<Upgrade, int>();
    }

    void Start()
    {
        foreach(Upgrade j in masterUpgrades){
            j.reset();
        }
        UpgradeManage.Instance.remainingUpgrades = new List<Upgrade>(UpgradeManage.Instance.masterUpgrades);
    }

    public void reset(){
        coinUpgrades.Clear();
        levelUpgrades.Clear();
        bulletCreationUpgrades.Clear();
        enemyCollisionUpgrades.Clear();
        enemyDeathUpgrades.Clear();
        ballShootUpgrades.Clear();
        healthUpgrade.reset();
        foreach(Upgrade j in masterUpgrades){
            j.reset();
        }
        UpgradeManage.Instance.remainingUpgrades = new List<Upgrade>(UpgradeManage.Instance.masterUpgrades);
    }
}
