using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// toggle tự tạo theo game
public class CustomToggle : MonoBehaviour
{
	bool _value;
	public GameObject ON, OFF;

	void Awake ()
	{
		
		Button bt = GetComponent<Button> ();
		bt.onClick.AddListener (() => {
			value = !value;
		});
	}

	public bool value {
		get { return _value; }
		set {
			_value = value;

			ON.SetActive (value ? true : false);
			OFF.SetActive (value ? false : true);

		}

	}




}
