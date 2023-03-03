using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using GometGames.Tools;
using MyBox;
using SimpleTools.AudioManager;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

[System.Serializable]
public enum OrbTypes {
	None = 0,
	Blue = 1,
	Red = 2,
	Purple = 3,
	InvertedControls = 4,
	HighGravity = 5,
	LowGravity = 6,
	DarkRoom = 7,
	Freeze = 8
};

public class GameManager : MonoBehaviour {

	[SerializeField] GenericDictionary<OrbTypes, UnityEvent> orbStartEffects;
	[SerializeField] GenericDictionary<OrbTypes, UnityEvent> orbEndEffects;
	[SerializeField] OrbTypes currentType = OrbTypes.None;
	OrbTypes _lastType = OrbTypes.None;

	public static GameManager instance;

	[Header("Clock")]
	[SerializeField] int time = 180;
	public float CurrentTime { private set; get; }
	[SerializeField] int doorTime = 30;

	[SerializeField] RectTransform clockPivot;

	bool _doorOpened, _dead;

	[SerializeField] UnityEvent doorOpenEvent;
	[SerializeField] UnityEvent timeEndEvent;
	
	// Start is called before the first frame update
	void Awake() {
		instance = this;
	}

	void Start() {
		AudioManager.instance.FadeIn("Viento", 1f);
	}

	void Update() {
		if (_stopped) return;
		
		CurrentTime += Time.deltaTime;
		float ratio = CurrentTime / time;
		clockPivot.rotation = Quaternion.Euler(0f, 0f, ratio * 360f - 180f);
		if (CurrentTime >= time - doorTime) {
			if (!_doorOpened) {
				_doorOpened = true;
				AudioManager.instance.Play("Puerta");
				doorOpenEvent.Invoke();
			}
			if (CurrentTime >= time && !_dead) {
				_dead = true;
				timeEndEvent.Invoke();
			}
		}
	}

	[SerializeField] CanvasGroup group;
	[SerializeField] CanvasController controller;
	bool _stopped;
	public void DisableManager() {
		group.DOFade(0, 1f);
		controller.AbortCoroutine();
		group.interactable = false;
		_stopped = true;
	}

	public OrbTypes SelectRandomEffect() {
		OrbTypes selected;
		do {
			selected = (OrbTypes)Random.Range(1, 9);
		} while (selected == currentType || selected == _lastType);
		
		ChangeEventCall(selected);
		return selected;
	}

	void ChangeEventCall(OrbTypes type) {
		if(currentType != OrbTypes.None) orbEndEffects[currentType].Invoke();
		_lastType = currentType;
		currentType = type;
		if(type != OrbTypes.None) orbStartEffects[currentType].Invoke();
	}
}
