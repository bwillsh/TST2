using UnityEngine;
using System.Collections;

public class ShowItem : MonoBehaviour {

	public GameObject item;
	public GameObject dialogBox;
	private bool inDialog = false;
	private Vector2 mousePos;

	float squareSize = 1.5f;
	Vector2 buttonPos = new Vector2( -.14f, -3 );

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

		UpdateMousePos();

		if (inDialog && Input.GetMouseButtonDown (0)) {
			if (mousePos.x >= buttonPos.x - squareSize && mousePos.x <= buttonPos.x + squareSize
			   && mousePos.y >= buttonPos.y - squareSize && mousePos.y <= buttonPos.y + squareSize) { //Math to check to see if the mouse is inside a hit box for a button
				// button
			} else {
				print ("LateUpdate");
				dialogBox.SetActive (false);
				inDialog = false;
			}
		}
	}

	void UpdateMousePos()
	{
		Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -10.0f));
		mousePos = new Vector2(mouseWorldPos.x, mouseWorldPos.y);
	}

	void OnTriggerEnter2D(Collider2D other) {
		item.SetActive (true);
	}

	void OnTriggerExit2D(Collider2D other) {
		item.SetActive (false);
		dialogBox.SetActive (false);
	}

	void Button() {
		print ("Opening dialog");
		dialogBox.SetActive (!dialogBox.activeInHierarchy);
		inDialog = dialogBox.activeInHierarchy;
	}
}
