using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class WinPopupControl : MonoBehaviour
{

	public Text tLevel, tCompleted, tNext, tShare,  tCoinAddValue;
	//string text = "Coins can be obtained for the first time";
	public Image iCoinIcon;

	void OnEnable ()
	{
		if(GameManager.currentLevel % 3 == 0)
		{
			GlanceAds.ReplayAd("ReplayOnLevel");
		}
		transform.position = new Vector3 (0, 0, 0);
		transform.localScale = new Vector3 (0, 0, 0);
		transform.DOScale (new Vector3 (1, 1, 1), 1F).SetEase (Ease.OutElastic);
		tLevel.text = GameManager.currentLevel.ToString ();
		tCompleted.text = LanguageManager.GetText (LanguageKey.succeed);
		tNext.text = LanguageManager.GetText (LanguageKey.Continue);
		//tShare.text = LanguageManager.GetText (LanguageKey.share);

		GameManager.currentLevel++;
		if (GameManager.currentLevel > GameManager.lvMax) 
		{
			// nếu là lần đầu thằng level này thì hiện phần thêm coin
			GameManager.lvMax = GameManager.currentLevel;			
			tCoinAddValue.enabled = true;
			iCoinIcon.enabled = true;
			tCompleted.rectTransform.anchoredPosition = new Vector2 (0, -17);
			

			// add 3 coin 
			AudioManager.PlaySound (AudioClipType.AC_GET_COIN);
			GameManager.dataSave.coinCount += 10;
			GameManager.SaveData ();
		} 
		else 
		{
			tCompleted.rectTransform.anchoredPosition = new Vector2 (0, -46);
			iCoinIcon.enabled = false;
			tCoinAddValue.enabled = false;
		}
        //StartCoroutine(ShowAds());
	}

	public void Next ()
	{
		AudioManager.PlaySound (AudioClipType.AC_BUTTON);
		GameControl.instance.blackLayer.SetActive (false);
		PopupManager.instance.HidePopup (PopupName.Win);
		if (GameManager.LevelDataDict [GameManager.currentGameMode].ContainsKey (GameManager.currentLevel)) {
			GameManager.gameState = GameState.Playing;
			GameControl.instance.NextLevel ();
		} else {
			// nếu đạt đến level cuối cùng thì quay về menu
			GameManager.gameState = GameState.SelectLevel;
		}
	}

	public void Share ()
	{
		Debug.Log ("Share");
	}

    IEnumerator ShowAds()
    {
        yield return new WaitForSeconds(1.0f);
        AdsControl.Instance.showAds();
    }
}
