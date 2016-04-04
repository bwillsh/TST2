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
		Application.LoadLevel ("OpenCutscene");
	}

	public void Endless() {
		Application.LoadLevel ("endlessStart");
	}

	public void Select(){
		Application.LoadLevel ("Select_Hall1");
	}
}
