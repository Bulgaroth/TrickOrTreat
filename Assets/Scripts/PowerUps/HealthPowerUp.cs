using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPowerUp : PowerUp
{
    [SerializeField] private int healAmount;

    protected override void ApplyEffects(PlayerController ctrl)
    {
        base.ApplyEffects(ctrl);
        //ctrl.AddLife
    }
}
