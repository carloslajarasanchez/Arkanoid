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
        GameManager.Instance.AddLifes(1);
    }

    public override void Remove()
    {
        
    }
}
