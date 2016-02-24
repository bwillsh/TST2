using UnityEngine;
using System.Collections;

public class Wheelchair : MonoBehaviour {

	private HealthBar	health;

	// Use this for initialization
	void Start () {
		health = GameObject.Find ("Bar").GetComponent<HealthBar>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D coll)
	{
		if (coll.gameObject.tag == "Ninja")
		{
			health.lowerHealth(0.2f);
		}
	}
}
