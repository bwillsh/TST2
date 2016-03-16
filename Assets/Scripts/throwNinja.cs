using UnityEngine;
using System.Collections;

public class throwNinja : MonoBehaviour {

	public int turns_left;
	private CombatController combat; //the combat script that keeps track of the combat flow
	public ParticleSystem	explosion; //the particle system that makes the ninja explode
	private Foot			foot;

	// Use this for initialization
	void Start () {


		int starting_turns = turns_left;
		foot = GameObject.Find ("Foot").GetComponent<Foot>();

		//access variables in combat controller
		combat = GameObject.Find("CombatController").GetComponent<CombatController>();

	}
	
	// Update is called once per frame
	void Update () {
		if (combat.turn == TurnState.ENEMYSTART)
		{
			turns_left--;
		}
	}
}
