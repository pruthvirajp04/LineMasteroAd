using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HintLine : MonoBehaviour
{

	public LineRenderer line;
	public SpriteRenderer arrow;

	public void CreateHintLine (PointCell pc1, PointCell pc2, PointCell pcTarget)
	{
		Vector3 startPos = new Vector3 ((pc1.transform.position.x + pc2.transform.position.x) / 2, (pc1.transform.position.y + pc2.transform.position.y) / 2, 0);
		Vector3 endPos = pcTarget.transform.position;
		// hiệu ứng hintline dài dần
		StartCoroutine (CreateHintLineCoroutine (startPos, endPos));
	}

	IEnumerator CreateHintLineCoroutine (Vector3 startPos, Vector3 endPos)
	{
		arrow.enabled = false;
		line.SetPosition (0, startPos);
		line.enabled = true;
		float t = 0;
		Vector3 pos;
		while (t < 1) {
			t += Time.deltaTime * 2;
			pos = Vector3.Lerp (startPos, endPos, t);
			line.SetPosition (1, pos);
			yield return null;
		}
		Vector2 dir = endPos - startPos;
		float rot = Mathf.Atan2 (dir.y, dir.x) * Mathf.Rad2Deg;
		arrow.transform.rotation = Quaternion.Euler (0, 0, rot - 90);
		arrow.transform.position = endPos;
		arrow.enabled = true;

	}

	public void ClearHintLine ()
	{
		line.enabled = false;
		arrow.enabled = false;
	}
}
