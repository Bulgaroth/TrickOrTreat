using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Controls;

public class HeadBobbing : MonoBehaviour
{
    #region Attributes

    public bool _isEnable;

    [SerializeField] private SC_HeadBobbingData _data;

    private Camera _camera;
    [SerializeField] private Transform _cameraHolder;

    private float _toggleSpeed = 3.0f;
    private Vector3 _startPos;
    private CharacterController _controller;

    private bool isMoving;
    
    #endregion

    #region Unity API

    void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _camera = Camera.main;
        _startPos = _camera.transform.localPosition;
    }

    void Update()
    {
        if (!_isEnable) return;
        
        CheckMotion();
        
        if (!isMoving)
            ResetPosition();
        //_camera.LookAt(FocusTarget());
    }

    #endregion

    #region Methods

    private void CheckMotion()
    {
        var velocity = _controller.velocity;
        float speed = new Vector3(velocity.x, 0, velocity.z).magnitude;

        isMoving = speed > _toggleSpeed;
        
        if (!isMoving) return;
        //if (!_controller.isGrounded) return;
        
        PlayMotion(FootStepMotion());
    }
    
    private Vector3 FootStepMotion()
    {
        float frequency = _data.frequency;
        float amplitude = _data.amplitude;
        
        Vector3 pos = Vector3.zero;
        pos.y += Mathf.Sin(Time.time * frequency) * amplitude;
        pos.x += Mathf.Cos(Time.time * frequency / 2) * amplitude * 2;
        Debug.Log($"StepMotion: {pos}");
        return pos;
    }

    private void PlayMotion(Vector3 motion)
    {
        Debug.Log("Play Motion");
        
        _camera.transform.localPosition += motion;
    }

    private void ResetPosition()
    {
        if (Vector3.Distance(_camera.transform.localPosition, _startPos) < 0.001f)
        {
            _camera.transform.localPosition = _startPos;    
            return;
        }
        
        _camera.transform.localPosition = Vector3.Lerp(_camera.transform.localPosition, _startPos, 1.5f * Time.deltaTime);
    }

    private Vector3 FocusTarget()
    {
        var position = transform.position;
        Vector3 pos = new Vector3(position.x,
            position.y + _cameraHolder.localPosition.y,
            position.z);
        pos += _cameraHolder.forward * 15.0f;
        return pos;
    }

    #endregion
    
}
