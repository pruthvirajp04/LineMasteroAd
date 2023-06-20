using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseButton : MonoBehaviour
{

	void OnMouseDown ()
	{
		if (GameManager.gameState == GameState.Playing && GameConfig.instance.tutorialControl.haveTutorial == false) {
			AudioManager.PlaySound (AudioClipType.AC_BUTTON);
			GameManager.gameState = GameState.Pause;
		}
	}
}
