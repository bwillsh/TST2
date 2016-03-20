using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class tankNinja : MonoBehaviour {
	public JumpState 		_jumpState;
	public JumpState		jumpState
	{
		get {return _jumpState;}
		set 
		{
			if (_jumpState == value) return;
			_jumpState = value;
			switch(_jumpState)
			{
			case JumpState.GROUNDED:
				break;
			case JumpState.STUNNED:
				if (currentJumpPoint != jumpPoints.Count - 1) {
					currentJumpPoint += 1;
				}
				break;
			case JumpState.FORWARD:
				currentJumpPoint -= 1;
				break;
			case JumpState.KNOCKBACK:
				currentJumpPoint += jumpPoints.Count - 1;
				currentJumpPoint = Mathf.Clamp(currentJumpPoint, 0, numberOfJumpPoints - 1);
				break;
			default:
				break;
			}
		}
	}

	public List<Transform>  jumpPoints; //a list of empty Transforms for the ninja to jump to
	public List<TurnCounter> turnCounter;
	public int				numberOfJumpPoints; //the total number of jump points for this ninja
	public int				currentJumpPoint; //which jump point the ninja is currently on
	public float			jumpSpeed; //the speed to jump between positions
	private CombatController combat; //the combat script that keeps track of the combat flow
	public ParticleSystem	explosion; //the particle system that makes the ninja explode
	private Foot			foot;
	private bool stunned = false;
	public int stunned_turns = 0;

	// Use this for initialization

	void Start () 
	{
		combat = GameObject.Find("CombatController").GetComponent<CombatController>();
		++combat.NinjaCount;

		//initialize variables
		foot = GameObject.Find ("Foot").GetComponent<Foot>();
		jumpState = JumpState.GROUNDED;
		numberOfJumpPoints = jumpPoints.Count;
		currentJumpPoint = numberOfJumpPoints - 1;
		for (int i = 0; i < numberOfJumpPoints; i += 1)
		{
			Vector3 temp = jumpPoints[i].position;
			temp.z = transform.position.z;
			jumpPoints[i].position = temp;
		}
		foreach (Transform child in transform)
		{
			if (child.GetComponent<TurnCounter>() != null)
			{
				turnCounter.Add(child.GetComponent<TurnCounter>());
			}
		}
	}

	// Update is called once per frame
	void Update () 
	{
		TurnOnTurnCounter (numberOfJumpPoints - currentJumpPoint);
		if (combat.turn == TurnState.ENEMYSTART)
		{	
			if (!stunned) {
				jumpState = JumpState.FORWARD;
			}
		}
		if (jumpState == JumpState.FORWARD) {
			JumpForward ();
		} else if (jumpState == JumpState.KNOCKBACK) {
			Knockback ();
		}
		if (jumpState == JumpState.STUNNED) {
			Stunned ();
		}
			
	}

	//move Ninja to next jump point
	void JumpForward()
	{	
		transform.position = Vector3.MoveTowards (transform.position, jumpPoints [currentJumpPoint].position, jumpSpeed * Time.deltaTime);
		if (transform.position == jumpPoints[currentJumpPoint].position)
		{
			jumpState = JumpState.GROUNDED;
		}
	}

	//move ninja to a previous jump point
	void Knockback()
	{	
		transform.position = Vector3.MoveTowards (transform.position, jumpPoints [currentJumpPoint].position, jumpSpeed * 2 * Time.deltaTime);
		if (transform.position == jumpPoints[currentJumpPoint].position)
		{
			jumpState = JumpState.GROUNDED;
		}
	}

	
	void Stunned()
	{	
		//don't knock back if on last jump point, knock back on first hit
		if (currentJumpPoint != jumpPoints.Count - 1 && stunned_turns == 0) {
			transform.position = Vector3.MoveTowards (transform.position, jumpPoints [currentJumpPoint].position, jumpSpeed * 2 * Time.deltaTime);
		}

		//stay stunned for 3 turns
		if (combat.turn == TurnState.ENEMYSTART) {
			++stunned_turns;
			Debug.Log (stunned_turns);
		}
		if(stunned_turns == 3){
			jumpState = JumpState.GROUNDED;
			stunned_turns = 0;
			stunned = false;
		}
	}

	//when hit by the foot or Tommy (not currently working, which is fine; easy fix)
	void OnTriggerEnter2D (Collider2D coll)
	{
		if (jumpState == JumpState.STUNNED && coll.gameObject.tag == "Foot" && stunned_turns > 0) {
			Instantiate (explosion, transform.position, Quaternion.identity);
			Destroy (this.gameObject);
			--combat.NinjaCount;
			if (combat.NinjaCount == 0) {
				combat.ItemDropPosition = transform.position;
			}

			Rigidbody2D footRB = coll.gameObject.GetComponent<Rigidbody2D> ();

			if (footRB.velocity.magnitude / foot.shotSpeedOriginal <= .3f || true) {

			} else {
				Instantiate (explosion, transform.position, Quaternion.identity);
				Destroy (this.gameObject);
			}
		} else if (jumpState == JumpState.GROUNDED && coll.gameObject.tag == "Foot" && !stunned) { 
			Debug.Log ("stunned");
			stunned = true;
			jumpState = JumpState.STUNNED;
		}
		else if (coll.gameObject.tag == "Player")
		{
			jumpState = JumpState.KNOCKBACK;
		}

	}
	void TurnOnTurnCounter(int n)
	{
		if (n > turnCounter.Count)
		{
			n = turnCounter.Count;
		}

		for (int i = 0; i < turnCounter.Count; ++i)
		{
			if (i < n)
			{
				turnCounter[i].TurnOn();
			}
			else 
			{
				turnCounter[i].TurnOff();
			}
		}
	}
}
	
