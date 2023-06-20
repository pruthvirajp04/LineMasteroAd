using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SelectModeButton : MonoBehaviour
{
	public GameMode mode;
	public Text tModeName, tProgress;
	public Image iProgress;

	void OnEnable ()
	{
		Invoke ("Init", 0.1F);
	}

	void Init ()
	{
		// ban đầu set fillamount = 0 để làm hiệu ứng tăng dần
		iProgress.fillAmount = 0;

		int lvMax = 0, totalLevel = 0;
		switch (mode) {
		case GameMode.Normal: 
			tModeName.text = LanguageManager.GetText (LanguageKey.normal_mode);
			lvMax = GameManager.dataSave.normalLevelMax;
			totalLevel = GameManager.LevelDataDict [GameMode.Normal].Count;
			break;
		case GameMode.Double:
			tModeName.text = LanguageManager.GetText (LanguageKey.double_mode);
			lvMax = GameManager.dataSave.doubleLevelMax;
			totalLevel = GameManager.LevelDataDict [GameMode.Double].Count;
			break;
		case GameMode.Copy:
			tModeName.text = LanguageManager.GetText (LanguageKey.copy_mode);
			lvMax = GameManager.dataSave.copyLevelMax;
			totalLevel = GameManager.LevelDataDict [GameMode.Copy].Count;
			break;
		}
		float lv = 0;
		tProgress.text = lv + "/" + totalLevel;
		// hiệu ứng tăng dần số level đã đạt được
		DOTween.To (() => lv, x => lv = x, lvMax, 0.5F).SetDelay (1F).OnUpdate (() => {
			tProgress.text = Mathf.RoundToInt (lv) + "/" + totalLevel;
			iProgress.fillAmount = (float)lv / totalLevel;
		});

	}
}
