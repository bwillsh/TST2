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
	public Sprite before, after;
	private SpriteRenderer sprend;

	void Start () {
		m = GameObject.Find ("MovementController").GetComponent<MovementController> ();
		sprend = transform.parent.GetComponent<SpriteRenderer>();
	}



	void OnTriggerEnter2D(Collider2D other) {
		item.SetActive (true);
		if (!GameManager.S.puzzlesBeaten.Contains (puzzle)) {
			m.StopRight ();
			arrow.gameObject.SetActive (false);
		}
	}

	void OnTriggerStay2D(Collider2D other) {
		if (GameManager.S.puzzlesBeaten.Contains (puzzle)) {
			checkmark.SetActive (true);
			sprend.sprite = after;
			arrow.gameObject.SetActive (true);
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