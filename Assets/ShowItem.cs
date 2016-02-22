using UnityEngine;
using System.Collections;

public class ShowItem : MonoBehaviour {

	public GameObject item;
	public GameObject dialogBox;

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}

	void OnTriggerEnter2D(Collider2D other) {
		item.SetActive (true);
	}

	void OnTriggerExit2D(Collider2D other) {
		item.SetActive (false);
		dialogBox.SetActive (false);
	}

	void Button() {
		dialogBox.SetActive (!dialogBox.activeInHierarchy);

	}
}
