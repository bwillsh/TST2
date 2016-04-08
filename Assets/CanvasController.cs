using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class CanvasController : MonoBehaviour {

	public int num_answers;
	public int count;
	public Canvas well_done;
	public string loaded_level;


	// Use this for initialization
	void Start () {
		count = 0;
	}



	void Update(){
		if (count == num_answers) {
			StartCoroutine (win ());
		}
	}

	IEnumerator win() {
		well_done.gameObject.SetActive (true);
		yield return new WaitForSeconds (3);
		if (!GameManager.S.levelsBeaten.Contains (Application.loadedLevelName)) {
			GameManager.S.levelsBeaten.Add (Application.loadedLevelName);
		}
		Application.LoadLevel (loaded_level);
	}

	public void increment(){
		count++;
	}
}
