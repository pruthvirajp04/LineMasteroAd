using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameConfig : MonoBehaviour
{
	// config một số data của game
	public static GameConfig instance;

	void Awake ()
	{
		instance = this;
	}

	public TutorialControl tutorialControl;
	// color chủ đạo
	public static Color mainColor = Color.blue;
	// color cho line2 dùng trong chế độ 2color or copy
	public static Color mainColor2 = Color.green;
	// list màu
	public List<Color> listLineColors;
	// list các background
	public List<Sprite> listBlurBackground;
	public List<Sprite> listBackground;
}
