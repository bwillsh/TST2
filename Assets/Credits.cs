using UnityEngine;
using System.Collections;

public class Credits : MonoBehaviour {

	public string returnToScene;
	private BoxCollider2D col;

	// Use this for initialization
	void Start () {
		col = GetComponent<BoxCollider2D>();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown(0))
		{
			Application.LoadLevel(returnToScene);
		}
	}
}
