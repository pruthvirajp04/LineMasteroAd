using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PausePopup : MonoBehaviour
{
	public CustomToggle musicToggle, soundToggle;
	public Text tMenu, tContinue, tSound, tMusic;

	void OnEnable ()
	{
		GameCanvas.instance.blackLayer.gameObject.SetActive (true);
		tMenu.text = LanguageManager.GetText (LanguageKey.quit);
		tContinue.text = LanguageManager.GetText (LanguageKey.Continue);
		tSound.text = LanguageManager.GetText (LanguageKey.sounds_label);
		tMusic.text = LanguageManager.GetText (LanguageKey.music_label);
		musicToggle.value = GameManager.dataSave.isMusicOn;
		soundToggle.value = GameManager.dataSave.isSoundOn;

	}

	void OnDisable ()
	{
		GameCanvas.instance.blackLayer.gameObject.SetActive (false);
	}

	public void Menu ()
	{
		GlanceAds.EndAnalytics(GameManager.currentLevel);
		GlanceAds.ReplayAd("ReplayOnHome");
		if (GameConfig.instance.tutorialControl.haveTutorial) {
			GameConfig.instance.tutorialControl.gameObject.SetActive (false);
		}
		GameManager.gameState = GameState.SelectLevel;
	}

	public void Continue ()
	{
		GlanceAds.instance.resumeEvent();
	}

	public void ChangeSoundStatus ()
	{
		Debug.Log ("Sound is : " + !soundToggle.value);
		AudioManager.SetSoundStatus (!soundToggle.value);
	}

	public void ChangeMusicStatus ()
	{
		Debug.Log ("Music is : " + !musicToggle.value);
		AudioManager.SetMusicStatus (!musicToggle.value);
	}

}
