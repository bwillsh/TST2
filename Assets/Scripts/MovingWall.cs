using UnityEngine;
using System.Collections;

public class MovingWall : MonoBehaviour {

	public Vector3 	spot1;
	public Vector3	spot2;
	private Vector3	targetSpot;
	public float	speed;
	private bool	moving = true;
	private bool	stunned = false;

	// Use this for initialization
	void Start () {
		targetSpot = spot1;
	}
	
	// Update is called once per frame
	void Update () {
		if (moving)
		{
			Move();
		}
		else if (stunned && Foot.S.attackState == AttackState.NORMAL)
		{
			stunned = false;
			moving = true;
		}
	}

	void Move()
	{

		transform.position = Vector3.MoveTowards(transform.position, targetSpot, speed * Time.deltaTime);
		if (transform.position == targetSpot)
		{
			moving = false;
			if (targetSpot == spot1)
			{
				targetSpot = spot2;
			}
			else
			{
				targetSpot = spot1;
			}
			StartCoroutine (Pause(2));
		}
	}
		
	IEnumerator Pause(float time)
	{
		yield return new WaitForSeconds(time);
		if (!stunned)
			moving = true;
	}

	void OnTriggerEnter2D(Collider2D coll)
	{
		if (coll.gameObject.tag == "Foot")
		{
			StopCoroutine("Pause");
			moving = false;
			stunned = true;
		}
	}
}
