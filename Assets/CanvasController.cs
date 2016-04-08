using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class CanvasController : MonoBehaviour {

	public int num_answers;
	public int count;
	public Canvas well_done;

	// Use this for initialization
	void Start () {
		count = 0;
	}



	void Update(){
		if (count == num_answers) {
			StartCoroutine (wait ());
		}
	}

	IEnumerator wait() {
		well_done.gameObject.SetActive (true);
		yield return new WaitForSeconds (3);
		gameObject.SetActive (false);
	}

	public void increment(){
		count++;
	}
}
