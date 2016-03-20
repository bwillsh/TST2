using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class throwNinja : MonoBehaviour {

	public int current_turn;
	private CombatController combat; //the combat script that keeps track of the combat flow
	public ParticleSystem	explosion; //the particle system that makes the ninja explode
	public List<TurnCounter> turnCounter;
	public int max_turns;
	private ninjaStar star;
	public Transform  star_pos;
	public Transform 	wheelchair_pos;
	private Vector3 original_star_pos;
	public float speed;
	private bool move_done;
	private HealthBar	health;




	// Use this for initialization
	void Start () {
		
		combat = GameObject.Find("CombatController").GetComponent<CombatController>();
		++combat.NinjaCount;

		current_turn = 0;
		health = GameObject.Find ("Bar").GetComponent<HealthBar>();

		//get star component
		star = GetComponentInChildren<ninjaStar> (true);
		star_pos = star.transform;
		original_star_pos = star_pos.position;

		//get wheelchair component
		wheelchair_pos = GameObject.Find("TommyCombat").transform;
	


		foreach (Transform child in transform)
		{
			if (child.GetComponent<TurnCounter>() != null)
			{
				turnCounter.Add(child.GetComponent<TurnCounter>());
			}
		}
			


	}
	
	// Update is called once per frame
	void Update () {
		if (current_turn == max_turns) {
			moveStar ();
			if (move_done) {
				current_turn = 0;
			}
		} 
		else if(combat.turn ==  TurnState.ENEMYSTART) {
			current_turn++;
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
	void moveStar(){
		move_done = false;				
		star.activate();
		float step = speed * Time.deltaTime;
		star.transform.position = Vector3.MoveTowards (star_pos.position, wheelchair_pos.position, step);
		if (star.transform.position == wheelchair_pos.position) {
			move_done = true;
			Instantiate(explosion, wheelchair_pos.position, Quaternion.identity);
			star.deactivate ();
			star.transform.position = original_star_pos;
			health.lowerHealth(0.2f);
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

}

