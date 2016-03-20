using UnityEngine;
using System.Collections;

public class FootFollow : MonoBehaviour {

	public Transform	foot;
	public Transform	tommy;
	private float		footToTommyDistance;
	private Vector3 	originalCombatPosition;
	private Vector3 	footOffset;
	private bool		followingFoot = false;
	private CombatController combat; //the combat script that keeps track of the combat flow
	public float speed; 


	void Awake () {
		Application.targetFrameRate = 50;
	}

	// Use this for initialization
	void Start () {
		originalCombatPosition = transform.position;
		combat = GameObject.Find("CombatController").GetComponent<CombatController>();
	}
	
	// Update is called once per frame
	void Update () {
		//return camera to original position when turn ends
		if (combat.turn == TurnState.ENEMY) {
			float step = speed * Time.deltaTime;
			transform.position = Vector3.Lerp (transform.position, originalCombatPosition, step);
		}

		if (followingFoot == false)
		{	
			KeepFootInView();
		}

		else 
		{
			FollowFoot();
		}
	}

	//if the foot passes the camera's center x or y axis, return true
	void KeepFootInView()
	{
		if (foot.position.x >= transform.position.x || foot.position.y >= transform.position.y)
		{
			followingFoot = true;
			//distance from foot to camera
			footOffset = foot.position - transform.position;
			//distance from tommy to foot when it first crosses one of the axes
			footToTommyDistance = Vector3.Distance(foot.position, tommy.position);
		}
		else
			followingFoot = false;			
	}

	//keep the current offset from Tommy's foot
	void FollowFoot()
	{
		if (Vector3.Distance(foot.position, tommy.position) <= footToTommyDistance)
		{
			//transform.position = originalCombatPosition;
			followingFoot = false;
		}

		else
		{
			Vector3 newPos = foot.position - footOffset;
			transform.position = newPos;
		}
	}
}
