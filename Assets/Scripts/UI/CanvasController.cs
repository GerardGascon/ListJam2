using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using MyBox;
using SimpleTools.AudioManager;
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

	OrbTypes _currentType = OrbTypes.None;
	void PlayEffectMusic() {
		switch (_currentType) {
			case OrbTypes.None:
				AudioManager.instance.Play("intro");
				break;
			case OrbTypes.Blue:
				AudioManager.instance.Play("azul");
				break;
			case OrbTypes.Red:
				AudioManager.instance.Play("rojo");
				break;
			case OrbTypes.Purple:
				AudioManager.instance.Play("ambos");
				break;
			case OrbTypes.InvertedControls:
				AudioManager.instance.Play("invertido");
				break;
			case OrbTypes.HighGravity:
				AudioManager.instance.Play("alta");
				break;
			case OrbTypes.LowGravity:
				AudioManager.instance.Play("baja");
				break;
			case OrbTypes.DarkRoom:
				AudioManager.instance.Play("oscuro");
				break;
			case OrbTypes.Freeze:
				AudioManager.instance.Play("rojo");
				break;
			default:
				throw new ArgumentOutOfRangeException();
		}
	}
	
	IEnumerator Countdown() {
		PlayEffectMusic();
		
		for (int i = 0; i < 10; ++i) {
			orb.sprite = orbCountdown[i];
			yield return new WaitForSeconds(1f);
			if (_abort) yield break;
		}

		_currentType = GameManager.instance.SelectRandomEffect();

		_effectImage.sprite = effectSprites[(int)_currentType - 1];
		effectTween.DORestart();
		StartCoroutine(Countdown());
	}
}
