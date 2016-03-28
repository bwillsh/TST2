using UnityEngine;
using System.Collections;

public class Wheelchair : MonoBehaviour {

	public static Wheelchair S;

	private HealthBar	health;

	// Use this for initialization
	void Awake()
	{
		S = this;
	}

	void Start () {
		health = GameObject.Find ("Bar").GetComponent<HealthBar>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D coll)
	{

		if (coll.gameObject.tag == "Ninja" || coll.gameObject.tag == "NinjaStar")
		{
			health.lowerHealth(.2f);
		}
	}
}
