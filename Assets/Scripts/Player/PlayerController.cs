using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using MyBox;
using SimpleTools.Cinemachine;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerController : MonoBehaviour {

	[Header("Stats")]
	[SerializeField] float speed = 50f;
	float _currentSpeed;
	[SerializeField, Range(0f, .5f)] float acceleration = .1f;
	[SerializeField, Range(0f, .5f)] float flipSmooth = .1f;
	[SerializeField] float jumpForce = 10f;
	[SerializeField, Range(0f, 1f)] float gradualJumpMultiplier = .5f;
	
	Vector2 _input;
	float _xVelocity, _flipVelocity;
	int _facingDirection = 1;
	bool _cancelJump;
	Rigidbody2D _rb2d;

	[Header("Jump Management")]
	[SerializeField] LayerMask groundMask;
	[SerializeField, Range(0f, 1f)] float feetHeight;
	[SerializeField, Range(0f, 2f)] float feetWidth;
	[SerializeField] Transform feetPos;
	bool _isGrounded;

	[SerializeField, Range(0f, .5f)] float coyoteTime;
	float _coyoteTime;
	[SerializeField, Range(0f, .5f)] float jumpBuffer;
	float _jumpBuffer;

	[Header("Camera")]
	[SerializeField, OverrideLabel("vCam")] CinemachineVirtualCamera vCam;
	Transform _cameraTarget;

	Animator _anim;
	static readonly int Speed = Animator.StringToHash("Speed");
	static readonly int Grounded = Animator.StringToHash("Grounded");
	static readonly int Die = Animator.StringToHash("Die");

	bool _dead;

	void Awake() {
		_rb2d = GetComponent<Rigidbody2D>();
		_anim = GetComponent<Animator>();
		
		_cameraTarget = new GameObject("Camera Target").transform;
		vCam.m_Follow = _cameraTarget;

		ResetSpeed();
	}

	void Update() {
		Vector3 localScale;
		if (_dead) {
			localScale = transform.localScale;
			localScale = new Vector3(Mathf.SmoothDamp(localScale.x, _facingDirection, ref _flipVelocity, flipSmooth),
				localScale.y, localScale.z);
			transform.localScale = localScale;
			return;
		}
		
		_input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
		_facingDirection = _input.x switch {
			> 0f => 1,
			< 0f => -1,
			_ => _facingDirection
		};

		localScale = transform.localScale;
		localScale = new Vector3(Mathf.SmoothDamp(localScale.x, _facingDirection, ref _flipVelocity, flipSmooth),
				localScale.y, localScale.z);
		transform.localScale = localScale;

		_isGrounded = Physics2D.OverlapBox(feetPos.position, new Vector2(feetWidth, feetHeight), 0f, groundMask);
		_anim.SetBool(Grounded, _isGrounded);
		if (_isGrounded && _rb2d.velocity.y <= 0f) _coyoteTime = coyoteTime;
		
		if (Input.GetKeyDown(KeyCode.Space)) _jumpBuffer = jumpBuffer;
		if (Input.GetKeyUp(KeyCode.Space)) _cancelJump = true;

		_jumpBuffer -= Time.deltaTime;
		_coyoteTime -= Time.deltaTime;
	}

	void FixedUpdate() {
		if (_dead) return;
		
		_rb2d.velocity = new Vector2(Mathf.SmoothDamp(_rb2d.velocity.x, _input.x * _currentSpeed, ref _xVelocity, acceleration), _rb2d.velocity.y);
		_anim.SetFloat(Speed, Mathf.Abs(_rb2d.velocity.x));

		if (_coyoteTime > 0f && _jumpBuffer > 0f) {
			_rb2d.velocity = new Vector2(_rb2d.velocity.x, 0f);
			_rb2d.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
			_coyoteTime = _jumpBuffer = 0f;
		}

		if (_cancelJump) {
			if(_rb2d.velocity.y > 0f) _rb2d.velocity = new Vector2(_rb2d.velocity.x, _rb2d.velocity.y * gradualJumpMultiplier);
			_cancelJump = false;
		}
	}

	void LateUpdate() {
		if (_dead) return;
		
		float yPos = _isGrounded && _rb2d.velocity.y <= 0f ? transform.position.y : _cameraTarget.position.y;
		_cameraTarget.position = new Vector3(transform.position.x, yPos);
	}

	void OnDrawGizmosSelected() {
		Gizmos.color = Color.red;
		if (feetPos) Gizmos.DrawWireCube(feetPos.position, new Vector3(feetWidth, feetHeight));
	}

	void OnTriggerEnter2D(Collider2D col) {
		if (col.CompareTag("Ghost")) {
			_anim.SetTrigger(Die);
			_dead = true;
			ScreenShake.Shake(20f, .5f);
		}
	}

#region OrbPowerups
	public void ResetSpeed() {
		_currentSpeed = speed;
	}
	public void SetSpeed(float newSpeed) {
		_currentSpeed = newSpeed;
	}
#endregion
}
