using UnityEngine;
using System.Collections;

public class TouchPoint : MonoBehaviour {

	SpriteRenderer sprend;

	// Use this for initialization
	void Start () {
		sprend = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
		if (Foot.S.attackState == AttackState.CHARGING)
		{
			sprend.enabled = true;
			Vector3 newPos = Foot.S.originalTouchPoint;
			newPos.z = 0;
			transform.position = newPos;
			print (transform.position);
		}
		else 
		{
			sprend.enabled = false;
		}
	}
}
