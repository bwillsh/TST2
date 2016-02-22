using UnityEngine;
using System.Collections;

public class HealthBar : MonoBehaviour {

	float health = 1;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void lowerHealth(float n)
	{
		health -= n;
		if (health < 0)
		{
			health = 0;
		}
	}
}
