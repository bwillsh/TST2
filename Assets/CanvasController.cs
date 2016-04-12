using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class CanvasController : MonoBehaviour {

	public int num_answers;
	public int count;
	public Canvas well_done;
	public Canvas out_of_time;
	public string loaded_level;
	private 

	// Use this for initialization
	void Start () {
		count = 0;
	}

	public float timeLeft = 30.0f;
	public Text timer;
	public bool lost = false;

	void Update(){		
		StartCoroutine ("startTimer");
		if (count == num_answers && !lost) {
			StopCoroutine ("startTimer");
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

	IEnumerator startTimer(){
		yield return new WaitForSeconds (8);
		timer.gameObject.SetActive (true);
		timeLeft -= Time.deltaTime;
		timer.text = "" + Mathf.Round(timeLeft);
		if(timeLeft < 0){
			lost = true;
			timer.text = "0";
			out_of_time.gameObject.SetActive (true);
			yield return new WaitForSeconds (3);
			Application.LoadLevel (loaded_level);
		}

	}


	public void increment(){
		count++;
	}
}
