using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using UnityEngine.SceneManagement;
using TMPro;


public class GlanceAds : MonoBehaviour
{
    [SerializeField] Transform popupParent;

    public static GlanceAds instance;

    [DllImport("__Internal")]
    public static extern void LoadGlanceAds();

    [DllImport("__Internal")]
    public static extern void RewardedAd(string type);

    [DllImport("__Internal")]
    public static extern void ReplayAd(string type);

    [DllImport("__Internal")]
    public static extern void LoadAnalytics();

    [DllImport("__Internal")]
    public static extern void StartAnalytics();

    [DllImport("__Internal")]
    public static extern void ReplayAnalytics(int level);

    [DllImport("__Internal")]
    public static extern void EndAnalytics(int level);

    [DllImport("__Internal")]
    public static extern void LevelAnalytics(int level);

    [DllImport("__Internal")]
    public static extern void LevelCompletedAnalytics(int level);

    [DllImport("__Internal")]
    public static extern void RewardedAdsAnalytics(string successCBf, string failureCBf);

    [DllImport("__Internal")]
    public static extern void MilestoneAnalytics(int collectedStars, int level);

    [DllImport("__Internal")]
    public static extern void GameLifeEndAnalytics(int RemainingLife, int level);

    [DllImport("__Internal")]
    public static extern void InGameAnalytics(string items, int amount, int level);

    private GameControl gameControl;
    
    void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
        gameControl = FindObjectOfType<GameControl>();
    }


    public void deleteGlanceKey(){
        PlayerPrefs.DeleteKey("firstGlance");
    }
    public void muteAudio()
    {
        AudioManager.SetMusicStatus(false);
        AudioManager.SetSoundStatus(false);
    }
    public void setLanguage(string LanguageChar)
    {
        PlayerPrefs.DeleteKey("LanguageChar");
        PlayerPrefs.SetString("LanguageChar", LanguageChar);
    }

    public void setAd(string Adtype)
    {
        PlayerPrefs.SetInt(Adtype, 1);
    }

    public void pauseEvent()
    {
        //TODO: logic to pause the game
        if(GameManager.gameState == GameState.Playing && GameConfig.instance.tutorialControl.haveTutorial == false)
        {
            GameManager.gameState = GameState.Pause;
            Debug.Log("Pause Event Works");
        }
        
    }
    
    public void resumeEvent()
    {
        //TODO: logic to resume the game
        if(GameManager.gameState == GameState.Pause)
        {
            GameManager.gameState = GameState.Playing;
            Debug.Log("Resume Event works");
        }
    }

    public void replayGameEvent()
    {
        //TODO: Logic to replay the game Level
        if(gameControl!=null)
        {
            gameControl.RestartLevel();
            Debug.Log("Replay Game Event works");
        }
    }

    public void nextLevelEvent()
    {
        //TODO: Logic to go to the next level
        gameControl.NextLevel();
    }

    public void gotoHomeEvent()
    {
        //TODO: Logic to go to home screen
        PausePopup pausePopup = FindObjectOfType<PausePopup>();
        if (pausePopup != null)
        {
            pausePopup.Menu();
            Debug.Log("Go to home event works");
        }
    }

    public void gotoLevel(int levelNo)
    {
        //TODO: Logic to go to a specific level
        GameManager.currentLevel = levelNo;
        GameManager.gameState = GameState.Playing;
    }
    public void DoneReplay(){
       //You will show a replay ad when the restart button is clicked(You need to find the function for the restart button in other C# files). And once they are done, in the callback of that ad you will call this function to resume music/sound here based on the initial state of music/sound.
       //Check if the audio was already muted or unmuted before the ad was shown and mute/unmute audio based on that here..

        AudioManager.SetMusicStatus(AudioManager.instance.GetMusicStatus());
        AudioManager.SetSoundStatus(AudioManager.instance.GetSoundStatus());  
    }
    public void DoneReplayWin(){
        //You will show a replay ad when the win screen is shown(You need to find the function for it in other C# files). And once they are done, in the callback of that ad you will call this function to resume music/sound here based on the initial state of music/sound and show the win screen.
        //Check if the audio was already muted or unmuted before the ad was shown and mute/unmute audio based on that here..

        AudioManager.SetMusicStatus(AudioManager.instance.GetMusicStatus());
        AudioManager.SetSoundStatus(AudioManager.instance.GetSoundStatus());
    }

    public void HintReward(){
        //You will show a rewarded ad when the hint button is clicked(You need to find the function for it in other C# files). And only if the user has watched the ad fully i.e success case (check glance documentation to know more about success and failure cases [https://glanceinmobi.atlassian.net/wiki/spaces/GSC/pages/815628492/Rewarded+page]), in the callback of that ad you will call this function to resume music/sound here based on the initial state of music/sound and show the hint.
        //Check if the audio was already muted or unmuted before the ad was shown and mute/unmute audio based on that here..
        //Call the function that shows the win screen. You need to find this in C# files.

        AudioManager.SetMusicStatus(AudioManager.instance.GetMusicStatus());
        AudioManager.SetSoundStatus(AudioManager.instance.GetSoundStatus());
        //GameControl.instance.bHint = true;

    }
    public void CancelHintReward(){
        //You will show a rewarded ad when the hint button is clicked(You need to find the function for it in other C# files). And only if the user hasn't watched the ad fully i.e failure case (check glance documentation to know more about success and failure cases [https://glanceinmobi.atlassian.net/wiki/spaces/GSC/pages/815628492/Rewarded+page]), in the callback of that ad you will call this function to only resume music/sound here based on the initial state of music/sound.
        //Check if the audio was already muted or unmuted before the ad was shown and mute/unmute audio based on that here..
        //That's it, no need to show hints.
        
        AudioManager.SetMusicStatus(AudioManager.instance.GetMusicStatus());
        AudioManager.SetSoundStatus(AudioManager.instance.GetSoundStatus());
    }
}
