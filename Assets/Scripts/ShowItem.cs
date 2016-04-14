using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ShowItem : MonoBehaviour {

	public GameObject item;
	public GameObject unlockParticle;

	public string itemName;
	public Button arrow;
	public MovementController m;

	public int hall;

//	bool beaten = false;

	void Start() {
		GameManager.S.currentHall = hall;
//		beaten = GameManager.S.IsHallBeaten();
//		if (beaten) {
//			gameObject.tag = "Untagged";
//		}
	}

	void OnTriggerEnter2D(Collider2D other) {
//		if (!beaten) {
			item.SetActive (true);
			m.StopRight ();
			arrow.gameObject.SetActive (false);
//		}
	}

	void OnTriggerExit2D(Collider2D other) {
		item.SetActive (false);
		arrow.gameObject.SetActive (true);
	}

	void Button() {
		if (GameManager.S.currentItem == itemName) {
			print ("Unlock");
			item.SetActive (false);
			arrow.gameObject.SetActive (true);
			Instantiate (unlockParticle, transform.position, transform.rotation);
		} else {
			print ("Need item to unlock");
		}
	}
}
