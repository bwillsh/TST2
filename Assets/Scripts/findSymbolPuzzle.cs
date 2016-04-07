using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class findSymbolPuzzle : MonoBehaviour {

	public Image i; 
	public Transform start;
	public Transform end;
	public float speed = 1.0F;
	private float startTime;
	private float journeyLength;
	public float targetScale = 0.1f;
	public float shrinkSpeed = 0.1f;
	public Canvas home;

	// Use this for initialization
	void Start () {
		
		startTime = Time.time;
		journeyLength = Vector3.Distance(start.position, end.position);
	}
	
	// Update is called once per frame
	void Update () {
		StartCoroutine (wait ());
	}

	IEnumerator wait() {
		yield return new WaitForSeconds (4);

		float distCovered = (Time.time - startTime) * speed;
		float fracJourney = distCovered / journeyLength;
		transform.position = Vector3.Lerp(start.position, end.position, fracJourney);
		transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(targetScale, targetScale, targetScale), Time.deltaTime*shrinkSpeed);
		yield return new WaitForSeconds (3);
		home.gameObject.SetActive (true);
	}


}
