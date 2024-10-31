using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class CandyBullet : MonoBehaviour
{
	[SerializeField] private int damage;
	[SerializeField] private SC_PlayerData playerData;
	[SerializeField] private AudioClip[] missImpactSounds;
	[SerializeField] private AudioSource aS;

	private void OnTriggerEnter(Collider other)
	{
		//deleting the clone when it hits another collider

		if (other.transform.CompareTag("Player")) return;

		Debug.Log("Hit");

		if (other.transform.CompareTag("Enemy"))
			other.gameObject.GetComponent<Enemy>().TakeDamage.Invoke(damage);
		else aS.PlayOneShot(missImpactSounds[Random.Range(0, missImpactSounds.Length)]);

		Destroy(gameObject);
	}
}