using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using MyBox;
using UnityEngine;
using UnityEngine.UI;

public class CanvasController : MonoBehaviour {

	[SerializeField] Sprite[] orbCountdown;
	[SerializeField] Image orb;

	[SerializeField] DOTweenAnimation effectTween;
	Image _effectImage;
	[SerializeField] Sprite[] effectSprites;
	
	// Start is called before the first frame update
	[ButtonMethod]
	void Start() {
		_effectImage = effectTween.GetComponent<Image>();
		StartCoroutine(Countdown());
	}

	// Update is called once per frame
	void Update(){
		
	}

	bool _abort;
	public void AbortCoroutine() => _abort = true;
	
	IEnumerator Countdown() {
		for (int i = 0; i < 10; ++i) {
			orb.sprite = orbCountdown[i];
			yield return new WaitForSeconds(1f);
			if (_abort) yield break;
		}

		_effectImage.sprite = effectSprites[(int)GameManager.instance.SelectRandomEffect() - 1];
		effectTween.DORestart();
		StartCoroutine(Countdown());
	}
}
