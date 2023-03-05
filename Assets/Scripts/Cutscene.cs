using System;
using System.Collections;
using System.Collections.Generic;
using MyBox;
using SimpleTools.AudioManager;
using SimpleTools.DialogueSystem;
using SimpleTools.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class Cutscene : MonoBehaviour {

	[SerializeField] Dialogue _dialogue;
	
	[SerializeField] float fadeDuration;
	[SerializeField] float[] times;

	bool _visible = true;
	[SerializeField] Image occluder;
	
	// Start is called before the first frame update
	IEnumerator Start() {
		foreach (float t in times) {
			yield return new WaitForSeconds(fadeDuration);
			if (!_visible) {
				_visible = true;
				_start = Time.time;
			}
			DialogueManager.instance.Dialogue(_dialogue);
			yield return new WaitForSeconds(t);
			_visible = false;
			_start = Time.time;
			yield return new WaitForSeconds(fadeDuration);
		}
		AudioManager.instance.Stop("menu");
		Loader.Load(2);
	}

	float _start;
	void Update() {
		float alpha = _visible
			? Mathf.Lerp(1, 0, (Time.time - _start) / fadeDuration)
			: Mathf.Lerp(0, 1, (Time.time - _start) / fadeDuration);

		alpha = Mathf.Round(alpha * 5);
		alpha = alpha / 5f;
		
		occluder.SetAlpha(alpha);
	}
}
