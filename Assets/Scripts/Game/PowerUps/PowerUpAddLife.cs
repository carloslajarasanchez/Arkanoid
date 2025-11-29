using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpAddLife : PowerUp
{
    protected new void Start()
    {
        base.Start();
        stackeable = true;
    }
    
    public override void ApplyEffect()
    {
        GameManager.Instance.AddLifes(1);// Aumentamos la vida del GameManager
    }

    public override void Remove()
    {
        
    }
}
