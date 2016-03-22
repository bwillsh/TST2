using UnityEngine;
using System.Collections;

public class verbCanvas : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void activate(){
		gameObject.SetActive (true);
	}
	public void deactivate(){
		gameObject.SetActive (false);
	}
}
