using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DamageText : MonoBehaviour
{
    public TMP_Text damageText;
    private CanvasGroup canvasGroup;
    
    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        canvasGroup.alpha -= Time.deltaTime/2;
        transform.Translate(Vector2.up*canvasGroup.alpha/20);
        if(canvasGroup.alpha <=0){
            Destroy(gameObject);
        }
    }

    public void setText(int num){
        damageText.text = num.ToString();
    }
}
