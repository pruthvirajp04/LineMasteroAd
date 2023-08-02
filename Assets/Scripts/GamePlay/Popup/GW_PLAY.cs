using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GW_PLAY : MonoBehaviour
{

	void OnEnable ()
	{
        // tạo 1 level chơi mới 
        GameControl.instance.StartNewLevel ();
	}
}
