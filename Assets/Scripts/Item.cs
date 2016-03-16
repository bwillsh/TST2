using UnityEngine;
using System.Collections;

public class Item : MonoBehaviour {

	public string name;

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
			GameManager.S.AddItem (name);
			print ("Added item to inventory");
//			Destroy (this.gameObject);
			Application.LoadLevel (GameManager.S.level);
		}
	}
}
