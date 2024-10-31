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
}
