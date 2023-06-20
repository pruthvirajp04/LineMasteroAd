using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
// module lưu trữ data 
public class DataSaveModule
{
	// level tối đa đã đạt tới chế độ normal
	public int normalLevelMax = 1;
	// level tối đa đã đạt tới chế độ copy
	public int copyLevelMax = 1;
	// level tối đa đã đạt tới chế độ double
	public int doubleLevelMax = 1;
	// số lượng hint đang có
	public int hintCount = 0;
	// số coin đang có
	public int coinCount = 0;
	// ngôn ngữ hiện tại
	public Language language = Language.English;
	// bật , tắt âm thanh,nhạc nền
    public bool isSoundOn = true, isMusicOn = true;
}
