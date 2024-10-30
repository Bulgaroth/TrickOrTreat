using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    #region Attributes

    [SerializeField] private SC_PlayerData playerData;

    [SerializeField] private Vector2 cameraRange;
    
    private Camera mainCamera;
    private CharacterController _controller;
    
    private float verticaRotation;
    private Vector3 movements;
    private Vector2 mouseRotation;
    
    #endregion
    
    
    #region Unity API

    private void Awake()
    {
        mainCamera = Camera.main;
        _controller = GetComponent<CharacterController>();
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
        
        
        
        _controller.Move( moveDirection * Time.deltaTime);
    }

    void RotateCamera()
    {
        transform.Rotate(0, mouseRotation.x,0);
        
        verticaRotation -= mouseRotation.y;
        verticaRotation = Mathf.Clamp(verticaRotation, cameraRange.x, cameraRange.y);
        mainCamera.transform.localRotation = Quaternion.Euler(verticaRotation, 0, 0);
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
