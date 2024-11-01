using UnityEngine;

[CreateAssetMenu(menuName = "TrickOrTreats/Player")]
public class SC_PlayerData : ScriptableObject
{
	public int playerBaseHP;
	
	public float playerSpeed;

	public int addedDamage;

	// PowerUps

	public int healAmount;

	public float slowTime;

	public float fireRate;

	public void Clear()
	{
		playerBaseHP = 10;
		playerSpeed = 10;
		addedDamage = 0;
		healAmount = 5;
		slowTime = 3;
		fireRate = 0.5f;
	}
}
