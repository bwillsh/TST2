using UnityEngine;
using System.Collections;

public class Wheelchair : MonoBehaviour {

	public static Wheelchair S;

	private AudioSource injuredSound;
	private HealthBar	health;

	// Use this for initialization
	void Awake()
	{
		S = this;
	}

	void Start () {
		health = GameObject.Find ("Bar").GetComponent<HealthBar>();
		injuredSound = GameObject.Find("Punch").GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D coll)
	{

		if (coll.gameObject.tag == "Ninja" || coll.gameObject.tag == "NinjaStar")
		{
			health.lowerHealth(.2f);
			if (coll.gameObject.tag == "NinjaStar")
				injuredSound.pitch = .5f;
			else
				injuredSound.pitch = 1f;
			injuredSound.PlayOneShot(injuredSound.clip);
		}
	}
}
