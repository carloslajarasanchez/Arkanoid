using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpAddLife : PowerUp
{
    public override void ApplyEffect()
    {
        GameManager.Instance.AddLifes(1);
    }
}
