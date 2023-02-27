using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerController : MonoBehaviour {

	[SerializeField] float speed = 50f;
	[SerializeField, Range(0f, .5f)] float acceleration = .1f;
	[SerializeField, Range(0f, .5f)] float flipSmooth = .1f;
	[SerializeField] float jumpForce = 10f;
	
	Vector2 _input;
	float _xVelocity, _flipVelocity;
	int _facingDirection = 1;
	Rigidbody2D _rb2d;

	[Space]
	[SerializeField] LayerMask groundMask;
	[SerializeField, Range(0f, 1f)] float feetHeight;
	[SerializeField, Range(0f, 2f)] float feetWidth;
	[SerializeField] Transform feetPos;
	bool _isGrounded;

	[SerializeField, Range(0f, .5f)] float coyoteTime;
	float _coyoteTime;
	[SerializeField, Range(0f, .5f)] float jumpBuffer;
	float _jumpBuffer;

	void Awake() {
		_rb2d = GetComponent<Rigidbody2D>();
	}

	void Update() {
		_input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
		_facingDirection = _input.x switch {
			> 0f => 1,
			< 0f => -1,
			_ => _facingDirection
		};

		transform.localScale =
			new Vector3(Mathf.SmoothDamp(transform.localScale.x, _facingDirection, ref _flipVelocity, flipSmooth),
				transform.localScale.y, transform.localScale.z);

		_isGrounded = Physics2D.OverlapBox(feetPos.position, new Vector2(feetWidth, feetHeight), 0f, groundMask);
		if (_isGrounded && _rb2d.velocity.y <= 0f) _coyoteTime = coyoteTime;
		if (Input.GetKeyDown(KeyCode.Space)) _jumpBuffer = jumpBuffer;

		_jumpBuffer -= Time.deltaTime;
		_coyoteTime -= Time.deltaTime;
	}

	void FixedUpdate() {
		_rb2d.velocity = new Vector2(Mathf.SmoothDamp(_rb2d.velocity.x, _input.x * speed, ref _xVelocity, acceleration), _rb2d.velocity.y);

		if (_coyoteTime > 0f && _jumpBuffer > 0f) {
			_rb2d.velocity = new Vector2(_rb2d.velocity.x, 0f);
			_rb2d.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
			_coyoteTime = _jumpBuffer = 0f;
		}
	}

	void OnDrawGizmosSelected() {
		Gizmos.color = Color.red;
		if (feetPos) Gizmos.DrawWireCube(feetPos.position, new Vector3(feetWidth, feetHeight));
	}
}
