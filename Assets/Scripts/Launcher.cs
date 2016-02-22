using UnityEngine;
using System.Collections;

public class Launcher : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update() { 

        //Need this scene and object so GameManagers don't multiply
        //Can't start in a scene that you will later load
		StartCoroutine(LoadAfterDelay());
	}

	IEnumerator LoadAfterDelay() {
		// oh yeah take in that splash screen
		yield return new WaitForSeconds (2);
		Application.LoadLevel(1);
	}
}
