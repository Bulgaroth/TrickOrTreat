using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoffeePowerUp : PowerUp
{
	protected override void ApplyEffects(PlayerController ctrl)
	{
		ctrl.OnBulletTime();
		base.ApplyEffects(ctrl);
	}
}
