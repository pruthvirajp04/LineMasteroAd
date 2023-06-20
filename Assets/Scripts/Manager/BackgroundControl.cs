using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BackgroundControl : MonoBehaviour
{
	public static BackgroundControl instance;
	bool isShowingBlurBackground = true;

	void Awake ()
	{
		instance = this;
	}

	public List<Sprite> listBlurBackground;
	public List<Sprite> listBackground;

	public Image iBackground, iBackgroundBlur;

	public void ChangeBackground (bool isBlur, float time = 0)
	{
		// set background theo mode 
		iBackground.sprite = listBackground [(int)GameManager.currentGameMode];
		iBackgroundBlur.sprite = listBlurBackground [(int)GameManager.currentGameMode];
		// hiệu ứng mờ dần và bât ,tắt background cần thiết
		if (isBlur) {
			iBackgroundBlur.enabled = true;
			iBackground.DOFade (0, time).OnComplete (() => {
				iBackground.enabled = false;
			});
		} else {
			iBackground.enabled = true;
			iBackground.DOFade (1, time).OnComplete (() => {
				iBackgroundBlur.enabled = false;
			});
		}
	}
}
