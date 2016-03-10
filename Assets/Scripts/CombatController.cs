using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum TurnState
{
	TOMMY,
	TOMMYEND,
	ENEMY,
	ENEMYSTART,
}

//CONTROLLS THE FLOW OF BATTLE (e.g. who's turn it is)
public class CombatController : MonoBehaviour {

	public TurnState _turn;
	public TurnState turn



	{
		get {return _turn;}
		set 
		{
			if (_turn == value) return;
			_turn = value;
			switch(_turn)
			{
			case TurnState.TOMMY:
				break;
			case TurnState.TOMMYEND:
				break;
			case TurnState.ENEMY:
				break;
			case TurnState.ENEMYSTART:
				break;
			default:
				break;
			}
		}
	}

	public List<Ninja> ninjaList;

	public int NinjaCount; 

	void Awake () {
		TommysTurn();
	}
	// Use this for initialization
	void Start () {
		NinjaCount = ninjaList.Count;
	}
	
	// Update is called once per frame
	void Update () {

		if (turn == TurnState.TOMMYEND && NinjasDoneMoving())
		{
			EnemysTurn();
		}
		else if (turn == TurnState.ENEMY && NinjasDoneMoving())
		{
			TommysTurn();
		}
		else if (turn == TurnState.ENEMYSTART) 
		{
			turn = TurnState.ENEMY;
		}

		//remainingNinjas ();

		if (NinjaCount == 0) 
		{	
			if (turn == TurnState.ENEMY) {
				Application.LoadLevel (1);
			}
		}
			
	}

	public void TommysTurn()
	{
		turn = TurnState.TOMMY;
	}
	public void TommyEnd()
	{
		turn = TurnState.TOMMYEND;
	}
	public void EnemysTurn()
	{
		turn = TurnState.ENEMYSTART;
	}


	bool NinjasDoneMoving()
	{
		for (int i = 0; i < ninjaList.Count; i += 1)
		{
			if (ninjaList[i].jumpState != JumpState.GROUNDED)
			{
				return false;
			}
		}
		return true;
	}

	/*int remainingNinjas()
	{
		for (int i = 0; i < ninjaList.Count; i += 1) {
			if (!ninjaList[i].activeInHierarchy) {
				--NinjaCount;
			}
		}
	}	*/
}
