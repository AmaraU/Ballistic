using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingHealthBar : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    public Slider slider;
    
    void Start()
    {
        canvasGroup.alpha = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        canvasGroup.alpha -= Time.deltaTime/1;
        canvasGroup.alpha = Mathf.Max(0,canvasGroup.alpha);
    }

    public void showHealth(){
        canvasGroup.alpha =1;
    }

    public void setSlider(float num){
        slider.value = num;
    }
}
