using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuControl : MonoBehaviour
{
	// gameMode là giá trị int của enum GameMode  , 0 = normal ,1 = copy , 2 = 2 color
	public void GotoSelectLevel (int gameMode)
	{
		AudioManager.PlaySound (AudioClipType.AC_BUTTON);
		// set current gamemode
		GameManager.currentGameMode = 0;
		//
		GameManager.gameState = GameState.SelectLevel;
	}
}
