using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class LoadDataControl : MonoBehaviour
{

	public static void LoadLanguage ()
	{
		LanguageManager.languageDict = new Dictionary<Language, LanguageDataModule> ();
		TextAsset[] all = Resources.LoadAll ("Language/").Cast<TextAsset> ().ToArray ();
		for (int i = 0; i < all.Length; i++) {
			string json = all [i].text;
			LanguageDataModule lgModule = JsonUtility.FromJson<LanguageDataModule> (json);
			Language lg = (Language)Enum.Parse (typeof(Language), all [i].name);
			LanguageManager.languageDict.Add (lg, lgModule);
		}
	}

	public static void LoadAllLevel (GameMode mode)
	{
		// load tất các level theo mode
		if (GameManager.LevelDataDict == null) {
			GameManager.LevelDataDict = new Dictionary<GameMode, Dictionary<int, LevelModule>> ();
		}
		if (!GameManager.LevelDataDict.ContainsKey (mode)) {
			Dictionary<int, LevelModule> l = new Dictionary<int, LevelModule> ();
			GameManager.LevelDataDict.Add (mode, l);
		}
		//
		string name = "LevelData/";
		switch (mode) {
		case GameMode.Normal: 
			name += "NormalMode/";
			break;
		case GameMode.Copy: 
			name += "CopyMode/";
			break;
		case GameMode.Double: 
			name += "DoubleMode/";
			break;
		}

		TextAsset[] all = Resources.LoadAll (name).Cast<TextAsset> ().ToArray ();
		for (int i = 0; i < all.Length; i++) {
			int lv;
			// lấy số level trong tên 
			string num = new String (all [i].name.Where (Char.IsDigit).ToArray ());
			lv = int.Parse (num);

			InitLevelData (mode, lv, all [i].text);
		}

	}

	public static void InitLevelData (GameMode mode, int level, string json)
	{
		LevelModule lv = new LevelModule ();
		GameManager.LevelDataDict [mode].Add (level, lv);
		GameManager.currenLevelModule = lv;
		JSONObject lvO = new JSONObject (json);
		// goal_line
		JSONObject goalLineO = lvO ["goal_line"];
		List<List<int>> goalLine = new List<List<int>> ();
		lv.goal_line = goalLine;
		for (int i = 0; i < goalLineO.Count; i++) {
			JSONObject lineO = goalLineO [i];
			List<int> line = new List<int> ();
			for (int j = 0; j < lineO.Count; j++) {
				line.Add (int.Parse (lineO [j].ToString ()));
			}
			goalLine.Add (line);
		}
		// init line 
		JSONObject initLineO = lvO ["init_line"];
		List<List<int>> initLine = new List<List<int>> ();
		lv.init_line = initLine;
		for (int i = 0; i < initLineO.Count; i++) {
			JSONObject lineO = initLineO [i];
			List<int> line = new List<int> ();
			for (int j = 0; j < lineO.Count; j++) {
				line.Add (int.Parse (lineO [j].ToString ()));
			}
			initLine.Add (line);
		}
		// Tips 
		JSONObject tipsO = lvO ["tips"];
		List<List<int>> tips = new List<List<int>> ();
		lv.tips = tips;
		for (int i = 0; i < tipsO.Count; i++) {
			JSONObject tipO = tipsO [i];
			List<int> tip = new List<int> ();
			for (int j = 0; j < tipO.Count; j++) {
				tip.Add (int.Parse (tipO [j].ToString ()));
			}
			tips.Add (tip);
		}
		//initline 2 
		JSONObject initLine2O = lvO ["init_line_2"];
		if (initLine2O != null) {
			List<List<int>> initLine2 = new List<List<int>> ();
			lv.init_line_2 = initLine2;
			for (int i = 0; i < initLine2O.Count; i++) {
				JSONObject line2O = initLine2O [i];
				List<int> line2 = new List<int> ();
				for (int j = 0; j < line2O.Count; j++) {
					line2.Add (int.Parse (line2O [j].ToString ()));
				}
				initLine2.Add (line2);
			}
		}
			
		/// // Tips 2
		JSONObject tips2O = lvO ["tips_2"];
		if (tips2O != null) {
			List<List<int>> tips2 = new List<List<int>> ();
			lv.tips_2 = tips2;
			for (int i = 0; i < tips2O.Count; i++) {
				JSONObject tip2O = tips2O [i];
				List<int> tip2 = new List<int> ();
				for (int j = 0; j < tip2O.Count; j++) {
					tip2.Add (int.Parse (tip2O [j].ToString ()));
				}
				tips2.Add (tip2);
			}
		}
	}

	// load riêng 1 level
	public static void LoadLevelData (GameMode gameMode, int level)
	{
		string name = "LevelData/";
		switch (gameMode) {
		case GameMode.Normal: 
			name += "NormalMode/";
			break;
		case GameMode.Copy: 
			name += "CopyMode/";
			break;
		case GameMode.Double: 
			name += "DoubleMode/";
			break;
		}

		// 



		name += "/Level_" + level.ToString ("00");

		TextAsset ta = Resources.Load (name) as TextAsset;
		LevelModule lv = new LevelModule ();
		GameManager.currenLevelModule = lv;
		JSONObject lvO = new JSONObject (ta.text);
		// goal_line
		JSONObject goalLineO = lvO ["goal_line"];
		List<List<int>> goalLine = new List<List<int>> ();
		lv.goal_line = goalLine;
		for (int i = 0; i < goalLineO.Count; i++) {
			JSONObject lineO = goalLineO [i];
			List<int> line = new List<int> ();
			for (int j = 0; j < lineO.Count; j++) {
				line.Add (int.Parse (lineO [j].ToString ()));
			}
			goalLine.Add (line);
		}
		// init line 
		JSONObject initLineO = lvO ["init_line"];
		List<List<int>> initLine = new List<List<int>> ();
		lv.init_line = initLine;
		for (int i = 0; i < initLineO.Count; i++) {
			JSONObject lineO = initLineO [i];
			List<int> line = new List<int> ();
			for (int j = 0; j < lineO.Count; j++) {
				line.Add (int.Parse (lineO [j].ToString ()));
			}
			initLine.Add (line);
		}

		Debug.Log ("Init line 0 : " + lv.init_line [0] [0]);
	}

}

[System.Serializable]
public class LevelModule
{
	public List<List<int>> goal_line;
	public List<List<int>> init_line;
	public List<List<int>> tips;
	public List<List<int>> init_line_2;
	public List<List<int>> tips_2;
}

[System.Serializable]
public class ListLineModule
{
	public int p1, p2, p3;
}

public enum GameMode
{
	Normal,
	Copy,
	Double
}

public enum DifficultGameMode
{
	easy,
	medium,
	hard
}