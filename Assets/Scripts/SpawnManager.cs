using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using static UnityEngine.EventSystems.EventTrigger;
using Random = UnityEngine.Random;

public class SpawnManager : MonoBehaviour
{
	public static SpawnManager Instance;
	
	#region Attributes
	public float radius = 1;
	public Vector3 regionSize = Vector3.one;
	public int rejectionSamples = 15;
	public float displayRadius =1;

	List<Vector3> points;

	public bool CanContinueSpawn;
	private List<Enemy> spawnedEnemies = new();

	[SerializeField] private const float MAX_ENEMY = 50;
	[SerializeField] private const float SPAWN_COOLDOW = 0.5f;
	[SerializeField] private const float DISTANCE_PLAYER = 10.0f;
	[SerializeField] private const String POOL_TAG = "Enemy_";

	private IEnumerator spawnCoroutine;
	
	public enum EnemyType
	{
		Ghost,
		Clown,
		Pumpkin,
		Skeleton,
		Vampire
	}

	[SerializeField] private List<EnemyType> enemyTypes;
	
	#endregion

	#region Events

	[HideInInspector] public UnityEvent StartSpawning;
	[HideInInspector] public UnityEvent StopSpawning;
	[HideInInspector] public UnityEvent SpawnEnemy;
	[HideInInspector] public UnityEvent<Enemy> EnemyKilled;

	#endregion
	
	#region Unity API

	private void Awake()
	{
		Instance = this;
		
		points = PoissonDiscSampling.Get3DPointsFrom2DPointList(radius, new Vector2(regionSize.x, regionSize.z), rejectionSamples);

		spawnCoroutine = SpawnCooldownCoroutine();
	}
	
	#endregion

	#region Methods

	private bool RandomPosition(out Vector3 result)
	{
		for (int i = 0; i < 20; i++)
		{
			Vector3 randomPoint =  points[Random.Range(0, points.Count)];
			if (NavMesh.SamplePosition(randomPoint, out var hit, 1.0f, NavMesh.AllAreas))
			{
				Debug.Log($"Hit.position:{hit.position}");
				if (Vector3.Distance(hit.position, GameManager.Instance.GetPlayer().transform.position) >
					DISTANCE_PLAYER)
				{
					result = hit.position;
					return true;
				}
			}
		}
		
		result = Vector3.zero;
		return false;
	}
	
	#endregion

	#region Event Handlers

	private void OnEnable()
	{
		StartSpawning.AddListener(OnStartSpawning);
		StopSpawning.AddListener(OnStopSpawning);
		SpawnEnemy.AddListener(OnSpawnEnemy);
		EnemyKilled.AddListener(OnEnemyKilled);
	}

	private void OnDisable()
	{
		StartSpawning.RemoveListener(OnStartSpawning);
		StopSpawning.RemoveListener(OnStopSpawning);
		SpawnEnemy.RemoveListener(OnSpawnEnemy);
		EnemyKilled.RemoveListener(OnEnemyKilled);
	}

	void OnStartSpawning()
	{
		StartCoroutine(spawnCoroutine);
	}

	void OnStopSpawning()
	{
		StopCoroutine(spawnCoroutine);
	}
	
	void OnSpawnEnemy()
	{
		Debug.Log($"OnSpawnEnemy > Start Spawn enemy");

		var enemy = PoolManager.instance.GetElement(POOL_TAG + enemyTypes[Random.Range(0, enemyTypes.Count)]).GetComponent<Enemy>();
		
		if (RandomPosition(out var position))
		{
			Debug.Log("Spawn enemy");
			enemy.transform.position = position;
			enemy.gameObject.SetActive(true);
		}

		spawnedEnemies.Add(enemy);
	}

	void OnEnemyKilled(Enemy enemy)
	{
		spawnedEnemies.Remove(enemy);
	}

	public void TogglePause(bool pause)
	{
		foreach (Enemy enemy in spawnedEnemies)
		{
			if (pause) enemy.Pause();
			else enemy.Play();
		}
	}
	
	#endregion

	#region Editor

	void OnValidate() {
		points = PoissonDiscSampling.Get3DPointsFrom2DPointList(radius, new Vector2(regionSize.x, regionSize.z), rejectionSamples);
	}

	void OnDrawGizmosSelected() {
		Gizmos.DrawWireCube(transform.position + regionSize/2, regionSize);
		if (points != null) {
			foreach (Vector3 point in points) {
				Gizmos.DrawSphere(transform.position + point, displayRadius);
			}
		}
	}

	#endregion

	#region Coroutines

	IEnumerator SpawnCooldownCoroutine()
	{
		if (spawnedEnemies.Count < MAX_ENEMY)
			SpawnEnemy.Invoke();
		
		yield return new WaitForSeconds(SPAWN_COOLDOW);
		
		spawnCoroutine = SpawnCooldownCoroutine();
		StartCoroutine(spawnCoroutine);
	}

	#endregion
}
