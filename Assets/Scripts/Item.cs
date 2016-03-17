using UnityEngine;
using System.Collections;

public class Item : MonoBehaviour {

	public string item_name;

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
			Foot.S.hasItem = true;
			this.gameObject.transform.parent = Foot.S.transform;
			GetComponent<BoxCollider2D> ().enabled = false;
			GameManager.S.currentItem = item_name;
		}
	}
}
