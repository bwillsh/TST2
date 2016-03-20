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
				CantMove();
				break;
			case JumpState.STUNNED:
				anim.SetInteger("State", 3);
//				if (currentJumpPoint != jumpPoints.Count - 1) {
//					currentJumpPoint += 1;
//				}
				break;
			case JumpState.FORWARD:
				anim.SetInteger("State", 1);
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
	public int stunned_turns = 0;
	private Animator anim;
	bool canMove = false;

	// Use this for initialization

	void Start () 
	{
		anim = GetComponent<Animator>();
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
			if (jumpState != JumpState.STUNNED) {
				jumpState = JumpState.FORWARD;
			}
		}
		if (jumpState == JumpState.FORWARD && canMove) {
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
			anim.SetInteger("State", 2);
		}
	}

	//move ninja to a previous jump point
	void Knockback()
	{	
		transform.position = Vector3.MoveTowards (transform.position, jumpPoints [currentJumpPoint].position, jumpSpeed * 2 * Time.deltaTime);
		if (transform.position == jumpPoints[currentJumpPoint].position)
		{
			jumpState = JumpState.GROUNDED;
			anim.SetInteger("State", 2);
		}
	}

	
	void Stunned()
	{	
		//stay stunned for 3 turns
		if (combat.turn == TurnState.ENEMYSTART) {
			++stunned_turns;
			//Debug.Log (stunned_turns);
		}
		if(stunned_turns == 3) {
			jumpState = JumpState.FORWARD;
			stunned_turns = 0;
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
			Instantiate (explosion, transform.position, Quaternion.identity);
			Destroy (this.gameObject);

		} else if (jumpState == JumpState.GROUNDED && coll.gameObject.tag == "Foot") { 
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

	void GoToIdle()
	{
		anim.SetInteger("State", 0);
		Flip();
	}

	void CanMove()
	{
		canMove = true;
	}
	void CantMove()
	{
		canMove = false;
	}

	void Flip()
	{
		int nextPos = jumpPoints.Count - 1;
		if (currentJumpPoint > 0) nextPos = currentJumpPoint - 1;

		if ((jumpPoints[nextPos].position.x > jumpPoints[currentJumpPoint].position.x &&
			transform.localScale.x < 0) || 
			(jumpPoints[nextPos].position.x < jumpPoints[currentJumpPoint].position.x &&
				transform.localScale.x > 0))
		{
			Vector3 temp = transform.localScale;
			temp.x *= -1;
			transform.localScale = temp;
		}
	}
}
	
