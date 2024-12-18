using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class Enemy : MonoBehaviour
{
	#region Attributes

	private float sound_cooldown;
	private bool monsterSoundReady = true;

	[SerializeField] private SC_EnemyData data;
	[SerializeField] private int _currentHP;
	[SerializeField] private AudioSource aS;
	[SerializeField] private AudioClip[] headshotSounds;
	[SerializeField] private AudioClip[] hitSounds;
	[SerializeField] private AudioClip[] monsterSounds;
	[SerializeField] private GameObject visuals;
	[SerializeField] private GameObject deathParticles;

	private bool dead;
	
	private NavMeshAgent agent;
	private PlayerController player;

	#endregion

	#region Events

	public UnityEvent<int,bool> TakeDamage;
	public UnityEvent Die;
	private bool paused;

	#endregion
	
	#region Unity API
	void Awake()
	{
		agent = GetComponent<NavMeshAgent>();
		_currentHP = data.health;
	}

	void Start()
	{
		player = FindObjectOfType<PlayerController>();
	}

	void Update()
	{
		if (dead) return;

		Vector3 targetPostition = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
		transform.LookAt(targetPostition);

		if (paused) return;

		if (monsterSoundReady) PlayMonsterSound();
		agent.SetDestination(player.transform.position);
	}

	#endregion

	#region Event Handlers

	private void OnEnable()
	{
		sound_cooldown = Random.Range(1.5f, 3f);
		TakeDamage.AddListener(OnTakeDamage);
		Die.AddListener(OnDie);
		visuals.SetActive(true);
		dead = false;
		agent.enabled = true;
	}


	private void OnDisable()
	{
		TakeDamage.RemoveListener(OnTakeDamage);
		Die.RemoveListener(OnDie);
	}
	
	private void OnTriggerEnter(Collider other)
	{
		if(dead) return;
		if (!other.transform.CompareTag("Player")) return;
	
		other.GetComponent<PlayerController>().TakeDamage.Invoke(data.damage);
	}
	
	void OnTakeDamage(int damage, bool headshot)
	{
		_currentHP -= damage;

		PlayHitSound(headshot);
		
		if (_currentHP <= 0 || headshot)
			Die.Invoke();
		
	}

	void OnDie()
	{
		Instantiate(deathParticles, transform.position + Vector3.up, Quaternion.identity);
		XPManager.instance.RegisterDeath(data.scoreGained);
		visuals.SetActive(false);
		dead = true;
		agent.enabled = false;
		
		SpawnManager.Instance.EnemyKilled.Invoke(this);
	}

	public void Pause()
	{
		paused = true;
		agent.enabled = false;
		aS.Pause();
	}

	public void Play()
	{
		paused = false;
		agent.enabled = true;
		aS.Play();
	}

	void PlayHitSound(bool headshot)
	{
		var sounds = headshot ? headshotSounds : hitSounds;
		aS.PlayOneShot(sounds[Random.Range(0, sounds.Length)]);
	}

	void PlayMonsterSound()
	{
		monsterSoundReady = false;
		aS.PlayOneShot(monsterSounds[Random.Range(0, monsterSounds.Length)]);
		sound_cooldown = Random.Range(1.5f, 3f);
		StartCoroutine(MonsterSoundCooldown());
	}

	IEnumerator MonsterSoundCooldown()
	{
		yield return new WaitForSeconds(sound_cooldown);
		monsterSoundReady = true;
	}

	#endregion
}
