using System;
using System.Collections;
using System.Collections.Generic;
using GometGames.Tools;
using MyBox;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

[System.Serializable]
public enum OrbTypes {
	None,
	Blue,
	Red,
	Purple,
	InvertedControls,
	HighGravity,
	LowGravity,
	DarkRoom,
	Freeze
};

public class GameManager : MonoBehaviour {

	[SerializeField] GenericDictionary<OrbTypes, UnityEvent> orbStartEffects;
	[SerializeField] GenericDictionary<OrbTypes, UnityEvent> orbEndEffects;
	[SerializeField] OrbTypes currentType = OrbTypes.None;
	OrbTypes _lastType = OrbTypes.None;

	public static GameManager instance;
	
	// Start is called before the first frame update
	void Awake() {
		instance = this;
	}

	public OrbTypes SelectRandomEffect() {
		OrbTypes selected;
		Array values = Enum.GetValues(typeof(OrbTypes));
		do {
			selected = (OrbTypes)values.GetValue(Random.Range(1, values.Length));
		} while (selected == currentType || selected == _lastType);
		
		Debug.Log(selected);
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
