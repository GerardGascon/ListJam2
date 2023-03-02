using System.Collections;
using System.Collections.Generic;
using SimpleTools.AudioManager;
using SimpleTools.SceneManagement;
using UnityEngine;

public class SceneLoad : MonoBehaviour{

	public void LoadScene(int sceneIndex) {
		AudioManager.instance.FadeOut("Viento", 1f);
		Loader.Load(sceneIndex);
	}
}
