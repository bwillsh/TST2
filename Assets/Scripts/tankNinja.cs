using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class tankNinja : NinjaParent {
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
				CombatController.S.NinjaDoneMoving();
				CantMove();
				canDealDamage = true;
				break;
			case JumpState.STUNNED:
				CombatController.S.NinjaStunned();
				anim.SetInteger("State", 3);
				break;
			case JumpState.FORWARD:
				anim.SetInteger("State", 1);
				SetJumpPoints();
				currentJumpPoint -= 1;
				StartCoroutine(PlayJumpSound(Random.Range(0, .5f)));
				break;
			case JumpState.KNOCKBACK:
				currentJumpDuration = 0;
				SetJumpPoints();
				currentJumpPoint += jumpPoints.Count - 1;
				currentJumpPoint = Mathf.Clamp(currentJumpPoint, 0, numberOfJumpPoints - 1);
				break;
			default:
				break;
			}
		}
	}

	public List<Transform>  jumpPoints; //a list of empty Transforms for the ninja to jump to
	private List<TurnCounter> turnCounter;
	public GameObject		turnCounterObject;
	public float			spaceOfCounters = 2.3f;
	public int				numberOfJumpPoints; //the total number of jump points for this ninja
	public int				currentJumpPoint; //which jump point the ninja is currently on
	public float			jumpSpeed; //the speed to jump between positions
	private CombatController combat; //the combat script that keeps track of the combat flow
	public ParticleSystem	explosion; //the particle system that makes the ninja explode
	public int 				stunned_turns = 0;
	private Animator 		anim;
	bool 					canMove = false; //needed for animation
	public float			jumpDuration = 3;
	public int				stunnedTurnsMax = 3;
	private float			currentJumpDuration = 0;
	private Vector3			startJump, endJump, midJump;
	public Transform		emptyTransform;
	private Transform		countHolder;
	private AudioSource 	deathSound, jumpSound, punch;
	[HideInInspector] public bool canDealDamage = true;

	// Use this for initialization

	void Start () 
	{
		jumpSound = GameObject.Find("NinjaTankJump").GetComponent<AudioSource>();
		deathSound = GameObject.Find("NinjaDeathSound").GetComponent<AudioSource>();
		punch = GameObject.Find("Punch").GetComponent<AudioSource>();
		anim = GetComponent<Animator>();
		combat = GameObject.Find("CombatController").GetComponent<CombatController>();
		++combat.NinjaCount;

		//initialize variables
		jumpState = JumpState.GROUNDED;
		numberOfJumpPoints = jumpPoints.Count;
		currentJumpPoint = numberOfJumpPoints - 1;
		for (int i = 0; i < numberOfJumpPoints; i += 1)
		{
			Vector3 temp = jumpPoints[i].position;
			temp.z = transform.position.z;
			jumpPoints[i].position = temp;
		}

		countHolder = Instantiate(emptyTransform, transform.position, transform.rotation) as Transform;
		countHolder.parent = transform;

		turnCounter = new List<TurnCounter>();
		float spacing = 0;
		if ((numberOfJumpPoints - 1) % 2 == 0) spacing = (spaceOfCounters / ((numberOfJumpPoints - 1) * 2)) * (numberOfJumpPoints - 2);
		else spacing = (spaceOfCounters / (numberOfJumpPoints - 1)) * ((numberOfJumpPoints - 1) / 2);
		for (int i = 0; i < numberOfJumpPoints - 1; ++i)
		{
			float num = i * (spaceOfCounters / (numberOfJumpPoints - 1));
			Vector3 spot = new Vector3(transform.position.x + num - spacing, transform.position.y + 2f, transform.position.z);
			GameObject go = Instantiate(turnCounterObject, spot, Quaternion.identity) as GameObject;
			turnCounter.Add(go.GetComponent<TurnCounter>());
			go.transform.parent = countHolder;
			if (i == numberOfJumpPoints - 3) go.GetComponent<TurnCounter>().onColor = Color.yellow;
			if (i == numberOfJumpPoints - 2) go.GetComponent<TurnCounter>().onColor = Color.red;
		}

		Flip();
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
			//if (!canMove) CanMove();
		}
		if (jumpState == JumpState.FORWARD && canMove) {
			JumpForward ();
		}
		else if (jumpState == JumpState.KNOCKBACK) {
			Knockback ();
		}
		if (jumpState == JumpState.STUNNED) {
			Stunned ();
		}

			
	}

	public void StartAttack()
	{
		jumpState = JumpState.FORWARD;
	}

	//move Ninja to next jump point
	void JumpForward()
	{	
		transform.position = MoveAlongBezierCurve(currentJumpDuration / jumpDuration);
		currentJumpDuration += Time.deltaTime;
		if (currentJumpDuration >= jumpDuration)
		{
			transform.position = endJump;
			currentJumpDuration = 0;
			jumpState = JumpState.GROUNDED;
			anim.SetInteger("State", 2);
		}
	}

	//move ninja to a previous jump point
	void Knockback()
	{	
		transform.position = MoveAlongBezierCurve(currentJumpDuration / jumpDuration);
		currentJumpDuration += Time.deltaTime;
		if (currentJumpDuration >= jumpDuration)
		{
			transform.position = endJump;
			currentJumpDuration = 0;
			jumpState = JumpState.GROUNDED;
			anim.SetInteger("State", 2);
		}
	}

	
	void Stunned()
	{	
		//stay stunned for 3 turns
		if (combat.turn == TurnState.ENEMYSTART) {
			++stunned_turns;
		}
		if(stunned_turns == stunnedTurnsMax + 1) {
			anim.SetInteger("State", 0);
			jumpState = JumpState.GROUNDED;
			CombatController.S.NinjaUnStunned();
			stunned_turns = 0;
		}
	}

	//when hit by the foot or Tommy (not currently working, which is fine; easy fix)
	void OnTriggerEnter2D (Collider2D coll)
	{
		if (jumpState == JumpState.STUNNED && coll.gameObject.tag == "Foot" && stunned_turns > 0) {
			deathSound.PlayOneShot(deathSound.clip);
			Instantiate (explosion, transform.position, Quaternion.identity);
			Destroy (this.gameObject);
			--combat.NinjaCount;
			CombatController.S.NinjaUnStunned();
			if (combat.NinjaCount == 0) {
				combat.ItemDropPosition = transform.position;
			}

		} else if (jumpState == JumpState.GROUNDED && coll.gameObject.tag == "Foot") { 
			jumpState = JumpState.STUNNED;
			punch.PlayOneShot(punch.clip);
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
			Vector3 countScale = countHolder.localScale;
			countScale.x *= -1;
			countHolder.localScale = countScale;
		}
	}

	Vector3 MoveAlongBezierCurve(float t)
	{
		float u = 1 - t;

		Vector3 point = u * u * startJump;
		point += 2 * u * t * midJump;
		point += t * t * endJump;

		return point;
	}

	void SetJumpPoints()
	{
		startJump = jumpPoints[currentJumpPoint].position;
		if (currentJumpPoint == 0) endJump = jumpPoints[jumpPoints.Count - 1].position;
		else endJump = jumpPoints[currentJumpPoint - 1].position;
		midJump.z = startJump.z;
		midJump.x = (startJump.x + endJump.x) / 2;
		midJump.y = Mathf.Max(startJump.y, endJump.y) + Mathf.Abs(((startJump.y + endJump.y) / 2));
		float height = GetComponent<PolygonCollider2D>().bounds.max.y - transform.position.y;
		if (midJump.y > CombatController.S.ceilingHeight || transform.position.y + height > CombatController.S.ceilingHeight)
		{
			midJump.y = CombatController.S.ceilingHeight - height;
		}
	}

	IEnumerator PlayJumpSound(float time)
	{
		yield return new WaitForSeconds(time);
		jumpSound.pitch = Random.Range(.5f, .8f);
		jumpSound.PlayOneShot(jumpSound.clip);
	}
}
	
