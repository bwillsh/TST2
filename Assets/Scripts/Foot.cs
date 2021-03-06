﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//Keeps track of if there is currently input or not
public enum InputState
{
	INPUT,
	NOINPUT
}

//Keeps track of what part of the attack tommy's foot is in
public enum AttackState
{
	CHARGING,
	SHOOTING,
	SLOWING,
	RETURNING,
	NORMAL
}


public class Foot : MonoBehaviour {

	public static Foot S;

	void Awake()
	{
		S = this;
	}
	//sets up the inputState variable. 
	//can set inputState by inputState = InputState.INPUT (or another state)
	private InputState 	_inputState;
	public InputState	inputState
	{
		get {return _inputState;}
		set 
		{
			if (_inputState == value) return;
			_inputState = value;
			switch(_inputState)
			{
			case InputState.INPUT:
				break;
			case InputState.NOINPUT:
				break;
			default:
				break;
			}
		}
	}

	//sets up the attackState variable
	//can set attackState by attackState = AttackState.CHARGING (or another state)
	public AttackState 	_attackState;
	public AttackState	attackState
	{
		get {return _attackState;}
		set 
		{
			if (_attackState == value) return;
			_attackState = value;
			switch(_attackState)
			{
			case AttackState.NORMAL:
				if (hasItem)
				{
					print ("Loading level " + GameManager.S.level);
					Application.LoadLevel (GameManager.S.level);
				}
				currentShootSpeed = shootSpeed;
				break;
			case AttackState.CHARGING:
				originalTouchPoint = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,Input.mousePosition.y, -10.0f));
				break;
			case AttackState.SHOOTING:
				pop.PlayOneShot(pop.clip);
				originalShotPos = footPos;
				GetShotPathVertices();
				break;
			case AttackState.SLOWING:
				break;
			case AttackState.RETURNING:
				distanceTraveled = 0;
				if (ricochetPoints.Count > 0)
				{
					returnTarget = ricochetPoints[ricochetPoints.Count - 1];
				}
				else 
				{
					returnTarget.x = originalShotPos.x;
					returnTarget.y = originalShotPos.y;
					returnTarget.z = transform.position.z;
				}
				Vector3 dir = transform.position - returnTarget;
				float angle = Mathf.Atan2(dir.y,dir.x) * Mathf.Rad2Deg;
				transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

				break;
			default:
				break;
			}
		}
	}

	//CHARGING
	public float		maxChargeRadius; //how far back you can pull the foot before the charge maxes out
	public float		selectFootRadius; //how close you must click to the foot to grab it
	public Vector3		originalTouchPoint = Vector3.zero;

	//SHOOTING AND RETURNING
	public float		maxShotDistance; //the maximum distance the foot can travel
	public float		startSlowingDistance; //after how much distance should the foot start to decelerate
	public float		shotSpeedOriginal; //the original speed of the foot after firing it
	private float		shotSpeedCurrent; //the current speed of the foot
	public float		returnAccelRate; //how quickly the foot accelerates when returning to Tommy
	private Vector2		originalShotPos; //where the foot was positioned at the beginning of the shot
	private Vector3		attackAngle;
	private float		attackStrength; //the charge converted to a decimal [0, 1]
	private List<Vector3> ricochetPoints; //the positions where the foot ricochets
	private int			currentRicPoint;
	private Vector3		returnTarget;
	private float		distanceTraveled;
	public float		shootSpeed = 20;
	private float		currentShootSpeed = 20;

	//AIMING
	public GameObject	line; //the line direction indicator prefab
	private ShotPath	shotPath; //the line direction object
	private float		prevAngle = 0;

	//GENERAL
	private Rigidbody2D	rb; //the foot's rigidbody2D component
	private Vector2 	mousePos; //where the mouse is each frame
	private Vector2 	footPos; //where the foot is each fram
	private CombatController	combat;	//the script that keeps track of combat
	public LayerMask	collisionMask;
	private HealthBar	health;
	private AudioSource boing, crank, pop;

	//POWERUPS
	public string curPower = "";
	public int curPowerLength = -1;
	public bool newPower = false;
	public bool hasItem = false;
	// Use this for initialization
	void Start () 
	{
		boing = GameObject.Find("Boing").GetComponent<AudioSource>();
		crank = GameObject.Find("Crank").GetComponent<AudioSource>();
		pop = GameObject.Find("Pop").GetComponent<AudioSource>();

		//initialize states and variables
		inputState = InputState.NOINPUT;
		attackState = AttackState.NORMAL;
		rb = GetComponent<Rigidbody2D>();
		shotSpeedCurrent = shotSpeedOriginal;
		combat = GameObject.Find("CombatController").GetComponent<CombatController>();
		shotPath = line.GetComponent<ShotPath>();
		ricochetPoints = new List<Vector3>(); 
		health = GameObject.Find ("Bar").GetComponent<HealthBar>();
	}

	void GetInput ()
	{
		if (inputState == InputState.NOINPUT) //if there is no input
		{
			if (Input.GetMouseButtonDown(0)) //0 for left mouse button, 1 for right, 2 for middle
			{
				//if the mouse is now being pressed
				inputState = InputState.INPUT;

				UpdateMousePos();
				//if you are not attacking and are clicking within selectFootRadius
				if (attackState == AttackState.NORMAL/* && Vector2.Distance(mousePos, footPos) <= selectFootRadius*/)
				{
					attackState = AttackState.CHARGING;
				}
			}
		}

		//if there is input
		if (inputState == InputState.INPUT)
		{
			//update the mousePos variable
			UpdateMousePos();

			//if you release the left mouse button
			if (Input.GetMouseButtonUp(0))
			{
				//indicate there is no more input
				inputState = InputState.NOINPUT;

				//determine here if you should fire off the foot or not
				if (attackState == AttackState.CHARGING)
				{
					shotPath.HideLine();

					if (Vector2.Distance(originalTouchPoint, Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,Input.mousePosition.y, -10.0f))) < .01f)
					{
						//set back to normal
						attackState = AttackState.NORMAL;
						transform.rotation = Quaternion.identity;
					}
					else
					{
						//fire off the foot
						attackState = AttackState.SHOOTING;
					}
				}
			}
		}
	}



	void FixedUpdate () 
	{
		if (attackState == AttackState.NORMAL)
		{
		}
		
		else if (attackState == AttackState.CHARGING)
		{
			RotateFoot();
			CalculateAttackStrength();
			if (attackStrength > 0)
			{
				shotPath.Ricochet(transform.position, transform.right, maxShotDistance * attackStrength);
			}
			else
			{
				shotPath.HideLine();
			}

		}
		else if (attackState == AttackState.SHOOTING)
		{
			ShootFoot();
		}
		else if (attackState == AttackState.RETURNING)
		{
			ReturnFoot();
		}
		else if (attackState == AttackState.SLOWING)
		{
			SlowFoot();
		}
		RotationCheck();
	}

	void Update()
	{
		if (attackState == AttackState.SHOOTING || attackState == AttackState.SLOWING)
		{
			DistanceTraveled();
		}
		footPos.x = transform.position.x;
		footPos.y = transform.position.y;
		
		if (combat.turn == TurnState.TOMMY)
		{
			GetInput();
		}
			
	}

	//updates the mousePos variable so you always knows where the mouse is on the screen
	void UpdateMousePos()
	{
		Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,Input.mousePosition.y, -10.0f));
		mousePos = new Vector2 (mouseWorldPos.x, mouseWorldPos.y);
	}

	//rotates the foot so it is facing the opposite direction of the mouse.
	//Some of this code is taken from online
	void RotateFoot() 
	{

		Vector3 diff = Camera.main.ScreenToWorldPoint(Input.mousePosition) - originalTouchPoint;
		diff.Normalize();
		
		float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg - 180;
		if (Mathf.Abs(rot_z - prevAngle) > 30)
		{
			prevAngle = rot_z;
			crank.pitch = Random.Range(.7f, .9f);
			crank.PlayOneShot(crank.clip);
		}
		transform.rotation = Quaternion.Euler(0f, 0f, rot_z);

	}

	//sets attackStrength to a float 0-1 to represent to attack strength, 0 being no power, 1 being full power
	void CalculateAttackStrength() 
	{
		Vector2 originalTouchPoint2D = new Vector2(originalTouchPoint.x, originalTouchPoint.y);
		float distanceFromTouchToFoot = Vector2.Distance(mousePos, originalTouchPoint2D);

		float strength = distanceFromTouchToFoot > maxChargeRadius ? 1 : distanceFromTouchToFoot / maxChargeRadius;
		attackStrength = strength;

	}

	//once the foot has reached a certain point the state will change to SLOWING
	void ShootFoot() 
	{
		if (distanceTraveled >= attackStrength * maxShotDistance * startSlowingDistance / 2)
		{
			attackState = AttackState.SLOWING;
			return;
		}
		if (currentRicPoint < 0) currentRicPoint = 0;
		if (currentRicPoint > ricochetPoints.Count - 1) currentRicPoint = ricochetPoints.Count - 1;
		if (transform.position == ricochetPoints[currentRicPoint])
		{
			if (currentRicPoint != 0) 
			{
				boing.pitch = Random.Range(.6f, 1.4f);
				boing.PlayOneShot(boing.clip);
			}
			currentRicPoint++;
			if (currentRicPoint < ricochetPoints.Count)
			{
				returnTarget = ricochetPoints[currentRicPoint];
				Vector3 dir = transform.position - returnTarget;
				float angle = Mathf.Atan2(dir.y,dir.x) * Mathf.Rad2Deg - 180;
				transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
			}
		}
		if (currentRicPoint < 0) currentRicPoint = 0;
		if (currentRicPoint > ricochetPoints.Count - 1) currentRicPoint = ricochetPoints.Count - 1;
		transform.position = Vector3.MoveTowards(transform.position, ricochetPoints[currentRicPoint], currentShootSpeed * Time.deltaTime);

	}

	//every frame subtracts from the foot's velocity until it hits near zero (Mathf.Epsilon)
	void SlowFoot()
	{
		//need better slowing equation
		if (currentShootSpeed > shotSpeedOriginal / 1.3f)
			currentShootSpeed -= .1f;
		if (transform.position == ricochetPoints[ricochetPoints.Count - 1])
		{
			attackState = AttackState.RETURNING;
			return;
		}
		if (transform.position == ricochetPoints[currentRicPoint])
		{
			if (currentRicPoint != 0) 
			{
				boing.pitch = Random.Range(.6f, 1.4f);
				boing.PlayOneShot(boing.clip);
			}
			currentRicPoint++;
			returnTarget = ricochetPoints[currentRicPoint];
			Vector3 dir = transform.position - returnTarget;
			float angle = Mathf.Atan2(dir.y,dir.x) * Mathf.Rad2Deg - 180;
			transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
		}
		transform.position = Vector3.MoveTowards(transform.position, ricochetPoints[currentRicPoint], currentShootSpeed * Time.deltaTime);
	}

	void ReturnFoot()
	{
		if (ricochetPoints.Count > 0)
		{
			if ((transform.position - ricochetPoints[ricochetPoints.Count - 1]).magnitude < .05f)
			{
				ricochetPoints.RemoveAt(ricochetPoints.Count - 1);
				if (ricochetPoints.Count > 0)
				{
					returnTarget = ricochetPoints[ricochetPoints.Count - 1];
					Vector3 dir = transform.position - returnTarget;
					float angle = Mathf.Atan2(dir.y,dir.x) * Mathf.Rad2Deg;
					transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
					boing.pitch = Random.Range(.6f, 1.4f);
					boing.PlayOneShot(boing.clip);
				}
				else 
				{
					returnTarget.x = originalShotPos.x;
					returnTarget.y = originalShotPos.y;
					returnTarget.z = transform.position.z;
					Vector3 dir = transform.position - returnTarget;
					float angle = Mathf.Atan2(dir.y,dir.x) * Mathf.Rad2Deg;
					transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
				}
			}
		}

		shotSpeedCurrent = Mathf.Lerp (shotSpeedCurrent, shotSpeedOriginal * 2, returnAccelRate * Time.deltaTime);
		transform.position = Vector3.MoveTowards(transform.position, returnTarget, shotSpeedCurrent * Time.deltaTime);
	

		//once the foot is back to its original position
		if (Vector2.Distance(footPos, originalShotPos) < .1f)
		{

			Vector3 returnPosition = new Vector3(originalShotPos.x, originalShotPos.y, -4);
			transform.position = returnPosition;

			//reset velocity
			rb.velocity = Vector2.zero;
			//reset foot rotation
			transform.rotation = Quaternion.Euler(0, 0, 90);
			//reset shot speed
			shotSpeedCurrent = shotSpeedOriginal;
			//reset Tommy to his neutral state
			attackState = AttackState.NORMAL;

			//run Power up code
			ManagePowerUp();

			//tell combat controller that tommy has finished his attack
			combat.TommyEnd();
		}
	}


	//set the size of the shot indicator
	void SetShotPath()
	{
		shotPath.SetStartPos(transform.position);
		shotPath.SetEndPos(transform.position + (transform.right * maxShotDistance * attackStrength));
	}

	void OnTriggerEnter2D(Collider2D coll)
	{
		if (coll.gameObject.layer == 8 && (attackState == AttackState.SHOOTING || attackState == AttackState.SLOWING))
		{
			if (coll.gameObject.tag == "DamageWall")
			{
				health.lowerHealth(0.1f);
			}
		}
	}

	//makes sure there is no rotation in the y or x directions
	void RotationCheck()
	{
		transform.eulerAngles = new Vector3(0, 0, transform.eulerAngles.z);
	}

	void DistanceTraveled()
	{
		Vector2 newPosition = new Vector2(transform.position.x, transform.position.y);
		distanceTraveled += Vector2.Distance(newPosition, footPos);
	}

	void GetShotPathVertices()
	{
		currentRicPoint = 0;
		ricochetPoints.Clear();
		for (int i = 0; i < shotPath.vertices.Count; ++i)
		{
			ricochetPoints.Add(shotPath.vertices[i]);
		}
	}


	void ManagePowerUp()
	{
		float bigFootSize = 2;
		curPowerLength--;

		if (newPower) //Handles the turning on of powerUps
		{
			switch (curPower) {
			case "PlaceHolder": 
				print ("place holder power on");
				break;
			case "BigFoot":
				transform.localScale *= bigFootSize;
				break;
			}

			newPower = false;
		}

		if (curPowerLength >= 0) //Handles ongoing effects, might not need
		{
			switch (curPower) {
			case "PlaceHolder": 
				print ("have placeholder power this turn");
				break;
			}
		} 
		else if (curPowerLength == -1) //Handles the turning off of powerUps
		{
			switch (curPower) {
			case "PlaceHolder": 
				print ("place holder power off");
				break;
			case "BigFoot":
				transform.localScale /= bigFootSize;
				break;
			}
		}
	}

}
