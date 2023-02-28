using System;
using System.Collections;
using System.Collections.Generic;
using SimpleTools;
using UnityEngine;

public class MovingPlatform : MonoBehaviour {

	[SerializeField] Vector2 endPosition;
	[SerializeField] float moveTime;
	[SerializeField] float stopDuration;

	Vector2 _start, _end;
	float _speed;

	Coroutine _swapRoutine;

	// Start is called before the first frame update
	void Awake() {
		_start = transform.position;
		_end = _start + endPosition;
		_speed = endPosition.magnitude / moveTime;
	}

	// Update is called once per frame
	void FixedUpdate() {
		StartCoroutine(FixedUpdateCoroutine());
	}

	IEnumerator FixedUpdateCoroutine() {
		yield return new WaitForFixedUpdate();
		transform.position = Vector3.MoveTowards(transform.position, _end, _speed * Time.deltaTime);

		if (Math.Abs(transform.position.sqrMagnitude - _end.sqrMagnitude) < 0.05f) {
			_swapRoutine ??= StartCoroutine(SwapCoordinates());
		}
	}

	IEnumerator SwapCoordinates() {
		yield return new WaitForSeconds(stopDuration);
		(_start, _end) = (_end, _start);
		_swapRoutine = null;
	}

	void OnTriggerEnter2D(Collider2D col) {
		if (col.CompareTag("Player")) col.transform.SetParent(transform);
	}
	void OnTriggerExit2D(Collider2D col) {
		if(col.CompareTag("Player")) col.transform.SetParent(null);
	}

	void OnDrawGizmos() {
		Gizmos.color = Color.green;
		Gizmos.DrawLine(transform.position, transform.position + (Vector3)endPosition);
		Gizmos.DrawSphere(transform.position, 0.25f);
		Gizmos.DrawSphere(transform.position + (Vector3)endPosition, 0.25f);
	}
}
