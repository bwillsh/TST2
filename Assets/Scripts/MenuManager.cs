using UnityEngine;
using System.Collections;

public class MenuManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Play() {
		Application.LoadLevel (2);
	}

	public void Endless() {
		Application.LoadLevel ("EndlessLevel");
	}

	public void stix(){
		Application.LoadLevel ("symbolStix");
	}
}
