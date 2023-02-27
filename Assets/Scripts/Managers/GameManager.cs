using System.Collections;
using System.Collections.Generic;
using GometGames.Tools;
using MyBox;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public enum OrbTypes {
	Blue,
	Red,
	InvertedControls,
	HighGravity,
	LowGravity,
	InvertedGravity,
	DarkRoom,
	HighSpeed,
	LowSpeed
};

public class GameManager : MonoBehaviour {

	[SerializeField] GenericDictionary<OrbTypes, UnityEvent> orbEffects;
	[SerializeField] OrbTypes currentType;
	
	// Start is called before the first frame update
	void Start(){
		
	}

	// Update is called once per frame
	void Update(){
		
	}

	void ChangeEventCall(OrbTypes type) {
		currentType = type;
		orbEffects[currentType].Invoke();
	}
}
