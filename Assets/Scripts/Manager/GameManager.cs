using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
	public static GameManager instance;

	void Awake ()
	{
       
		instance = this;
		LoadDataSave ();

	}

	void Start ()
	{

        if (PlayerPrefs.GetInt("First") == 0)
        {
            dataSave.isMusicOn = true;
            dataSave.isSoundOn = true;
            GameManager.dataSave.hintCount = 3;
            GameManager.SaveData();
            PlayerPrefs.SetInt("First",1);
        }
		AudioManager.SetMusicStatus (dataSave.isMusicOn);
		AudioManager.SetSoundStatus (dataSave.isSoundOn);


	}

	public static Language currentLanguage = Language.English;
	public static DataSaveModule dataSave;
	public static int currentLevel;
	public static GameMode currentGameMode;
	public static LevelModule currenLevelModule;
	GameState _gameState;
	// gameState + các thay đổi khi đổi gameState
	public static GameState gameState {
		get 
		{ return instance._gameState; }
		set {
			
			switch (value) {
			case GameState.SelectLevel: 
				BackgroundControl.instance.ChangeBackground (true, 0.4F);
				// hiện popup chọn level
				PopupManager.instance.ShowPopup (PopupName.SelectLevel, true, true, true);
				break;
			case GameState.Playing:				
				BackgroundControl.instance.ChangeBackground (false, 1F);
				// hiệu ứng mờ dần để chuyển sang màn chơi
				float alpha = 1;
				DOTween.To (() => alpha, x => alpha = x, 0, 1F).OnUpdate (() => {					
					GameCanvas.instance.canvasGroup.alpha = alpha;
				}).OnComplete (() => {
					GameCanvas.instance.coinGroup.transform.position = new Vector3 (12, GameCanvas.instance.coinGroup.transform.position.y, 0);
					GameCanvas.instance.hintGroup.transform.position = new Vector3 (-12, GameCanvas.instance.hintGroup.transform.position.y, 0);
					PopupManager.instance.ShowPopup (PopupName.GamePlay, true, false, false, false);
					GameCanvas.instance.canvasGroup.alpha = 1;
				});


				break;
			case GameState.Win: 
				PopupManager.instance.ShowPopup (PopupName.Win, false, false, false, false);
				break;
			case GameState.Pause: 
				PopupManager.instance.ShowPopup (PopupName.Pause, false, false, true, false);
				break;
			case GameState.Shopping: 
				PopupManager.instance.ShowPopup (PopupName.Shop, false, false, true, false);
				break;
			}
			instance._gameState = value;

		}
	}

	// level tối đa đã đạt được trong chế độ chơi đang chọn
	public static int lvMax {
		get {
			int _lvMax = currentGameMode == GameMode.Normal ? dataSave.normalLevelMax : 
				currentGameMode == GameMode.Copy ? dataSave.copyLevelMax : dataSave.doubleLevelMax;
			return _lvMax;
		}
		set {
			//---------------
			switch (currentGameMode) {
			case GameMode.Normal:
				dataSave.normalLevelMax = value;
				break;
			case GameMode.Copy:
				dataSave.copyLevelMax = value;
				break;
			case GameMode.Double:
				dataSave.doubleLevelMax = value;
				break;
			}
			SaveData ();
		}

	}

	public static Dictionary<GameMode,Dictionary<int,LevelModule>> LevelDataDict;

	// load data người chơi
	public static void LoadDataSave ()
	{
		string json = PlayerPrefs.GetString ("Data", "");
		if (json != "") {
			dataSave = JsonUtility.FromJson<DataSaveModule> (json);
		} else {
			dataSave = new DataSaveModule ();
		}
	}

	// lưu data người chơi

	public static void SaveData ()
	{
		string json = JsonUtility.ToJson (dataSave);
		PlayerPrefs.SetString ("Data", json);
		PlayerPrefs.Save ();
	}

	//
	public static void SetNativeSize (Image img, float widthSize = -1, float heightSize = -1)
	{
		img.SetNativeSize ();
		float k = img.rectTransform.rect.width / img.rectTransform.rect.height;
		if (widthSize > -1 && heightSize == -1) {
			img.rectTransform.sizeDelta = new Vector2 (widthSize, widthSize / k);
		} else if (widthSize == -1 && heightSize > -1) {
			img.rectTransform.sizeDelta = new Vector2 (heightSize * k, heightSize);
		} else if (widthSize > -1 && heightSize > -1) {
			img.rectTransform.sizeDelta = new Vector2 (widthSize, heightSize);
		}
	}

	public static DateTime CheckTime;

	// dung de check thoi gian thuc thi 1 doan code

	#region Check thoi gian thuc thi 1 doan code

	public static void StartCheckTime ()
	{
		CheckTime = DateTime.Now;
	}

	public static void EndCheckTime (string msg)
	{
		Debug.Log (msg + ": " + (DateTime.Now - CheckTime).TotalMilliseconds + "ms");
	}

	#endregion

	// dùng để check xem có lick vào UI trên canvas hay không
	public static bool IsClickOnUI {
		get {
			#if UNITY_EDITOR
			if (EventSystem.current.IsPointerOverGameObject ()) {
				return true;
			} else
				return false;
			#else
			if (Input.touchCount > 0 && EventSystem.current.IsPointerOverGameObject (Input.GetTouch(0).fingerId)) {

			return true;
			}else return false;
			#endif
		}
	}

}

public enum GameState
{
	SelectLevel,
	Playing,
	Win,
	Pause,
	Shopping
}