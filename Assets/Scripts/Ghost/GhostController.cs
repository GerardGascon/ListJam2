using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostController : MonoBehaviour {

	[SerializeField] float speed;
	[SerializeField] Transform player;

	Rigidbody2D _rb2d;
	
	// Start is called before the first frame update
	void Awake() {
		_rb2d = GetComponent<Rigidbody2D>();
	}

	// Update is called once per frame
	void FixedUpdate() {
		_rb2d.position = Vector2.MoveTowards(_rb2d.position, player.position, Time.deltaTime * speed);
	}
}
