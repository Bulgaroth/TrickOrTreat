using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
	private void OnEnable()
	{
		LeanTween.moveY(gameObject, 0.75f, 1f).setLoopPingPong().setEaseInOutSine();
	}

	private void OnTriggerEnter(Collider other)
	{
		if (!other.CompareTag("Player")) return;
		
		ApplyEffects(other.GetComponent<PlayerController>());
		LeanTween.cancel(gameObject);
	}

	protected virtual void ApplyEffects(PlayerController ctrl)
	{
		print(name + " Ramassé");
		gameObject.SetActive(false);
	}
}
