using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public static GameManager Instance;

	#region Attributes

	[SerializeField] private GameObject deathMenu;

	private PoolManager PoolManager;
	[SerializeField] private SpawnManager SpawnManager;

	[SerializeField] private PlayerController player;
	[SerializeField] private SC_PlayerData playerData;
	[SerializeField] private Timer timer;
	
	#endregion
	
	#region Unity API

	private void Awake()
	{
		Instance = this;
		Cursor.visible = false;
		PoolManager = PoolManager.instance;
		//SpawnManager = SpawnManager.Instance;
		playerData.Clear();
		player = FindObjectOfType<PlayerController>();
	}

	private void Start()
	{
		SpawnManager.StartSpawning.Invoke();
		deathMenu.SetActive(false);
	}

	#endregion

	#region Methods

	public PlayerController GetPlayer()
	{
		return player;
	}

	public void Pause()
	{
		SpawnManager.StopSpawning.Invoke();
		SpawnManager.TogglePause(true);
		timer.paused = true;
	}

	public void Play()
	{
		SpawnManager.StartSpawning.Invoke();
		SpawnManager.TogglePause(false);
		timer.paused = false;
	}


	void OnPlayerDie()
    {
		//TODO
		deathMenu.SetActive(true);
		Pause();
    }


    #endregion

    #region Event Handlers

    private void OnEnable()
    {
		player = FindObjectOfType<PlayerController>();

		player.Die.AddListener(OnPlayerDie);
    }

    private void OnDisable()
    {
		player.Die.RemoveListener(OnPlayerDie);
    }

    #endregion
}
