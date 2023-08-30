using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PausePopup : MonoBehaviour
{
	public CustomToggle musicToggle, soundToggle;
	public Text tMenu, tContinue, tSound, tMusic;
	public static PausePopup instance;
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

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if (PlayerPrefs.HasKey("DoneReplay"))
        {
            PlayerPrefs.DeleteKey("DoneReplay");
            if (PlayerPrefs.GetInt("InitialMusic") == 1)
            {
                AudioManager.SetMusicStatus(true);
            }
            if (PlayerPrefs.GetInt("InitialSound") == 1)
            {
                AudioManager.SetMusicStatus(true);
            }
        }
    }

    void OnDisable ()
	{
		GameCanvas.instance.blackLayer.gameObject.SetActive (false);
	}

	public void Menu ()
	{
        AudioManager.instance.StoreOriginalStatus();
        AudioManager.SetMusicStatus(false);
        AudioManager.SetSoundStatus(false);

        GlanceAds.ReplayAd("ReplayOnHome");
        GlanceAds.EndAnalytics(GameManager.currentLevel);		
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
