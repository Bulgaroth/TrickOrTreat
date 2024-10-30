using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    #region Attributes

    [SerializeField] private float speed;

    [SerializeField] private Vector2 cameraRange;
    
    private Camera camera;
    private CharacterController _controller;
    
    private float verticaRotation;
    private Vector3 movements;
    private Vector2 mouseRotation;
    #endregion
    
    
    #region Unity API

    private void Awake()
    {
        camera = Camera.main;
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
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);
        
        float curSpeedX = speed * movements.z;
        float curSpeedY = speed * movements.x;
        Vector3 moveDirection = (forward * curSpeedX) + (right * curSpeedY);
        
        _controller.Move( moveDirection * speed * Time.deltaTime);
        //transform.position += movements * speed * Time.deltaTime;
    }

    void RotateCamera()
    {
        transform.Rotate(0, mouseRotation.x,0);

        
        verticaRotation -= mouseRotation.y;
        verticaRotation = Mathf.Clamp(verticaRotation, cameraRange.x, cameraRange.y);
        camera.transform.localRotation = Quaternion.Euler(verticaRotation, 0, 0);
    }
    
    
    #endregion

    #region Inputs

    public void Move(InputAction.CallbackContext context)
    {
        //Debug.Log("Move");
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
