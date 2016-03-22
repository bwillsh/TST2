using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;



public class Launcher : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update() { 
		if (Input.GetMouseButtonDown (0)){
			Debug.Log ("Pressed left click.");
			SceneManager.LoadScene ("Menu");
		}

        //Need this scene and object so GameManagers don't multiply
        //Can't start in a scene that you will later load
		//StartCoroutine(LoadAfterDelay());
	}

	IEnumerator LoadAfterDelay() {
		// oh yeah take in that splash screen

		yield return new WaitForSeconds (1);
		SceneManager.LoadScene("Menu");
	}
}
