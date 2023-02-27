using System.Collections;
using System.Collections.Generic;
using GometGames.Tools;
using MyBox;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public enum OrbTypes {
	None,
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

	[SerializeField] GenericDictionary<OrbTypes, UnityEvent> orbStartEffects;
	[SerializeField] GenericDictionary<OrbTypes, UnityEvent> orbEndEffects;
	[SerializeField] OrbTypes currentType = OrbTypes.None;
	
	// Start is called before the first frame update
	void Start(){
		
	}

	// Update is called once per frame
	void Update(){
		
	}

	[Space, SerializeField] OrbTypes forceEventChangeType = OrbTypes.None;
	[ButtonMethod]
	void ForceEventChange() {
		if (!Application.isPlaying) return;
		ChangeEventCall(forceEventChangeType);
	}
	
	void ChangeEventCall(OrbTypes type) {
		if(currentType != OrbTypes.None) orbEndEffects[currentType].Invoke();
		currentType = type;
		if(type != OrbTypes.None) orbStartEffects[currentType].Invoke();
	}
}
