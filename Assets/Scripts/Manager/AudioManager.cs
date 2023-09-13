using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{

	public static AudioManager instance;
	Dictionary<AudioClipType,AudioClip> audioDic;
	public List<AudioStruct> listAudioStruct;

	public bool originalMusicStatus;
	public bool originalSoundStatus;


	void Awake ()
	{
		instance = this;
		audioDic = new Dictionary<AudioClipType, AudioClip> ();
		for (int i = 0; i < listAudioStruct.Count; i++) {
			if (!audioDic.ContainsKey (listAudioStruct [i].type)) {
				audioDic.Add (listAudioStruct [i].type, listAudioStruct [i].clip);
			}
		}
	}

	void Start ()
	{
		//PlayMusic (AudioClipType.AC_MAIN_MUSIC);
		//AS_SOUND.volume = GameManager.dataSave.isSoundOn == true ? 1F : 0;
		//AS_MUSIC.volume = GameManager.dataSave.isMusicOn == true ? 1F : 0;
	}

   

   

    public AudioSource AS_SOUND, AS_MUSIC;

	[System.Serializable]
	public struct AudioStruct
	{
		public AudioClipType type;
		public AudioClip clip;
	}


	public void StoreOriginalStatus()
	{
		originalMusicStatus = GetMusicStatus();
		originalSoundStatus = GetSoundStatus();
	}

    public void RestoreOriginalStatus()
    {
        SetMusicStatus(originalMusicStatus);
		SetSoundStatus(originalSoundStatus);
    }
    public bool GetSoundStatus ()
	{
		
		return GameManager.dataSave.isSoundOn;
	}

	public bool GetMusicStatus ()
	{	
		return GameManager.dataSave.isMusicOn;
	}

	public static void SetMusicStatus (bool isOn)
	{
		instance.AS_MUSIC.volume = isOn == true ? 0.5F : 0;
		GameManager.dataSave.isMusicOn = isOn;
		GameManager.SaveData ();

	}

	public static void SetSoundStatus (bool isOn)
	{
		instance.AS_SOUND.volume = isOn == true ? 1 : 0;
		GameManager.dataSave.isSoundOn = isOn;
		GameManager.SaveData ();
	}

	public static void ChangeSoundStatus ()
	{
		
		GameManager.dataSave.isSoundOn = !GameManager.dataSave.isSoundOn;
		SetSoundStatus (GameManager.dataSave.isSoundOn);

	}

	public static void ChangeMusicStatus ()
	{
		GameManager.dataSave.isMusicOn = !GameManager.dataSave.isMusicOn;
		SetMusicStatus (GameManager.dataSave.isMusicOn);


	}

    public void PauseMusic()
    {
        if (AS_MUSIC.isPlaying)
        {
            AS_MUSIC.Stop();
        }
    }

    public void ResumeMusic()
    {
        if (!AS_MUSIC.isPlaying && GetMusicStatus())
        {
            AS_MUSIC.Play();
        }
    }

    public static void PlaySound (AudioClipType type)
	{
		instance.AS_SOUND.PlayOneShot (instance.audioDic [type]);
	}

	public static void PlayMusic (AudioClipType type)
	{
		instance.AS_MUSIC.Stop ();
		instance.AS_MUSIC.clip = instance.audioDic [type];
		instance.AS_MUSIC.Play ();
	}

	public void PlaySoundTime (AudioClipType type, float startTime, float stopTime)
	{

		/* Create a new audio clip */
		AudioClip clip = audioDic [type]; 
		int frequency = clip.frequency;
		float timeLength = stopTime - startTime;
		int samplesLength = (int)(frequency * timeLength);
		AudioClip newClip = AudioClip.Create (clip.name + "-sub", samplesLength, 1, frequency, false);
		/* Create a temporary buffer for the samples */
		float[] data = new float[samplesLength];
		/* Get the data from the original clip */
		clip.GetData (data, (int)(frequency * startTime));
		/* Transfer the data to the new clip */
		newClip.SetData (data, 0);
		/* Return the sub clip */
		AS_SOUND.PlayOneShot (newClip);
		//return newClip;

	}
}


[System.Serializable]
public enum AudioClipType
{
	AC_BGM_GAME_1,
	AC_BGM_GAME_2,
	AC_BGM_MENU,
	AC_BUTTON,
	AC_CHANGE_LINE,
	AC_FINISH,
	AC_GET_COIN,
	AC_SMALL_BOX,
	AC_TOUCHEND_CORRECT,
	AC_UNLOCK
}