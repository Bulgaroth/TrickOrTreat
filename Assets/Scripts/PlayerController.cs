using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
	#region Attributes

	[Header("=== Player ===")]
	[SerializeField] private SC_PlayerData playerData;
	[SerializeField] private LayerMask groundLayers;
	[SerializeField] private HeadBobbing headBobbing;
	[SerializeField] private AudioSource characterAudio;
	[SerializeField] private AudioSource gunAudio;
	[SerializeField] private AudioClip[] powerUpSounds;
	[SerializeField] private AudioClip[] commentSounds;
	[SerializeField] private AudioClip[] gunSounds;
	[SerializeField] private Animator gunAnimator;
	[SerializeField] private Vector2 sensitivity;

	bool canShoot = true;
	[SerializeField] private int playerHP;
	
	[SerializeField] private Vector2 cameraRange;
	[SerializeField] private float gravity = 100.0f;
	
	private Camera mainCamera;
	private CharacterController _controller;
	
	private float verticaRotation;
	private Vector3 movements;
	private Vector2 mouseRotation;

	[SerializeField] private HealthBar healthBar;
	
	// Gun Related
	[Header("=== Gun ===")] 
	[SerializeField] private List<Rigidbody> bulletList;
	[SerializeField] private float bulletPower;
	[SerializeField] private Transform bulletSpawnPoint;
	#endregion

	#region Events

	[HideInInspector] public UnityEvent<int> Heal;
	[HideInInspector] public UnityEvent<int> TakeDamage;
	[HideInInspector] public UnityEvent Die;
	[HideInInspector] public UnityEvent BulletTime;

	[HideInInspector] public UnityEvent<int> AddMaxLife;
	[HideInInspector] public UnityEvent<float> AddSpeed;
	[HideInInspector] public UnityEvent<int> AddDmg;
	[HideInInspector] public UnityEvent<float> AddFireRate;
	[HideInInspector] public UnityEvent<float> AddPowerUpEfficiency;

	bool roaming = true;

	#endregion

	#region Unity API

	private void Awake()
	{
		mainCamera = Camera.main;
		_controller = GetComponent<CharacterController>();
		playerHP = playerData.playerBaseHP;
		healthBar.SetMaxHealth(playerData.playerBaseHP);
		healthBar.SetHealth(playerData.playerBaseHP);
	}

	void Update()
	{
		if (!roaming) return;

		MovePlayer();
		RotateCamera();
		CheckGroundType();
	}

	#endregion

	#region Methods

	void MovePlayer()
	{
		float curSpeedX = playerData.playerSpeed * movements.x;
		float curSpeedZ = playerData.playerSpeed * movements.z;
		Vector3 moveDirection = (transform.forward * curSpeedZ) + (transform.right * curSpeedX);
		
		ApplyGravity(ref moveDirection);
		
		_controller.Move( moveDirection * Time.deltaTime);
	}
	
	void ApplyGravity(ref Vector3 moveDirection)
	{
		if (_controller.isGrounded) return;

		moveDirection.y -= gravity * Time.deltaTime;
	}

	void RotateCamera()
	{
		transform.Rotate(0, mouseRotation.x * sensitivity.y,0);
		
		verticaRotation -= mouseRotation.y;
		verticaRotation = Mathf.Clamp(verticaRotation, cameraRange.x, cameraRange.y);
		mainCamera.transform.localRotation = Quaternion.Euler(verticaRotation * sensitivity.x, 0, 0);
	}

	void Shoot()
	{
		if (!canShoot) return;
		if (!roaming) return;
		canShoot = false;
		StartCoroutine(ShootCooldown());

		// Instantiate the projectile at the position and rotation of this transform
		Rigidbody clone;
		clone = Instantiate(bulletList[Random.Range(0, bulletList.Count)], bulletSpawnPoint.position, bulletSpawnPoint.rotation);

		// Give the cloned object an initial velocity along the current
		// object's Z axis
		clone.velocity = mainCamera.transform.forward * bulletPower;

		gunAudio.pitch = Random.Range(0.9f, 1.1f);
		gunAudio.PlayOneShot(gunSounds[0]);
		gunAnimator.SetTrigger("Shoot");
	}

	void CheckGroundType()
	{
		if(Physics.Raycast(transform.position, -transform.up, out RaycastHit hit, 1, groundLayers))
		{
			HeadBobbing.GroundType groundType = hit.collider.tag switch
			{
				"Ground_Grass" => HeadBobbing.GroundType.Grass,
				"Ground_Concrete" => HeadBobbing.GroundType.Concrete,
				"Ground_Tile" => HeadBobbing.GroundType.Tile,
				_ => HeadBobbing.GroundType.Car
			};

			headBobbing.groundType = groundType;
		}
	}

	#endregion

	#region Event Handlers

	private void OnEnable()
	{
		AddMaxLife.AddListener(OnAddMaxLife);
		AddDmg.AddListener(OnAddDmg);
		AddSpeed.AddListener(OnAddSpeed);
		AddFireRate.AddListener(OnAddFireRate);
		AddPowerUpEfficiency.AddListener(OnAddPowerUpEfficiency);
		
		Heal.AddListener(OnHeal);
		TakeDamage.AddListener(OnTakeDamage);
		Die.AddListener(OnDie);
	}


	private void OnDisable()
	{
		AddMaxLife.RemoveListener(OnAddMaxLife);
		AddDmg.RemoveListener(OnAddDmg);
		AddSpeed.RemoveListener(OnAddSpeed);
		AddFireRate.RemoveListener(OnAddFireRate);
		AddPowerUpEfficiency.RemoveListener(OnAddPowerUpEfficiency);

		Heal.RemoveListener(OnHeal);
		TakeDamage.RemoveListener(OnTakeDamage);
		Die.RemoveListener(OnDie);
	}

	private void OnHeal(int heal)
	{
		playerHP += heal;
		if (playerHP > playerData.playerBaseHP)
			playerHP = playerData.playerBaseHP;

		healthBar.SetHealth(playerHP);
		characterAudio.PlayOneShot(powerUpSounds[0]);

		gunAnimator.SetTrigger("Eat");
	}

	public void OnBulletTime()
	{
		characterAudio.PlayOneShot(powerUpSounds[1]);
		gunAnimator.SetTrigger("Eat");
		BulletTime.Invoke();
	}

	void OnTakeDamage(int damage)
	{
		//TODO Feedback hit
		playerHP -= damage;
		healthBar.SetHealth(playerHP);

		if (playerHP <= 0)
			Die.Invoke();
	}

	void OnDie()
	{
		Debug.Log($"Player died");
	}

	void OnAddMaxLife(int amount)
	{
		playerData.playerBaseHP += amount;
		// TODO Update UI.

		healthBar.SetMaxHealth(playerData.playerBaseHP);
		playerHP += amount;
		healthBar.SetHealth(playerData.playerBaseHP);

	}
	void OnAddDmg(int amount) => playerData.addedDamage += amount;
	void OnAddSpeed(float multiplier) => playerData.playerSpeed *= multiplier;
	void OnAddFireRate(float multiplier) => playerData.fireRate *= multiplier;

	void OnAddPowerUpEfficiency(float multiplier)
	{
		playerData.healAmount *= (int)multiplier;
		playerData.slowTime *= multiplier;
	}

	public void ToggleCamera(bool roaming)
	{
		this.roaming = roaming;
		Cursor.lockState = roaming ? CursorLockMode.Locked : CursorLockMode.None;
		Cursor.visible = !roaming;
	}

	#endregion

	#region Inputs

	public void Move(InputAction.CallbackContext context)
	{
		if (context.performed)
		{
			movements = new Vector3(context.ReadValue<Vector2>().x, 0f, context.ReadValue<Vector2>().y);
		}
		else if (context.canceled)
		{
			movements = Vector3.zero;
		}
	}

	public void Look(InputAction.CallbackContext context)
	{
		mouseRotation = context.ReadValue<Vector2>();
	}

	public void Fire(InputAction.CallbackContext context)
	{
		if (context.performed)
		{
			Shoot();
		}
	}

	#endregion

	private IEnumerator ShootCooldown()
	{
		yield return new WaitForSeconds(playerData.fireRate);
		canShoot = true;
	}
}
