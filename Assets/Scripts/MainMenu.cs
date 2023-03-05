using System.Collections;
using System.Collections.Generic;
using SimpleTools.AudioManager;
using UnityEngine;

public class MainMenu : MonoBehaviour{
	// Start is called before the first frame update
	void Start(){
		AudioManager.instance.Play("menu");
	}
}
