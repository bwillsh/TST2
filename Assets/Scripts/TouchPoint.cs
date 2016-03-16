using UnityEngine;
using System.Collections;

public class TouchPoint : MonoBehaviour {

	SpriteRenderer 	sprend;
	LineRenderer 	line;

	// Use this for initialization
	void Start () {
		sprend = GetComponent<SpriteRenderer>();
		line = GetComponent<LineRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
		if (Foot.S.attackState == AttackState.CHARGING)
		{
			sprend.enabled = true;
			Vector3 newPos = Foot.S.originalTouchPoint;
			newPos.z = 0;
			transform.position = newPos;

			Vector3 originalTouch = Foot.S.originalTouchPoint;
			originalTouch.z = 0;
			line.SetPosition(0, originalTouch);
			Vector3 currentTouchPoint = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,Input.mousePosition.y, 0));
			line.SetPosition(1, currentTouchPoint);
		}
		else 
		{
			line.SetPosition(0, Vector3.zero);
			line.SetPosition(1, Vector3.zero);
			sprend.enabled = false;
		}
	}
}
