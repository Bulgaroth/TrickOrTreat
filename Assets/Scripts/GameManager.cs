using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public class GameManager : MonoBehaviour
{
	public static GameManager Instance;

	#region Attributes

	[SerializeField] private GameObject deathMenu;
	[SerializeField] private GameObject winMenu;
	[SerializeField] private GameObject escapeMenu;
	[SerializeField] private Volume grayScale;

	private PoolManager PoolManager;
	[SerializeField] private SpawnManager SpawnManager;

	[SerializeField] private PlayerController player;
	[SerializeField] private SC_PlayerData playerData;
	[SerializeField] private Timer timer;

	private float bulletTimeEffectTimer;
	private bool inBulletTime;

	InputSystem_Actions input;

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
		player.BulletTime.AddListener(StartBulletTime);

		input = new();
		input.UI.Escape.Enable();
		input.UI.Escape.performed += ToggleEscape;
	}

	private void Start()
	{
		SpawnManager.StartSpawning.Invoke();
		deathMenu.SetActive(false);
	}

	private void Update()
	{
		if(inBulletTime)
		{
			bulletTimeEffectTimer -= Time.deltaTime;
			if (bulletTimeEffectTimer <= 0) StopBulletTime();
		}
	}

	#endregion

	#region Methods

	private void ToggleEscape(UnityEngine.InputSystem.InputAction.CallbackContext _)
	{
		if (escapeMenu.activeInHierarchy)
		{
			Play();
			escapeMenu.SetActive(false);
			player.ToggleCamera(true);
		}
		else
		{
			Pause();
			escapeMenu.SetActive(true);
			player.ToggleCamera(false);
		}
	}

	public PlayerController GetPlayer()
	{
		return player;
	}

	public void StartBulletTime()
	{
		print("BULLET TIME !");
		Pause();
		bulletTimeEffectTimer = playerData.slowTime;
		inBulletTime = true;
		grayScale.weight = 1f;
	}

	public void StopBulletTime()
	{
		Play();
		inBulletTime = false;
		grayScale.weight = 0f;
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
		player.ToggleCamera(false);
	}

	public void OnWin()
	{
		winMenu.SetActive(true);
		Pause();
		player.ToggleCamera(false);
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
