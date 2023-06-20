using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class GlanceAds : MonoBehaviour
{
    public static GlanceAds instance;
    [DllImport("__Internal")]
    public static extern void LoadGlanceAds();
    [DllImport("__Internal")]
    public static extern void RewardedAd(string type);
    [DllImport("__Internal")]
    public static extern void ReplayAd(string type);
    [DllImport("__Internal")]
    public static extern void StartAnalytics();
    [DllImport("__Internal")]
    public static extern void ReplayAnalytics(int level);
    [DllImport("__Internal")]
    public static extern void EndAnalytics(int level);
    [DllImport("__Internal")]
    public static extern void LevelAnalytics(int level);
    // Start is called before the first frame update
    void onAwake()
    {
        LoadGlanceAds();
    }
    public void muteAudio(){
        AudioManager.instance.TurnMusicOff();
        AudioManager.instance.TurnSoundOff();
    }
    public void doneReplay(){
        if(PlayerPrefs.GetInt("Music") == 0)
        {
            AudioManager.instance.TurnMusicOn();
        }
        else
        {
            AudioManager.instance.TurnMusicOff();
        }
        if (PlayerPrefs.GetInt("Sound") == 0)
        {
            AudioManager.instance.TurnSoundOn();
        }
        else
        {
            AudioManager.instance.TurnSoundOff();
        }
    }
    public void doneReplay1f(){
        if(PlayerPrefs.GetInt("Music") == 0)
        {
            AudioManager.instance.TurnMusicOn();
        }
        else
        {
            AudioManager.instance.TurnMusicOff();
        }
        if (PlayerPrefs.GetInt("Sound") == 0)
        {
            AudioManager.instance.TurnSoundOn();
        }
        else
        {
            AudioManager.instance.TurnSoundOff();
        }
         StartCoroutine(UIManager.instance.ShowGameOverIE());
    }
    public void deleteGlanceKey(){
        PlayerPrefs.DeleteKey("firstGlance");
    }
    public void doneReplay1w(){
        if(PlayerPrefs.GetInt("Music") == 0)
        {
            AudioManager.instance.TurnMusicOn();
        }
        else
        {
            AudioManager.instance.TurnMusicOff();
        }
        if (PlayerPrefs.GetInt("Sound") == 0)
        {
            AudioManager.instance.TurnSoundOn();
        }
        else
        {
            AudioManager.instance.TurnSoundOff();
        } 
         StartCoroutine(UIManager.instance.ShowGameWinIE());
    }

    public void HintReward(){
        if(PlayerPrefs.GetInt("Music") == 0)
        {
            AudioManager.instance.TurnMusicOn();
        }
        else
        {
            AudioManager.instance.TurnMusicOff();
        }
        if (PlayerPrefs.GetInt("Sound") == 0)
        {
            AudioManager.instance.TurnSoundOn();
        }
        else
        {
            AudioManager.instance.TurnSoundOff();
        }
        GameController.instance.hint = true;
    }
    public void CancelHintReward(){
        if(PlayerPrefs.GetInt("Music") == 0)
        {
            AudioManager.instance.TurnMusicOn();
        }
        else
        {
            AudioManager.instance.TurnMusicOff();
        }
        if (PlayerPrefs.GetInt("Sound") == 0)
        {
            AudioManager.instance.TurnSoundOn();
        }
        else
        {
            AudioManager.instance.TurnSoundOff();
        }
    }

    public void SkipLevelReward(){
        if(PlayerPrefs.GetInt("Music") == 0)
        {
            AudioManager.instance.TurnMusicOn();
        }
        else
        {
            AudioManager.instance.TurnMusicOff();
        }
        if (PlayerPrefs.GetInt("Sound") == 0)
        {
            AudioManager.instance.TurnSoundOn();
        }
        else
        {
            AudioManager.instance.TurnSoundOff();
        }
        GameController.instance.skip = true;
        UIManager.instance.skipBtn.GetComponent<Button>().enabled = false;
        //GameController.instance.uiManager.ActiveClock();
        GameController.instance.currentState = GameController.STATE.FINISH;  
        UIManager.instance.ShowResult();
        UIManager.instance.startClock = false;
        UIManager.instance.clock.SetActive(false);
        UIManager.instance.beeDirection = false; 
             
    }
    public void CancelSkipLevelReward(){
        if(PlayerPrefs.GetInt("Music") == 0)
        {
            AudioManager.instance.TurnMusicOn();
        }
        else
        {
            AudioManager.instance.TurnMusicOff();
        }
        if (PlayerPrefs.GetInt("Sound") == 0)
        {
            AudioManager.instance.TurnSoundOn();
        }
        else
        {
            AudioManager.instance.TurnSoundOff();
        }    
    }
    public void DoubleCoinReward(){
        if(PlayerPrefs.GetInt("Music") == 0)
        {
            AudioManager.instance.TurnMusicOn();
        }
        else
        {
            AudioManager.instance.TurnMusicOff();
        }
        if (PlayerPrefs.GetInt("Sound") == 0)
        {
            AudioManager.instance.TurnSoundOn();
        }
        else
        {
            AudioManager.instance.TurnSoundOff();
        }
        PlayerData.instance.coins = Stars.instance.requiredCoins;
        PlayerPrefs.SetInt("coins",PlayerData.instance.coins);
        Stars.instance.dc = true;
        StartCoroutine(Stars.instance.InstantiateAndMoveCoins());        
    }
    public void CancelDoubleCoinReward(){
        if(PlayerPrefs.GetInt("Music") == 0)
        {
            AudioManager.instance.TurnMusicOn();
        }
        else
        {
            AudioManager.instance.TurnMusicOff();
        }
        if (PlayerPrefs.GetInt("Sound") == 0)
        {
            AudioManager.instance.TurnSoundOn();
        }
        else
        {
            AudioManager.instance.TurnSoundOff();
        }
    }
}
