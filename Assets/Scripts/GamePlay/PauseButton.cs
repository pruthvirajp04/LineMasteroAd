using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseButton : MonoBehaviour
{
    public static PauseButton instance;

    private void Awake()
    {
        instance = this;
    }
    public void OnMouseDown()
	{
         
        if (GameManager.isFullyGameLoaded && GameManager.gameState == GameState.Playing && GameConfig.instance.tutorialControl.haveTutorial == false)
        {
            GameManager.gameState = GameState.Pause;
            PausePopup.instance.gameObject.SetActive(true);
            Debug.Log("Pause Event Works");
        }
        
       
    }
}
