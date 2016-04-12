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
		if (GameManager.S.isMuted) {
			injuredSound.mute = true;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D coll)
	{
		if (coll.gameObject.tag == "Ninja")
		{
			Ninja n = coll.gameObject.GetComponent<Ninja>();
			if (n != null)
			{
				if (n.canDealDamage)
				{
					n.canDealDamage = false;
					health.lowerHealth(.2f);
				}
				
			}
			else
			{
				tankNinja tn = coll.gameObject.GetComponent<tankNinja>();
				if (tn.canDealDamage)
				{
					tn.canDealDamage = false;
					health.lowerHealth(.3f);
				}
			}
			injuredSound.pitch = 1f;
			injuredSound.PlayOneShot(injuredSound.clip);
		}
		if (coll.gameObject.tag == "NinjaStar")
		{
			health.lowerHealth(.2f);
			injuredSound.pitch = .5f;
			injuredSound.PlayOneShot(injuredSound.clip);
		}
	}
}
