using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class TutorialControl : MonoBehaviour
{

	// pcTarget : poincell bat buoc phai di toi khi hien tutorial
	// pc1,pc2 : pointcell cua line dang chon
	public PointCell pcTarget, pc1, pc2;
	// lineTarget : line bat buoc phai nhap vao khi hien tutorial
	public Line lineTarget;
	public TextMeshPro notice;
	public int tutorialStep;
	public GameObject mask;
	public bool haveTutorial = false;
	public Transform tutorialHand;

	public Vector3 tutorialHandStartPos, tutorialHandEndPos;

	public void StartTutorial (GameMode mode)
	{
		GameControl.instance.bottomObj.SetActive (false);
		tutorialStep = 0;
		gameObject.SetActive (true);
		switch (mode) {
		case GameMode.Normal: 
			StartCoroutine (NormalModeTutorial ());
			break;
		case GameMode.Copy: 
			StartCoroutine (CopyModeTutorial ());
			break;
		case GameMode.Double: 
			StartCoroutine (DoubleModeTutorial ());
			break;
		}

	}

	// hiệu ứng ngón tay di chuyển
	void SetTutorialHand (bool isShow = true)
	{
		if (isShow) {
			tutorialHand.position = tutorialHandStartPos;
			DOTween.Kill ("tutorialHand");
			tutorialHand.DOMove (tutorialHandEndPos, 1.5F).SetId ("tutorialHand").SetLoops (999, LoopType.Restart);
		} else {
			DOTween.Kill ("tutorialHand");
			tutorialHand.transform.position = new Vector3 (100, 100, 0);
		}
	}
	//---------------------
	PointCell GetPoinCellByID (int id)
	{
		PointCell pc;
		for (int i = 0; i < GameControl.instance.listPointCell.Count; i++) {
			pc = GameControl.instance.listPointCell [i];
			if (pc.id == id) {
				return pc;
			}
		}
		return null;
	}
	//--------------------------
	void IncreaseTutorialStep ()
	{
		tutorialStep++;
	}
	//-------------------------
	// tutorial chế độ normal
	IEnumerator NormalModeTutorial ()
	{
		haveTutorial = true;
		int oldTutorialStep = -1;
		while (tutorialStep < 5) {
			if (tutorialStep > oldTutorialStep) {
				oldTutorialStep = tutorialStep;
				switch (tutorialStep) {
				case 0: 
					mask.transform.position = GetPoinCellByID (12).transform.position;
					notice.text = LanguageManager.GetText (LanguageKey.normal_step_1);
					Invoke ("IncreaseTutorialStep", 2F);
					SetTutorialHand (false);
					break;
				case 1: 
					mask.transform.position = GetPoinCellByID (2).transform.position;
					pcTarget = GetPoinCellByID (2);
					pc1 = GetPoinCellByID (6);
					pc2 = GetPoinCellByID (8);
					notice.text = LanguageManager.GetText (LanguageKey.normal_step_2);
					tutorialHandEndPos = pcTarget.transform.position;
					tutorialHandStartPos = (pc2.transform.position + pc1.transform.position) / 2;
					SetTutorialHand (true);
					break;
				case 2: 
					mask.transform.position = GetPoinCellByID (14).transform.position;
					pcTarget = GetPoinCellByID (14);
					pc1 = GetPoinCellByID (18);
					pc2 = GetPoinCellByID (8);
					notice.text = LanguageManager.GetText (LanguageKey.normal_step_3);
					tutorialHandEndPos = pcTarget.transform.position;
					tutorialHandStartPos = (pc2.transform.position + pc1.transform.position) / 2;
					SetTutorialHand (true);
					break;
				case 3: 
					mask.transform.position = GetPoinCellByID (22).transform.position;
					pcTarget = GetPoinCellByID (22);
					pc1 = GetPoinCellByID (16);
					pc2 = GetPoinCellByID (18);
					notice.text = LanguageManager.GetText (LanguageKey.normal_step_4);
					tutorialHandEndPos = pcTarget.transform.position;
					tutorialHandStartPos = (pc2.transform.position + pc1.transform.position) / 2;
					SetTutorialHand (true);
					break;
				case 4: 
					mask.transform.position = GetPoinCellByID (10).transform.position;
					pcTarget = GetPoinCellByID (10);
					pc1 = GetPoinCellByID (6);
					pc2 = GetPoinCellByID (16);
					notice.text = LanguageManager.GetText (LanguageKey.normal_step_5);
					tutorialHandEndPos = pcTarget.transform.position;
					tutorialHandStartPos = (pc2.transform.position + pc1.transform.position) / 2;
					SetTutorialHand (true);
					break;
				}

			}
			yield return null;
		}

		//
		GameControl.instance.bottomObj.SetActive (true);
		haveTutorial = false;
		DOTween.Kill ("tutorialHand");
		gameObject.SetActive (false);


	}
	//-------------------------------
	// tutorial chế độ copy
	IEnumerator CopyModeTutorial ()
	{
		yield return null;
	}
	//--------------------------------
	// tutorial chế độ double
	IEnumerator DoubleModeTutorial ()
	{
		haveTutorial = true;
		int oldTutorialStep = -1;




		while (tutorialStep < 3) {
			
			if (tutorialStep > oldTutorialStep) {
				oldTutorialStep = tutorialStep;
				switch (tutorialStep) {
				case 0: 

					Vector3 p1 = GetPoinCellByID (7).transform.position;
					Vector3 p2 = GetPoinCellByID (12).transform.position;
					mask.transform.position = new Vector3 ((p1.x + p2.x) / 2, (p1.y + p2.y) / 2, 0);
					mask.transform.localScale = new Vector3 (1, 0.7F, 1);
					pcTarget = GetPoinCellByID (12);
					pc1 = GetPoinCellByID (6);
					pc2 = GetPoinCellByID (8);
					notice.text = LanguageManager.GetText (LanguageKey.double_step_1);
					tutorialHandEndPos = pcTarget.transform.position;
					tutorialHandStartPos = (pc2.transform.position + pc1.transform.position) / 2;
					SetTutorialHand (true);
					break;
				case 1: 
					
					mask.transform.position = new Vector3 (100, 100, 0);
					notice.text = LanguageManager.GetText (LanguageKey.double_step_2);
					tutorialHandEndPos = pcTarget.transform.position;
					tutorialHandStartPos = (pc2.transform.position + pc1.transform.position) / 2;
					SetTutorialHand (false);
					break;
				case 2: 
					p1 = GetPoinCellByID (17).transform.position;
					p2 = GetPoinCellByID (12).transform.position;
					mask.transform.position = new Vector3 ((p1.x + p2.x) / 2, (p1.y + p2.y) / 2, 0);
					mask.transform.localScale = new Vector3 (1, 0.7F, 1);
					pcTarget = GetPoinCellByID (12);
					pc1 = GetPoinCellByID (16);
					pc2 = GetPoinCellByID (18);
					notice.text = LanguageManager.GetText (LanguageKey.double_step_3);
					tutorialHandEndPos = pcTarget.transform.position;
					tutorialHandStartPos = (pc2.transform.position + pc1.transform.position) / 2;
					SetTutorialHand (true);
					break;				
				}

			}
			yield return null;
		}

		//
		GameControl.instance.bottomObj.SetActive (true);
		haveTutorial = false;
		DOTween.Kill ("tutorialHand");
		gameObject.SetActive (false);
	}

}
