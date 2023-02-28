using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapActivation : MonoBehaviour {

	Tilemap _tilemap;
	Collider2D _tilemapColliders;

	[SerializeField] float disabledTransparency;
	[SerializeField] float transitionDuration;

	void Awake() {
		_tilemap = GetComponent<Tilemap>();
		_tilemapColliders = GetComponent<Collider2D>();
		
		_tilemapColliders.enabled = false;
	}

	public void ChangeTilemapState(bool enabled) {
		_tilemapColliders.enabled = _enabled = enabled;
	}

	bool _enabled;
	float _start;
	void Update() {
		float alpha = _enabled
			? Mathf.Lerp(disabledTransparency, 1, (Time.time - _start) / transitionDuration)
			: Mathf.Lerp(1, disabledTransparency, (Time.time - _start) / transitionDuration);

		alpha = Mathf.Round(alpha * 5);
		alpha /= 5f;

		Color color = _tilemap.color;
		color = new Color(color.r, color.g, color.b, alpha);
		_tilemap.color = color;
	}
}
