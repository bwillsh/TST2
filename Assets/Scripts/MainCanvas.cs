using UnityEngine;
using System.Collections;

public class MainCanvas : MonoBehaviour {
	public Canvas verbs; 

	public void deactivate(){
		gameObject.SetActive (false);
	}

	public void activate(){
		gameObject.SetActive (true);
	}
}
