using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class symbolstixTeacher : MonoBehaviour {

	public GameObject item;
	public string puzzle;
	public Button arrow;
	public MovementController m;
	public bool beaten = false;
	public GameObject buttons;
	public GameObject checkmark;

	void Start () {
		m = GameObject.Find ("MovementController").GetComponent<MovementController> ();
	}



	void OnTriggerEnter2D(Collider2D other) {
		if (!GameManager.S.levelsBeaten.Contains (puzzle)) {
			item.SetActive (true);
			m.StopRight ();
			arrow.gameObject.SetActive (false);
		}
	}

	void OnTriggerStay2D(Collider2D other) {
		if (GameManager.S.levelsBeaten.Contains (puzzle)) {
			checkmark.SetActive (true);
		}
	}
			

	void OnTriggerExit2D(Collider2D other) {
		item.SetActive (false);
		arrow.gameObject.SetActive (true);
		//		dialogBox.SetActive (false);
	}

	void Button() {
		item.SetActive (false);
		buttons.SetActive (false);
		Application.LoadLevel (puzzle);
	}


}