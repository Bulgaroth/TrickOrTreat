using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class HeadBobbing : MonoBehaviour
{
	#region Attributes

	private const float FOOTSTEP_COOLDOWN = 0.2f;

	public bool _isEnable;
	public bool alreadyStepped;

	[SerializeField] private SC_HeadBobbingData _data;
	[SerializeField] private AudioSource footStepsAudio;
	[Serializable] private class FootstepArray
	{
		public string tag;
		public List<AudioClip> footsteps;
	}
	[SerializeField] private List<FootstepArray> footstepsSounds;

	private Camera _camera;
	[SerializeField] private Transform _cameraHolder;

	private float _toggleSpeed = 3.0f;
	private Vector3 _startPos;
	private CharacterController _controller;

	private bool isMoving;
	public enum GroundType { Concrete, Grass, Tile, Car }
	public GroundType groundType;
	
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
		if (pos.y <= 0.002 && pos.y >= -0.002) PlayFootStep();
		pos.x += Mathf.Cos(Time.time * frequency / 2) * amplitude * 2;
		//Debug.Log($"StepMotion: {pos}");
		return pos;
	}

	private void PlayMotion(Vector3 motion)
	{
		//Debug.Log("Play Motion");
		
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

	private void PlayFootStep()
	{
		if (alreadyStepped) return;
		alreadyStepped = true;
		StartCoroutine(FootStepCooldown());

		print(groundType);
		var footstepArray = footstepsSounds[(int)groundType];
		footStepsAudio.PlayOneShot(footstepArray.footsteps[Random.Range(0, footstepArray.footsteps.Count)]);
	}

	#endregion

	private IEnumerator FootStepCooldown()
	{
		yield return new WaitForSeconds(FOOTSTEP_COOLDOWN);
		alreadyStepped = false;
	}
	
}
