using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanguageManager : MonoBehaviour
{
	public static LanguageManager instance;
	public static Dictionary<Language,LanguageDataModule> languageDict;
	// ngôn ngữ hiện tại
	LanguageDataModule currentLanguageModule;

	void Awake ()
	{
		instance = this;
	}
	

	void Start ()
	{
		currentLanguageModule = languageDict [GameManager.dataSave.language];
	}

	public static string GetText (LanguageKey key)
	{
		string k = key.ToString ();
		for (int i = 0; i < instance.currentLanguageModule.key.Count; i++) {
			string kCheck = instance.currentLanguageModule.key [i];
			if (kCheck == k) {
				return instance.currentLanguageModule.text [i];
			}
		}
		return "";
	}

}

public enum Language
{
	English,
	Bahasa
}

[System.Serializable]
public class LanguageDataModule
{
	public List<string> key, text;
}

public enum LanguageKey
{
	Continue,
	normal_step_1,
	normal_step_2,
	normal_step_3,
	normal_step_4,
	normal_step_5,
	back_help,
	retry_help,
	tips_help,
	copy_step_1,
	copy_step_2,
	double_step_1,
	double_step_2,
	double_step_3,
	mode_hint,
	double_tap,
	normal_mode,
	copy_mode,
	double_mode,
	easy_group,
	normal_group,
	hard_group,
	follow,
	evaluate,
	producer,
	member,
	music,
	music_name,
	copyright,
	unlock,
	spend_money,
	clear_level,
	video_ready,
	video_ready_2,
	watch,
	not_watch,
	reward_tips,
	level,
	succeed,
	earn_money,
	share,
	other_mode,
	ok,
	no,
	music_label,
	sounds_label,
	iap,
	ad,
	achievements,
	quit,
	hint_label,
	hint_label_sp
}