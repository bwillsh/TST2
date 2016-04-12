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

	public static CombatController S;

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

	public int NinjaCount;

    public bool generatedLevel = false;

    public GameObject ItemDrop;
	public Vector3 ItemDropPosition;
	private GameObject	ceiling;
	public float	ceilingHeight;
	private int		ninjasReady = 0;
	private int		ninjasStunned = 0;
	public List<Ninja>	ninjaList;
	private AudioSource	music;

    void Awake () {
		TommysTurn();
		S = this;
		Input.multiTouchEnabled = false;
		music = GameObject.Find("Music").GetComponent<AudioSource>();
		if (!GameManager.S.isMuted) {
			music.Play();
		}
	}
	// Use this for initialization
	void Start () {
        if(generatedLevel)
        {
            LevelGen.S.GenLevel();
        }

		ceiling = GameObject.Find("Ceiling");
		ceilingHeight = ceiling.GetComponent<BoxCollider2D>().bounds.min.y;
	}
	
	// Update is called once per frame
	void Update () {

		if (turn == TurnState.TOMMYEND)
		{
			EnemysTurn();
		}
		else if (turn == TurnState.ENEMY)
		{
			if (ninjasReady + ninjasStunned == NinjaCount)
			{
				ninjasReady = 0;
				TommysTurn();
			}
		}
		else if (turn == TurnState.ENEMYSTART) 
		{
			turn = TurnState.ENEMY;
		}

		if (NinjaCount == 0) 
		{	
			if (turn == TurnState.ENEMY) {

				if (generatedLevel)
					Application.LoadLevel (Application.loadedLevel);
				else {
					if (!GameManager.S.levelsBeaten.Contains (Application.loadedLevelName)) {
                        //GameManager.S.levelsBeaten.Add (Application.loadedLevelName);
						print("adding level to beaten");
						GameManager.S.BeatLevel(Application.loadedLevelName);
					}
					if (GameManager.S.IsHallBeaten () && ItemDrop != null) {
						print ("dropping item");
						Instantiate (ItemDrop, ItemDropPosition, Quaternion.identity);
					} else {
						print ("Loading level " + GameManager.S.level);
						Application.LoadLevel (GameManager.S.level);
					}
				}
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
		
	public void NinjaDoneMoving()
	{
		++ninjasReady;
	}
	public void NinjaStunned()
	{
		++ninjasStunned;
	}
	public void NinjaUnStunned()
	{
		--ninjasStunned;
	}
}
