using System.Collections;
using System.Collections.Generic;
using MyBox;
using UnityEngine;
using UnityEngine.UI;

public class CanvasController : MonoBehaviour {

	[SerializeField] Sprite[] orbCountdown;
	[SerializeField] Image orb;
	
	// Start is called before the first frame update
	[ButtonMethod]
	void Start() {
		StartCoroutine(Countdown());
	}

	// Update is called once per frame
	void Update(){
		
	}

	IEnumerator Countdown() {
		for (int i = 0; i < 10; ++i) {
			orb.sprite = orbCountdown[i];
			yield return new WaitForSeconds(1f);
		}
		GameManager.instance.SelectRandomEffect();
		StartCoroutine(Countdown());
	}
}
