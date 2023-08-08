using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SelectLevelControl : MonoBehaviour
{

	public GameObject levelGroup;
	public List<LevelButton> listLevelButton;
	int page = 1;
	int maxPage;

	void OnEnable ()
	{
		// mỗi khi mở trang chọn level thì sẽ lấy ngẫu nhiên mainColor
		//GameConfig.mainColor = GameConfig.instance.listLineColors [Random.Range (0, GameConfig.instance.listLineColors.Count)];
	
		page = GameManager.lvMax / 12 + 1;
		InitLevel ();
	}

	void Start ()
	{
		
		// tính số trang tối đa 
		maxPage = GameManager.LevelDataDict [GameManager.currentGameMode].Count / 12 + 1;
		InitLevel ();
	}

	public void InitLevel ()
	{
		for (int i = 0; i < listLevelButton.Count; i++) {
			// set màu cho nút theo mainColor vừa set
			listLevelButton [i].SetColor ();
			//
			int lv = 12 * (page - 1) + (i + 1);
			//set level cho các button , nếu ko tồn tại level đó trong data thì không hiện button tương ứng
			if (GameManager.LevelDataDict [GameManager.currentGameMode].ContainsKey (lv)) {
				if (!listLevelButton [i].gameObject.activeInHierarchy) {
					listLevelButton [i].gameObject.SetActive (true);
				}
				listLevelButton [i].level = lv;
			} else {
				listLevelButton [i].gameObject.SetActive (false);
			}
		}
	}

	public void NextPage ()
	{
		// mở trang levels tiếp theo
		AudioManager.PlaySound (AudioClipType.AC_BUTTON);
		if (page < maxPage) {
			// kill hiệu ứng di chuyển page , tránh lỗi khi các level đang di chuyển mà bấm next thêm lần nữa
			DOTween.Kill ("abc", false);
			// di chuyển và set lại level cho các button
			levelGroup.transform.DOLocalMoveX (-800, 0.2F).SetEase (Ease.OutBack).SetId ("abc").OnComplete (() => {
				page++;
				InitLevel ();
				levelGroup.transform.position = new Vector3 (8, 0, 0);
				levelGroup.transform.DOLocalMoveX (0, 0.4F).SetEase (Ease.OutBack).SetId ("abc");
			});
		}
	}

	public void PrePage ()
	{
		// mở trang levels trước đó
		AudioManager.PlaySound (AudioClipType.AC_BUTTON);
		if (page > 1) {
			DOTween.Kill ("abc", false);
			levelGroup.transform.DOLocalMoveX (800, 0.2F).SetId ("abc").SetEase (Ease.OutBack).OnComplete (() => {
				page--;
				InitLevel ();
				levelGroup.transform.position = new Vector3 (-8, 0, 0);
				levelGroup.transform.DOLocalMoveX (0, 0.4F).SetEase (Ease.OutBack).SetId ("abc");
			});
		}
	}

	public void Back ()
	{
		AudioManager.PlaySound (AudioClipType.AC_BUTTON);
		GameManager.gameState = GameState.SelectLevel;
	}
}
