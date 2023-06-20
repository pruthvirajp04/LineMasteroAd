using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PopupManager : MonoBehaviour
{
	public List<PopupStruct> listPopup;
	public static PopupManager instance;

	void Awake ()
	{
		instance = this;
		for (int i = 0; i < listPopup.Count; i++) {
			Vector3 pos = listPopup [i].popupObject.transform.position;
			listPopup [i].startPos = pos;
		}
	}

	[System.Serializable]
	public class PopupStruct
	{
		public PopupName name;
		public GameObject popupObject;
		public Vector3 startPos;
	}

	public PopupName showingPopup;



	PopupStruct GetPopupStruct (PopupName name)
	{
		for (int i = 0; i < listPopup.Count; i++) {
			if (listPopup [i].name == name) {
				return listPopup [i];
			}
		}
		return null;
	}

	public void ShowPopup (PopupName name, bool closeOtherPopup = true, bool transitionEffectOld = false, bool transitionEffectNew = false, bool showCoin = true)
	{
		PopupStruct oldPopup = GetPopupStruct (showingPopup);
		PopupStruct popupObj = GetPopupStruct (name);

		if (transitionEffectOld) {
			if (closeOtherPopup) {
				if (oldPopup != null) {
					oldPopup.popupObject.transform.DOMoveY (20, 0.4F).SetEase (Ease.InBack).OnComplete (() => {
						oldPopup.popupObject.SetActive (false);
						if (name == PopupName.SelectLevel) {
							GetPopupStruct (PopupName.GamePlay).popupObject.SetActive (false);
							GetPopupStruct (PopupName.Win).popupObject.SetActive (false);
						}
						// hiện luôn popup mới nếu ko có hiệu ứng hiện popup mới
						if (!transitionEffectNew) {
							popupObj.popupObject.SetActive (true);
						}
					});
				}

			}
		}
		//------
		if (transitionEffectNew) {		
			popupObj.popupObject.SetActive (true);
			popupObj.popupObject.transform.position = popupObj.startPos + new Vector3 (0, -40, 0);
			popupObj.popupObject.transform.DOMoveY (popupObj.startPos.y, 0.4F).SetEase (Ease.OutBack).SetDelay (0.25F);

		}
		//-----------
		if (!transitionEffectNew && !transitionEffectOld) {
			if (closeOtherPopup) {
				if (oldPopup != null)
					oldPopup.popupObject.SetActive (false);
				
			}
			popupObj.popupObject.SetActive (true);

		}

		// di chuyển coinGroup,hintGroup theo hiệu ứng và điều kiện 
		if (showCoin) {
			GameCanvas.instance.coinGroup.transform.DOMoveX (12, 0.4F).OnComplete (() => {				
				GameCanvas.instance.coinGroup.transform.DOMoveX (GameCanvas.instance.startCoinGroupPos.x, 0.4F);
			});
			GameCanvas.instance.hintGroup.transform.DOMoveX (-12, 0.4F).OnComplete (() => {				
				GameCanvas.instance.hintGroup.transform.DOMoveX (GameCanvas.instance.startHintGroupPos.x, 0.4F);
			});
		} else {
			if (transitionEffectOld && oldPopup != null) {				
				GameCanvas.instance.coinGroup.transform.position = GameCanvas.instance.startCoinGroupPos;
				GameCanvas.instance.coinGroup.transform.DOMoveX (12, 0.4F);
				GameCanvas.instance.hintGroup.transform.position = GameCanvas.instance.startHintGroupPos;
				GameCanvas.instance.hintGroup.transform.DOMoveX (-12, 0.4F);
			} else {
				
				GameCanvas.instance.coinGroup.transform.DOMoveX (12, 0F);
				GameCanvas.instance.hintGroup.transform.DOMoveX (-12, 0F);
				//GameCanvas.instance.coinGroup.gameObject.SetActive (false);
				//GameCanvas.instance.coinGroup.transform.position = new Vector3 (1200, GameCanvas.instance.coinGroup.transform.position.y, 0);
			}
		}
			
		showingPopup = name;
	}

	public void HidePopup (PopupName name)
	{
		for (int i = 0; i < listPopup.Count; i++) {
			if (listPopup [i].name == name) {
				listPopup [i].popupObject.SetActive (false);
				return;
			}
		}
	}
}

[System.Serializable]
public enum PopupName
{
	Noone,
	Menu,
	SelectLevel,
	GamePlay,
	Win,
	Pause,
	Shop
}
