using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using UnityEngine.SceneManagement;
using TMPro;


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
   
    //Work on Ads first and add analytics later. 
    void Awake()
    {
        LoadGlanceAds();
    }

    public void deleteGlanceKey(){
        PlayerPrefs.DeleteKey("firstGlance");
    }
    public void muteAudio(){
        //Mute All Audios Here
        //Did it for reference, so you can create logic for other purposes.
       AudioManager.SetMusicStatus(false);
        AudioManager.SetSoundStatus(false);
    }
    public void doneReplay(){
       //You will show a replay ad when the restart button is clicked(You need to find the function for the restart button in other C# files). And once they are done, in the callback of that ad you will call this function to resume music/sound here based on the initial state of music/sound.
       //Check if the audio was already muted or unmuted before the ad was shown and mute/unmute audio based on that here..
         bool musicState = GameManager.instance.GetMusicState();
        bool soundState = GameManager.instance.GetSoundState();

        AudioManager.SetMusicStatus(musicState);
        AudioManager.SetSoundStatus(soundState);
    }
    public void doneReplay1w(){
        //You will show a replay ad when the win screen is shown(You need to find the function for it in other C# files). And once they are done, in the callback of that ad you will call this function to resume music/sound here based on the initial state of music/sound and show the win screen.
        //Check if the audio was already muted or unmuted before the ad was shown and mute/unmute audio based on that here..
            bool musicState = GameManager.instance.GetMusicState();
        bool soundState = GameManager.instance.GetSoundState();

        AudioManager.SetMusicStatus(musicState);
        AudioManager.SetSoundStatus(soundState);

    }

    public void HintReward(){
        //You will show a rewarded ad when the hint button is clicked(You need to find the function for it in other C# files). And only if the user has watched the ad fully i.e success case (check glance documentation to know more about success and failure cases [https://glanceinmobi.atlassian.net/wiki/spaces/GSC/pages/815628492/Rewarded+page]), in the callback of that ad you will call this function to resume music/sound here based on the initial state of music/sound and show the hint.
        //Check if the audio was already muted or unmuted before the ad was shown and mute/unmute audio based on that here..
        //Call the function that shows the win screen. You need to find this in C# files.

           bool musicState = GameManager.instance.GetMusicState();
        bool soundState = GameManager.instance.GetSoundState();

        AudioManager.SetMusicStatus(musicState);
        AudioManager.SetSoundStatus(soundState);


    }
    public void CancelHintReward(){
        //You will show a rewarded ad when the hint button is clicked(You need to find the function for it in other C# files). And only if the user hasn't watched the ad fully i.e failure case (check glance documentation to know more about success and failure cases [https://glanceinmobi.atlassian.net/wiki/spaces/GSC/pages/815628492/Rewarded+page]), in the callback of that ad you will call this function to only resume music/sound here based on the initial state of music/sound.
        //Check if the audio was already muted or unmuted before the ad was shown and mute/unmute audio based on that here..
        //That's it, no need to show hints.
        bool musicState = GameManager.instance.GetMusicState();
        bool soundState = GameManager.instance.GetSoundState();

        AudioManager.SetMusicStatus(musicState);
        AudioManager.SetSoundStatus(soundState)

    }
}
