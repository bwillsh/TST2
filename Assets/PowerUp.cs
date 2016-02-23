using UnityEngine;
using System.Collections;

public class PowerUp : MonoBehaviour {

	public string power = "PlaceHolder";
	public int length = 1;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D (Collider2D coll)
	{
		if (coll.gameObject.tag == "Foot")
		{
			Foot.S.curPower = power;
			Foot.S.curPowerLength = length;
			Foot.S.newPower = true;
			gameObject.SetActive (false);
		}

	}
}
