using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    #region Attributes

    [SerializeField] private SC_PlayerData playerData;

    [SerializeField] private int playerHP;
    
    [SerializeField] private Vector2 cameraRange;
    [SerializeField] private float gravity = 100.0f;
    
    private Camera mainCamera;
    private CharacterController _controller;
    
    private float verticaRotation;
    private Vector3 movements;
    private Vector2 mouseRotation;
    
    #endregion

    #region Events

    [HideInInspector] public UnityEvent<int> Heal;
    [HideInInspector] public UnityEvent<int> TakeDamage;
    [HideInInspector] public UnityEvent Die;
    
    #endregion
    
    #region Unity API

    private void Awake()
    {
        mainCamera = Camera.main;
        _controller = GetComponent<CharacterController>();
        playerHP = playerData.playerBaseHP;
    }

    void Update()
    {
        MovePlayer();
        RotateCamera();
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
        transform.Rotate(0, mouseRotation.x,0);
        
        verticaRotation -= mouseRotation.y;
        verticaRotation = Mathf.Clamp(verticaRotation, cameraRange.x, cameraRange.y);
        mainCamera.transform.localRotation = Quaternion.Euler(verticaRotation, 0, 0);
    }

    
    
    #endregion

    #region Event Handlers

    private void OnEnable()
    {
        Heal.AddListener(OnHeal);
        TakeDamage.AddListener(OnTakeDamage);
        Die.AddListener(OnDie);
    }


    private void OnDisable()
    {
        Heal.RemoveListener(OnHeal);
        TakeDamage.RemoveListener(OnTakeDamage);
        Die.RemoveListener(OnDie);
    }

    private void OnHeal(int heal)
    {
        Debug.Log($"Player heal {heal} hp");
        
        playerHP += heal;
    }

    void OnTakeDamage(int damage)
    {
        Debug.Log($"Player take {damage} damage");
        
        playerHP -= damage;
        
        if (playerHP <= 0)
            Die.Invoke();
    }

    void OnDie()
    {
        Debug.Log($"Player died");
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

    #endregion
}
