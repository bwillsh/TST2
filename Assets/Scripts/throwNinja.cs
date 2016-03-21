using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public enum ThrowState
{
	STAGE0,
	STAGE1,
	STAGE2,
	STAGE3,
	THROWING
}

public class throwNinja : NinjaParent {

	private ThrowState _throwing = ThrowState.STAGE0;
	public ThrowState throwing {
		get {return _throwing;}
		set
		{
			if (value == _throwing) return;
			switch(value)
			{
			case ThrowState.STAGE0:
				break;
			case ThrowState.STAGE1:
				NextAnimationState();
				break;
			case ThrowState.STAGE2:
				NextAnimationState();
				break;
			case ThrowState.STAGE3:
				NextAnimationState();
				break;
			case ThrowState.THROWING:
				NextAnimationState();
				animationState = 0;
				break;
			default:
				break;
			}
		}
	}

	public int current_turn;
	private CombatController combat; //the combat script that keeps track of the combat flow
	public ParticleSystem	explosion; //the particle system that makes the ninja explode
	public List<TurnCounter> turnCounter;
	public int max_turns;
	public GameObject NinjaStar;
	public float speed;
	public Sprite ninjaStar;
	private Animator anim;
	private int animationState = 0;

	// Use this for initialization
	void Start () {

		anim = GetComponent<Animator>();
		throwing = ThrowState.STAGE0;
		combat = GameObject.Find("CombatController").GetComponent<CombatController>();
		++combat.NinjaCount;

		current_turn = 0;

		//get star component

		foreach (Transform child in transform)
		{
			if (child.GetComponent<TurnCounter>() != null)
			{
				turnCounter.Add(child.GetComponent<TurnCounter>());
			}
		}
		max_turns = turnCounter.Count;
	}
	
	// Update is called once per frame
	void Update () {
		
		if (current_turn == max_turns) {
			if (current_turn > 0)
				NextState();
			current_turn = 0;
			Instantiate(NinjaStar, transform.position, Quaternion.identity);

		} 
		else if(combat.turn == TurnState.ENEMYSTART) {
			current_turn++;
			NextState();
		} 
		TurnOnTurnCounter (current_turn + 1);
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
		
	void OnTriggerEnter2D (Collider2D coll)
	{
		if (coll.gameObject.tag == "Foot") {
			Instantiate (explosion, transform.position, Quaternion.identity);
			Destroy (this.gameObject);
			--combat.NinjaCount;
			if (combat.NinjaCount == 0) {
				combat.ItemDropPosition = transform.position;
			}
		
		}
	}

	void NextAnimationState()
	{
		animationState++;
		anim.SetInteger("State", animationState);
	}

	void NextState()
	{
		switch (throwing)
		{
		case ThrowState.STAGE0:
			throwing = ThrowState.STAGE1;
			break;
		case ThrowState.STAGE1:
			throwing = ThrowState.STAGE2;
			break;
		case ThrowState.STAGE2:
			throwing = ThrowState.STAGE3;
			break;
		case ThrowState.STAGE3:
			throwing = ThrowState.THROWING;
			break;
		}
	}

	void ReturnToState0()
	{
		throwing = ThrowState.STAGE0;
		animationState = 0;
		anim.SetInteger("State", animationState);
	}
}

