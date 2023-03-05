using System.Collections;
using System.Collections.Generic;
using SimpleTools.AudioManager;
using SimpleTools.SceneManagement;
using UnityEngine;

public class SceneLoad : MonoBehaviour {

	[SerializeField] bool stopMusic = true;
	
	public void LoadScene(int sceneIndex) {
		if(stopMusic) AudioManager.instance.StopAll();
		Loader.Load(sceneIndex);
	}
}
