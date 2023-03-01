using System;
using System.Collections;
using System.Collections.Generic;
using MyBox;
using SimpleTools.DialogueSystem;
using UnityEngine;
using UnityEngine.UI;

public class WinScreen : MonoBehaviour {

	[SerializeField] GameObject[] orbs;
	[SerializeField] Dialogue dialogue;
	
	[SerializeField] float fadeDuration;

	[SerializeField] Image occluder;
	
	// Start is called before the first frame update
	void Awake(){
		for (int i = 0; i < orbs.Length; i++) orbs[i].SetActive(i + 1 <= PlayerPrefs.GetInt("OrbsFound"));
	}

	// Start is called before the first frame update
	IEnumerator Start() {
		yield return new WaitForSeconds(fadeDuration);
		DialogueManager.instance.Dialogue(dialogue);
	}

	float _start;
	void Update() {
		float alpha = Mathf.Lerp(1, 0, (Time.time - _start) / fadeDuration);

		alpha = Mathf.Round(alpha * 5);
		alpha /= 5f;
		
		occluder.SetAlpha(alpha);
	}
}
