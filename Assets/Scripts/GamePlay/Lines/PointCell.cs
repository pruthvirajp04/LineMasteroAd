using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PointCell : MonoBehaviour
{
	public int id;
	bool _isEnablePoint;

	public bool isEnablePoint {
		get { return _isEnablePoint; }
		set {
			_isEnablePoint = value;

			if (value) {
				OnPointEnableEffect ();
			}
			if (value) {		
				// hiệu ứng mờ dần
				point.DOFade (0, 0F).OnComplete (() => {
					point.enabled = value;
				});
			} else {
				point.enabled = value;
				mask.enabled = value;
			}

		}
	}

	public Color color;
	bool _isMainType;

	public bool isMainType {
		get { return _isMainType; }
		set {
			_isMainType = value;
			if (value) {
				point.sortingOrder = 11;
				point.transform.position = transform.position;

			} else {
				
				point.sortingOrder = 9;
				point.transform.position = transform.position + new Vector3 (0, -0.1F, 0);
			}
		}
	}

	public void SetColor (Color col)
	{
		color = col;
		point.color = col;
		ring.color = col;
	}

	public SpriteRenderer point;
	public SpriteMask mask;

	public SpriteRenderer ring;
	bool isEnablingEffect = false;

	void OnPointEnableEffect ()
	{
		if (isEnablingEffect) {
			return;
		}
		isMainType = true;
		isEnablingEffect = true;
		point.DOFade (0, 0F).OnComplete (() => {
			point.DOFade (1, 1F).OnComplete (() => {
				isEnablingEffect = false;
			});
		});
		point.transform.localScale = new Vector3 (3, 3, 3);
		point.transform.DOScale (new Vector3 (1, 1, 1), 0.5F);
	}

	public void ScaleRingIn ()
	{
		ring.transform.localScale = new Vector3 (2, 2, 2);
		ring.transform.DOScale (new Vector3 (1, 1, 1), 0.2F);
		ring.DOFade (0, 0F).OnComplete (() => {
			ring.enabled = true;
			ring.DOFade (1, 0.2F);
		});

	}

	public void ScaleRingZero ()
	{
		ring.enabled = true;
		ring.transform.localScale = new Vector3 (1, 1, 1);
		ring.transform.DOScale (Vector3.zero, 1F);
	}

	public void ScaleRingOut ()
	{
		ring.enabled = true;
		ring.transform.localScale = new Vector3 (1, 1, 1);
		ring.transform.DOScale (new Vector3 (2, 2, 2), 0.2F);
		ring.DOFade (1, 0F).OnComplete (() => {			
			ring.DOFade (0, 0.2F).OnComplete (() => {
				ring.enabled = false;
			});
		});
	}
}
