using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
	Image img;
	public Text tLevel;
	int _level = 0;
	// iRing  = vòng tròn xung quanh nút
	// iWhite = nền của iCurrentLevel
	// iCurrentLevel = icon chỉ thị là level đang chơi đến
	public Image iShadow, iButton, iRing, iCurrentLevel, iWhite;

	void Awake ()
	{
		img = GetComponent<Image> ();
	}

	public void SetColor ()
	{
		iRing.color = GameConfig.mainColor;
		iCurrentLevel.color = GameConfig.mainColor;
	}



	public int level {
		get {
			return _level;
		}
		set {
			_level = value;
			Init ();
		}
	}

	public void Init ()
	{
		tLevel.text = level.ToString ();
		int lvMax = GameManager.lvMax;
		if (level < lvMax) {
			//set level đã chơi qua 
			iCurrentLevel.enabled = false;
			iWhite.enabled = false;
			iShadow.enabled = true;
			iButton.color = Color.white;
		} else if (level == lvMax) {
			//set level đang chơi đến
			iCurrentLevel.enabled = true;
			iWhite.enabled = true;
			iShadow.enabled = true;
			iButton.color = Color.white;
		} else {
			//set level chưa chơi đến
			iCurrentLevel.enabled = false;
			iWhite.enabled = false;
			iShadow.enabled = false;
			iButton.color = new Color (0.8F, 0.8F, 0.8F, 1);
			iRing.color = Color.white;
		}
	}

	public void OnClick ()
	{
		AudioManager.PlaySound (AudioClipType.AC_BUTTON);
		if (level <= GameManager.lvMax) {
			GameManager.currentLevel = level;
			GameManager.gameState = GameState.Playing;
		}

	}
}
