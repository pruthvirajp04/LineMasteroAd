using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopControl : MonoBehaviour
{
	public Button bBuyBy100Coin, bWatchVideo;
	public Text tDescription;
	Color fullCol = new Color (1, 1, 1, 1);
	Color midCol = new Color (1, 1, 1, 0.5F);
	int buy3HintPrice = 100;

	void OnEnable ()
	{
		tDescription.text = LanguageManager.GetText (LanguageKey.hint_label);
		GameCanvas.instance.blackLayer.gameObject.SetActive (true);
		// làm mờ nút mua bằng coin nếu không đủ coin
		if (GameManager.dataSave.coinCount >= buy3HintPrice) {
			bBuyBy100Coin.image.color = fullCol;
		} else {
			bBuyBy100Coin.image.color = midCol;
		}
	}

	void OnDisable ()
	{
		GameCanvas.instance.blackLayer.gameObject.SetActive (false);
	}

	public void BuyByCoin ()
	{
		AudioManager.PlaySound (AudioClipType.AC_BUTTON);
		if (GameManager.dataSave.coinCount >= buy3HintPrice) {
			GameManager.dataSave.coinCount -= buy3HintPrice;
			GameManager.dataSave.hintCount += 1;
			GameManager.gameState = GameState.Playing;
			GameManager.SaveData ();
		}

	}

	public void ShowVideo ()
	{
		
		AudioManager.PlaySound (AudioClipType.AC_BUTTON);

        AudioManager.instance.StoreOriginalStatus();
        AudioManager.SetMusicStatus(false);
        AudioManager.SetSoundStatus(false);

        GlanceAds.RewardedAdsAnalytics("HintReward", "CancelHintReward");
        GlanceAds.RewardedAd("Hint");
        

    }

	public static void OnVideoReward ()
	{
		GameManager.dataSave.hintCount += 1;
		GameManager.gameState = GameState.Playing;
		GameManager.SaveData ();
	}

	public void Close ()
	{
		GameManager.gameState = GameState.Playing;
	}
}
