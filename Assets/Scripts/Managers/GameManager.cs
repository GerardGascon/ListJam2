using System;
using System.Collections;
using System.Collections.Generic;
using GometGames.Tools;
using MyBox;
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
	
	// Start is called before the first frame update
	void Awake() {
		instance = this;
	}

	void Update() {
		CurrentTime += Time.deltaTime;
		float ratio = CurrentTime / time;
		clockPivot.rotation = Quaternion.Euler(0f, 0f, ratio * 360f - 180f);
		if (CurrentTime >= time - doorTime) {
			Debug.Log("DoorOpened");
			if (CurrentTime >= time) {
				Debug.Log("TimesUp");
			}
		}
	}

	public OrbTypes SelectRandomEffect() {
		OrbTypes selected;
		do {
			selected = (OrbTypes)Random.Range(1, 9);
		} while (selected == currentType || selected == _lastType);
		
		ChangeEventCall(selected);
		return selected;
	}

	[Space, SerializeField] OrbTypes forceEventChangeType = OrbTypes.None;
	[ButtonMethod]
	void ForceEventChange() {
		if (!Application.isPlaying) return;
		ChangeEventCall(forceEventChangeType);
	}
	
	void ChangeEventCall(OrbTypes type) {
		if(currentType != OrbTypes.None) orbEndEffects[currentType].Invoke();
		_lastType = currentType;
		currentType = type;
		if(type != OrbTypes.None) orbStartEffects[currentType].Invoke();
	}
}
