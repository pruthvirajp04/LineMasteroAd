using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Line : MonoBehaviour
{
	// 2 điểm của line
	public PointCell pc1, pc2;
	// vector 2 điểm
	public Vector3 point1, point2;
	// vector điểm giữa
	public Vector3 midPoint;
	//
	public LineRenderer lineRenderer, shadowLineRenderer;
	//
	public int vertextCount = 2;
	// dùng để set đường cong cho line , làm hiệu ứng dây rung
	public bool isBeizer = false;

	public bool haveMidPoint = false;
	public EdgeCollider2D col;

	float _width;
	// sprite line ,mask dùng để làm hiệu ứng ánh sáng trên dây lúc thằng
	public SpriteRenderer lineSprite;
	public SpriteMask mask;
	//
	bool canChooseThisLine = true;

	bool _haveShadow = true;

	[SerializeField]
	bool _isGoalLine = false;

	public bool isGoalLine {
		get { return _isGoalLine; }
		set {
			_isGoalLine = value;
			if (value) {
				mask.enabled = false;
				lineSprite.enabled = false;
				shadowLineRenderer.enabled = false;
			} else {
				shadowLineRenderer.enabled = true;
			}
		}
	}
	// độ rộng của dây + collider
	public float width {
		get { return _width; }
		set {
			_width = value;
			col.edgeRadius = value + 0.3F;
			lineRenderer.startWidth = value;
			lineRenderer.endWidth = value;
		}
	}

	[SerializeField]
	LineType _lineType;

	public LineType lineType {
		get { return _lineType; }
		set {
			_lineType = value;
			InitLine ();
		}
	}

	void InitLine ()
	{
		switch (lineType) {
		case LineType.goal_line:
			isGoalLine = true;
			lineSprite.color = GameConfig.mainColor;
			lineRenderer.startColor = GameConfig.mainColor;
			lineRenderer.endColor = GameConfig.mainColor;
			//pc1.SetColor (GameConfig.mainColor);
			break;
		case LineType.init_line_1:
			shadowLineRenderer.enabled = true;
			lineSprite.color = GameConfig.mainColor;
			lineRenderer.startColor = GameConfig.mainColor;
			lineRenderer.endColor = GameConfig.mainColor;
			pc1.SetColor (GameConfig.mainColor);
			pc2.SetColor (GameConfig.mainColor);
			break;
		case LineType.init_line_2:
			shadowLineRenderer.enabled = true;
			lineSprite.color = GameConfig.mainColor2;
			lineRenderer.startColor = GameConfig.mainColor2;
			lineRenderer.endColor = GameConfig.mainColor2;
			pc1.SetColor (GameConfig.mainColor2);
			pc2.SetColor (GameConfig.mainColor2);
			break;
		case LineType.mirror_line:
			shadowLineRenderer.enabled = false;
			lineSprite.color = GameConfig.mainColor2;
			lineRenderer.startColor = GameConfig.mainColor2;
			lineRenderer.endColor = GameConfig.mainColor2;
			pc1.SetColor (GameConfig.mainColor2);
			pc2.SetColor (GameConfig.mainColor2);
			break;
		}
	}

	void OnEnable ()
	{
		
		InitLine ();
	}

	void Awake ()
	{
		mask.enabled = false;
		lineRenderer = GetComponent<LineRenderer> ();
		lineRenderer.startColor = GameConfig.mainColor;
		lineRenderer.endColor = GameConfig.mainColor;
		pointList = new List<Vector3> ();
		pointList2D = new List<Vector2> ();
		shadowPointList = new List<Vector3> ();
		col = GetComponent<EdgeCollider2D> ();

	}
	// pointList : list cac diem de ve duong thang
	List<Vector3> pointList;
	// pointList2D : list cac diem de ve collider
	List<Vector2> pointList2D;
	// shadowPointList: list cac diem de ve shadow cua line
	List<Vector3> shadowPointList;
	bool haveCrossWithOtherLine = false;
	Vector3 crossPoint;

	void Update ()
	{
		if (GameManager.gameState != GameState.Playing) {
			return;
		}
		pointList.Clear ();
		pointList2D.Clear ();
		shadowPointList.Clear ();
		// haveMidPoint = true <=> dây đang được kéo 
		if (haveMidPoint) {
			if (isBeizer == true) {
				// init các điểm để vẽ đường cong beizer
				for (float ratio = 0; ratio <= 1; ratio += 1F / vertextCount) {
					var tangentLineVertex1 = Vector3.Lerp (point1, midPoint, ratio);
					var tangentLineVertex2 = Vector3.Lerp (midPoint, point2, ratio);
					var beizerPoint = Vector3.Lerp (tangentLineVertex1, tangentLineVertex2, ratio);
					pointList.Add (beizerPoint);

				}
			} else {
				// vẽ 2 đường thẳng theo chuột
				pointList.Add (point1);
				pointList.Add (midPoint);

				pointList.Add (point2);

				// check cắt với các đường thẳng trên card
				haveCrossWithOtherLine = false;
				for (int i = 0; i < GameControl.instance.listLine.Count; i++) {
					Line l = GameControl.instance.listLine [i];
					if (l != this && l.lineType == lineType) {
						Vector3 cross1 = GameControl.instance.CrossPoint (point1, midPoint, l.pc1.transform.position, l.pc2.transform.position);
						Vector3 cross2 = GameControl.instance.CrossPoint (point2, midPoint, l.pc1.transform.position, l.pc2.transform.position);
					    
						if (cross1.x != 9999) {
							crossPoint = cross1;
							haveCrossWithOtherLine = true;
							break;
						}
						if (cross2.x != 9999) {
							crossPoint = cross2;
							haveCrossWithOtherLine = true;
							break;
						}
					}
				}
				/// nếu có cắt thì hiện icon cross
				if (haveCrossWithOtherLine && GameControl.instance.selectedLine != null) {
					GameControl.instance.crossSprite.transform.position = crossPoint;
				} else {
					GameControl.instance.crossSprite.transform.position = new Vector3 (9999, 0, 0);
				}
				/// 
			}
		} else {
			// list điểm để vẽ 1 đường thẳng thường 
			pointList.Add (point1);
			pointList.Add (point2);
			// list điểm của collider
			pointList2D.Add (point1);
			pointList2D.Add (point2);


			col.points = pointList2D.ToArray ();
			//----------------
			// set line sprite
			lineSprite.transform.position = new Vector3 ((point1.x + point2.x) / 2, (point1.y + point2.y) / 2, 0);
			float scale = Vector3.Distance (point1, point2);
			lineSprite.transform.localScale = new Vector3 (scale, 1, 1);
			Vector2 dir = point2 - point1;
			float rot = Mathf.Atan2 (dir.y, dir.x) * Mathf.Rad2Deg;
			lineSprite.transform.rotation = Quaternion.Euler (0, 0, rot);
			//--------------------

		}
		// tạo list điểm cho line shadow
		for (int i = 0; i < pointList.Count; i++) {
			shadowPointList.Add (pointList [i] + new Vector3 (0, -0.1F, 0));
		}

		lineRenderer.positionCount = pointList.Count;
		shadowLineRenderer.positionCount = pointList.Count;
		shadowLineRenderer.SetPositions (shadowPointList.ToArray ());


		if (lineType == GameControl.instance.mainLineType || lineType == LineType.goal_line) {
			lineRenderer.SetPositions (pointList.ToArray ());
			col.enabled = true;
		} else {
			col.enabled = false;
			lineRenderer.SetPositions (shadowPointList.ToArray ());
		}
	}

	void OnMouseDown ()
	{
		
		if (GameManager.gameState != GameState.Playing) {
			return;
		}
		// neu ko phai la type co the click thi return
		if (lineType != GameControl.instance.mainLineType) {
			return;
		}
	
		// khong cho chon day neu khong phai la day tutorial
		if (GameConfig.instance.tutorialControl.haveTutorial) {
			PointCell tPc1 = GameConfig.instance.tutorialControl.pc1;
			PointCell tPc2 = GameConfig.instance.tutorialControl.pc2;
			if (!GameControl.instance.isLineAMatchLineB (pc1, pc2, tPc1, tPc2)) {
				return;
			}
		}

		GameControl.instance.midPointRender.color = lineSprite.color;
		if (GameControl.instance.selectedLine == null && GameControl.instance.canClickLine == true && canChooseThisLine == true) {
			// set lại color cho 2 poin nếu sai ( mode 2-color)
			pc1.SetColor (lineSprite.color);
			pc2.SetColor (lineSprite.color);
			// vòng tròn quanh 2 pointCell
			pc1.ScaleRingIn ();
			pc2.ScaleRingIn ();
			// set lại pósint1,point2
			point1 = pc1.transform.position;
			point2 = pc2.transform.position;

			this.haveMidPoint = true;
			isBeizer = false;
			GameControl.instance.canClickLine = false;
			GameControl.instance.selectedLine = this;

		}
	}

	public void OnMouseRelease (bool createNewLine)
	{
		point1 = pc1.transform.position;
		point2 = pc2.transform.position;
		GameControl.instance.crossSprite.transform.position = new Vector3 (9999, 0, 0);
		if (createNewLine == false || haveCrossWithOtherLine) {
			// nếu không thể đặt xuống
			//hiệu ứng vòng tròn xung quanh pointCell
			pc1.ScaleRingZero ();
			pc2.ScaleRingZero ();
			//
			Vector3 centerPoint = new Vector3 ((point1.x + point2.x) / 2, (point1.y + point2.y) / 2, 0);
			this.isBeizer = true;
			// hiệu ứng dây rung
			DOTween.To (() => midPoint, x => midPoint = x, centerPoint, 1F).SetEase (Ease.OutElastic).onComplete += OnEndMouseUp;
			canChooseThisLine = false;
			GameControl.instance.canClickLine = true;
		} else {
			AudioManager.PlaySound (AudioClipType.AC_TOUCHEND_CORRECT);

			// add info to undo list 
			pc1.ScaleRingOut ();
			pc2.ScaleRingOut ();
			UndoModule undo = new UndoModule ();
			undo.pc1 = pc1;
			undo.pc2 = pc2;
			undo.pcNew = GameControl.instance.nearestPointCell;
			undo.clearDot = undo.pcNew.point.enabled ? false : true;
			undo.pcNewIsMainType = undo.pcNew.isMainType;
			undo.lineType = lineType;
			undo.pcNewColor = undo.pcNew.color;
			GameControl.instance.listUndoModuleMain.Add (undo);

			// tạo 2 dây mới 
			Line l1 = GameControl.instance.CreateLine (this.point1, GameControl.instance.nearestPointCell.transform.position, 0.2F);
			l1.gameObject.transform.SetParent (GameControl.instance.card);
			l1.pc1 = pc1;
			l1.pc2 = GameControl.instance.nearestPointCell;
			l1.lineType = lineType;
			Line l2 = GameControl.instance.CreateLine (this.point2, GameControl.instance.nearestPointCell.transform.position, 0.2F);
			l2.gameObject.transform.SetParent (GameControl.instance.card);
			l2.pc1 = pc2;
			l2.pc2 = GameControl.instance.nearestPointCell;
			l2.lineType = lineType;

			// reset other value
			GameControl.instance.canClickLine = true;
			GameControl.instance.nearestPointCell.isEnablePoint = true;
			GameControl.instance.listLine.Add (l1);
			GameControl.instance.listLine.Add (l2);
			// xóa dây này đi
			GameControl.instance.listLine.Remove (this);
			Destroy (gameObject, 0.04F);
			GameControl.instance.CheckWin2 ();



		}

	}

	void OnEndMouseUp ()
	{
		
		canChooseThisLine = true;
		this.haveMidPoint = false;
		this.isBeizer = false;
	}

}

public enum LineType
{
	goal_line,
	init_line_1,
	init_line_2,
	mirror_line
}