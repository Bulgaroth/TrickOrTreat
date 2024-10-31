using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
	public PowerUpSpawner spawner;

	private void OnEnable()
	{
		LeanTween.moveLocalY(gameObject, 0.5f, 1f).setLoopPingPong().setEaseInOutSine();
	}

    private void OnTriggerEnter(Collider other)
	{
		if (!other.CompareTag("Player")) return;
		
		ApplyEffects(other.GetComponent<PlayerController>());
		LeanTween.cancel(gameObject);
		spawner.PowerUpUsed();
	}

	protected virtual void ApplyEffects(PlayerController ctrl)
	{
		print(name + " Ramassé");
		gameObject.SetActive(false);
	}
}
