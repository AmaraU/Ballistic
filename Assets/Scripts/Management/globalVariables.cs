using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class globalVariables : MonoBehaviour
{
    static public globalVariables Instance;
    public PaddleHealth paddleHealth;
    public PaddleMovement paddleMovement;
    public BallShoot ballShoot;
    public BallMovement ballMovement;
    public DamageText damageText;
    public Flasher flasher;
    public Blinker blinker;
    public Canvas spawnAnim;
    
    public SquareCoin coin;
    void Awake()
    {
        globalVariables.Instance = this;
    }
}
