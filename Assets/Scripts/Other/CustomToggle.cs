using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// toggle tự tạo theo game
public class CustomToggle : MonoBehaviour
{
	bool _value;
	public GameObject ON, OFF, Bahasa_ON, Bahasa_OFF, German_ON, German_OFF;

	void Awake ()
	{
		
		Button bt = GetComponent<Button> ();
		bt.onClick.AddListener (() => 
		{
			value = !value;
			
		});
	}

	public bool value {
		get { return _value; }
		set {
			_value = value;

			UpdateToggleState();

			//ON.SetActive (value ? true : false);
			//OFF.SetActive (value ? false : true);

		}

	}

	void UpdateToggleState()
	{
		bool isOn = _value;
		string lang = PlayerPrefs.GetString("LanguageChar");

		if(lang == "en")
		{
			ON.SetActive (isOn);
			OFF.SetActive (!isOn);
			Bahasa_ON.SetActive(false);
			Bahasa_OFF.SetActive (false);
			German_OFF.SetActive (false);
			German_ON.SetActive (false);
		}
		else if(lang == "id")
		{
			Bahasa_ON.SetActive(isOn);
			Bahasa_OFF.SetActive(!isOn);
			ON.SetActive (false);
			OFF.SetActive (false);
            German_OFF.SetActive(false);
            German_ON.SetActive(false);
        }
		else if(lang == "de")
		{
            German_ON.SetActive(isOn);
            German_OFF.SetActive(!isOn);
            Bahasa_ON.SetActive(false);
            Bahasa_OFF.SetActive(false);
            ON.SetActive(false);
            OFF.SetActive(false);

        }

	}




}
