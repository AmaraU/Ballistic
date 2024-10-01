using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class Upgrade : MonoBehaviour
{
    public int cost;
    public string upgradeName;
    public string desc;
    public Sprite sprite;
    abstract public void reset();
}
