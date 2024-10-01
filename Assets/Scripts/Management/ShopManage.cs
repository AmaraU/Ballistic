using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopManage : MonoBehaviour
{
    public static ShopManage Instance;
    public AudioClip itemBuy;
    public List<Button> items;
    public TMP_Text itemPriceText,itemNameText,itemDescText;

    public List<ShopItem> shopItems;
    void Awake()
    {
        ShopManage.Instance = this;
    }

    void Start()
    {
        ShopManage.Instance.shopItems = new List<ShopItem>(4);
        ShopItem heals = new GameObject("heals").AddComponent<ShopItem>();
        ShopItem item2 = new GameObject("item2").AddComponent<ShopItem>();
        ShopItem item3 = new GameObject("item3").AddComponent<ShopItem>();
        ShopItem item4 = new GameObject("item4").AddComponent<ShopItem>();
        heals.upgrade = UpgradeManage.Instance.healthUpgrade;
        heals.upgrade.reset();
        items[0].GetComponent<Image>().sprite = heals.upgrade.sprite;
        ShopManage.Instance.shopItems.Add(heals);
        ShopManage.Instance.shopItems.Add(item2);
        ShopManage.Instance.shopItems.Add(item3);
        ShopManage.Instance.shopItems.Add(item4);  
        foreach(ShopItem j in ShopManage.Instance.shopItems){
            j.audioSource = gameObject.AddComponent<AudioSource>();
            j.itemBuy = itemBuy;
        }      
    }

    public void reroll(){
        if(globalVariables.Instance.paddleMovement.desiredGoldAmount>=20){
            globalVariables.Instance.paddleMovement.removeGold(20);
            deGenerate();
            generateShop();
        }
    }

    public void generateShop(){
        for(int i = 1;i<4;i++){
            if(UpgradeManage.Instance.remainingUpgrades.Count != 0){
                shopItems[i].upgrade = UpgradeManage.Instance.remainingUpgrades[(int)Random.Range(0,UpgradeManage.Instance.remainingUpgrades.Count)];
                UpgradeManage.Instance.remainingUpgrades.Remove(shopItems[i].upgrade);
            }else{
                shopItems[i].upgrade = UpgradeManage.Instance.healthUpgrade;
            }
            shopItems[i].isBought = false;
            items[i].GetComponent<Image>().sprite = shopItems[i].upgrade.sprite;  
        }
    }

    public void deGenerate(){
        for(int i = 1;i<4;i++){
            if(!shopItems[i].isBought){
                UpgradeManage.Instance.remainingUpgrades.Add(shopItems[i].upgrade);
            }
        }
    }

    public void hoverEnter(int num){
        Button item = ShopManage.Instance.items[num];
        item.GetComponent<RectTransform>().sizeDelta = new Vector2(120f,120f);
        foreach (Button j in ShopManage.Instance.items){
            if(j!= item){
                j.GetComponent<RectTransform>().sizeDelta = new Vector2(80f,80f);
                j.GetComponent<Image>().color = Color.black;
            }
        }
        ShopItem shopItem = ShopManage.Instance.shopItems[num];
        Upgrade upgrade = shopItem.upgrade;

        if(shopItem.isBought){
            itemPriceText.text= "bought";
            itemPriceText.color = new Color(.31f,.69f,.26f);
        }else{
            itemPriceText.text= upgrade.cost.ToString()+"G";
            if(FindObjectOfType<PaddleMovement>().desiredGoldAmount < upgrade.cost){
                itemPriceText.color = Color.red;
            }else{
                itemPriceText.color = new Color(.97f,.77f,.29f);
            }
        }
        
        itemNameText.text=upgrade.upgradeName;
        itemDescText.text=upgrade.desc;
    }

    public void hoverExit(){
        foreach (Button j in ShopManage.Instance.items){
            j.GetComponent<RectTransform>().sizeDelta = new Vector2(100f,100f);
            j.GetComponent<Image>().color = Color.white;
        }
        itemPriceText.text="";
        itemNameText.text="";
        itemDescText.text="";
    }

    public void hoverEnter1(){
        hoverEnter(0);
    }
    public void hoverEnter2(){
        hoverEnter(1);
    }
    public void hoverEnter3(){
        hoverEnter(2);
    }
    public void hoverEnter4(){
        hoverEnter(3);
    }

    public void buy(int num){
        ShopItem shopItem = ShopManage.Instance.shopItems[num];
        shopItem.buy();
        Upgrade upgrade = shopItem.upgrade;

        if(shopItem.isBought){
            itemPriceText.text= "bought";
            itemPriceText.color = new Color(.31f,.69f,.26f);
        }else{
            itemPriceText.text= upgrade.cost.ToString()+"G";
            if(FindObjectOfType<PaddleMovement>().desiredGoldAmount < upgrade.cost){
                itemPriceText.color = Color.red;
            }else{
                itemPriceText.color = new Color(.97f,.77f,.29f);
            }
        }
        
        itemNameText.text=upgrade.upgradeName;
        itemDescText.text=upgrade.desc;
    }

    public void buy1(){
        buy(0);
    }
    public void buy2(){
        buy(1);
    }

    public void buy3(){
        buy(2);
    }
    public void buy4(){
        buy(3);
    }
}
