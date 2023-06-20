using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
using System.IO;

public class CreateData : Editor
{
	// tool tách data gốc thành các file data theo từng level , thuận tiện sau này thêm ,xóa ,sửa hoặc làm level editor
	[MenuItem ("Data/Create Level Data")]
	public static void CreateLevelData ()
	{
		
		TextAsset dataAsset = Resources.Load ("Data") as TextAsset;

		JSONObject jObj = new JSONObject (dataAsset.text);

		JSONObject normalModeObj = jObj ["copy_mode"];
		int level = 1;
		for (int d = 0; d < normalModeObj.Count; d++) {
			JSONObject difficultObj = normalModeObj [d];

			for (int g = 0; g < difficultObj.Count; g++) {
				JSONObject groupObj = difficultObj [g];
				for (int lv = 0; lv < groupObj.Count; lv++) {
					JSONObject lvObj = groupObj [lv];
					CreateLevel (GameMode.Copy, level, lvObj.ToString ());
					level++;
				}
			}
		}
		AssetDatabase.Refresh ();
	}

	static string directory = "/Resources/LevelData/";

	public static void CreateLevel (GameMode mode, int level, string data)
	{
		string smode = "";
		switch (mode) {
		case GameMode.Normal: 
			smode = "NormalMode";
			break;
		case GameMode.Copy: 
			smode = "CopyMode";
			break;
		case GameMode.Double: 
			smode = "DoubleMode";
			break;
		}
		string dir = directory + smode;
		string fileName = "Level_" + level.ToString () + ".txt";
		if (!AssetDatabase.IsValidFolder ("Assets" + dir)) {
			Debug.Log ("Check folder : " + "Assets" + dir + " ----- tao folder : " + "Assets/Resources/LevelData/" + smode);
			AssetDatabase.CreateFolder ("Assets/Resources/LevelData", smode);
			File.WriteAllText (Application.dataPath + dir + "/" + fileName, data);

		} else {
			File.WriteAllText (Application.dataPath + dir + "/" + fileName, data);

		}
	}

}
#endif