using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostController : MonoBehaviour {

	[SerializeField] float speed;
	[SerializeField] Transform player;

	Animator _anim;
	Rigidbody2D _rb2d;
	
	// Start is called before the first frame update
	void Awake() {
		_anim = GetComponent<Animator>();
		_rb2d = GetComponent<Rigidbody2D>();
	}

	// Update is called once per frame
	void FixedUpdate() {
		if (_hasKilled) return;
		
		_rb2d.position = Vector2.MoveTowards(_rb2d.position, player.position, Time.deltaTime * speed);
	}

	bool _hasKilled;
	static readonly int Kill1 = Animator.StringToHash("Kill");

	public void Kill() {
		_hasKilled = true;
		_anim.SetTrigger(Kill1);
	}
}
