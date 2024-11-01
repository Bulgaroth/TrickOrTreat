using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class CandyBullet : MonoBehaviour
{
	private const int BASE_DMG = 1;
	[SerializeField] private SC_PlayerData playerData;
	[SerializeField] private AudioClip[] missImpactSounds;
	[SerializeField] private AudioSource aS;

	[SerializeField] private GameObject visuals;
	[SerializeField] private Rigidbody rb;
	[SerializeField] private Collider collide;

	private void OnEnable()
	{
		rb.isKinematic = false;
		rb.useGravity = true;
		collide.enabled = true;
		visuals.SetActive(true);
	}

	private void OnTriggerEnter(Collider other)
	{
		//deleting the clone when it hits another collider

		if (other.transform.CompareTag("Player")) return;

		if (other.transform.CompareTag("Enemy"))
		{
			Debug.Log("Hit Enemy");
			var dmg = BASE_DMG + playerData.addedDamage;
			var enemy = other.transform.parent.GetComponentInParent<Enemy>();
			if (other.name == "Head") enemy.TakeDamage.Invoke(dmg, true);
			else enemy.TakeDamage.Invoke(dmg, false);
		}
		else aS.PlayOneShot(missImpactSounds[Random.Range(0, missImpactSounds.Length)]);

		StopProjectile();
		// Todo return to pool.
	}

	private void StopProjectile()
	{
		rb.isKinematic = true;
		rb.useGravity = false;
		rb.velocity = Vector3.zero;
		collide.enabled = false;
		visuals.SetActive(false);
	}
}