using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

// control một số thành phần UI trong object gameCanvas
public class GameCanvas : MonoBehaviour
{
	public static GameCanvas instance;
	// layer màu đen alpha
	public Image blackLayer;
	public CanvasGroup canvasGroup;

	void Awake ()
	{
		instance = this;
	}
	// UI hiển thị số coin , hint đang có
	public Image coinGroup, hintGroup;
	public Text tCoinValue, tHintValue;
	// coinTemp và hintTemp để check sự thay đổi coin,hint qua đó thay đổi text hiển thị
	public int coinTemp, hintTemp;
	//vị trí lúc bắt đầu của coinroup và hintGroup ,  thuận tiện làm animation
	public Vector3 startCoinGroupPos;
	public Vector3 startHintGroupPos;

	void Start ()
	{
		// set các giá trị ban đầu
		coinTemp = GameManager.dataSave.coinCount;
		hintTemp = GameManager.dataSave.hintCount;
		tHintValue.text = hintTemp.ToString ();
		tCoinValue.text = coinTemp.ToString ();
		startCoinGroupPos = coinGroup.transform.position;
		startHintGroupPos = hintGroup.transform.position;
	}

	void Update ()
	{
		// check thay đổi của coin và hint , nếu có thì thay đổi text hiển thị
		if (coinTemp != GameManager.dataSave.coinCount) {
			coinTemp = GameManager.dataSave.coinCount;
			tCoinValue.text = coinTemp.ToString ();
		}
		if (hintTemp != GameManager.dataSave.hintCount) {
			hintTemp = GameManager.dataSave.hintCount;
			tHintValue.text = hintTemp.ToString ();
		}
	}

}
