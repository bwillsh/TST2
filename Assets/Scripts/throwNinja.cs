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

	public GameObject turnCounterObject;
	public int current_turn;
	private CombatController combat; //the combat script that keeps track of the combat flow
	public ParticleSystem	explosion; //the particle system that makes the ninja explode
	private List<TurnCounter> turnCounter;
	public float spaceOfCounters = 2.3f;
	public int max_turns = 4;
	public GameObject NinjaStar;
	public float speed;
	public Sprite ninjaStar;
	private Animator anim;
	private int animationState = 0;
	private AudioSource audio;

	// Use this for initialization
	void Start () {

		audio = GameObject.Find("NinjaDeathSound").GetComponent<AudioSource>();
		if (GameManager.S.isMuted) {
			audio.mute = true;
		}
		anim = GetComponent<Animator>();
		throwing = ThrowState.STAGE0;
		combat = GameObject.Find("CombatController").GetComponent<CombatController>();
		++combat.NinjaCount;

		current_turn = 0;

		turnCounter = new List<TurnCounter>();
		float spacing = 0;
		if (max_turns % 2 == 0) spacing = (spaceOfCounters / (max_turns * 2)) * (max_turns - 1);
		else spacing = (spaceOfCounters / max_turns) * (max_turns / 2);
		for (int i = 0; i < max_turns; ++i)
		{
			float num = i * (spaceOfCounters / max_turns);
			Vector3 spot = new Vector3(transform.position.x + num - spacing, transform.position.y + 2f, transform.position.z);
			GameObject go = Instantiate(turnCounterObject, spot, Quaternion.identity) as GameObject;
			turnCounter.Add(go.GetComponent<TurnCounter>());
			go.transform.parent = transform;
			if (i == max_turns - 2) go.GetComponent<TurnCounter>().onColor = Color.yellow;
			if (i == max_turns - 1) go.GetComponent<TurnCounter>().onColor = Color.red;
		}

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
			if (current_turn < max_turns)
				CombatController.S.NinjaDoneMoving();
			NextState();
		} 
		TurnOnTurnCounter (current_turn + 1);
	}

	public void StartAttack()
	{
		NextState();
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
			audio.PlayOneShot(audio.clip);
			Destroy (this.gameObject);
			--combat.NinjaCount;
			if (combat.NinjaCount == 0) {
				combat.ItemDropPosition = transform.position;
			}
		
		}
	}

	void NextAnimationState()
	{
		if (max_turns == 3 && animationState == 1) {
			animationState += 2; //skip one animation
		}
		else if (max_turns > 4 && current_turn > 3 && current_turn != max_turns) 
			print ("Do Nothing");
		else {
			animationState++;
		}
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
			if (max_turns == 3)
				throwing = ThrowState.STAGE3;
			else 
				throwing = ThrowState.STAGE2;
			break;
		case ThrowState.STAGE2:
			throwing = ThrowState.STAGE3;
			break;
		case ThrowState.STAGE3:
			if (current_turn == max_turns)
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

