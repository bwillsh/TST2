using UnityEngine;
using System.Collections;

public class Storm : MonoBehaviour {

	SpriteRenderer myRenderer;
	public Sprite clear;
	public Sprite storm;
	public Sprite lightning;
	public GameObject lightningparticle;

	// Use this for initialization
	void Start () {
		myRenderer = GetComponentInParent<SpriteRenderer> ();
//		clear = Resources.Load<Sprite>("Outside");
//		storm = Resources.Load<Sprite>("OutsideStorm");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D coll) {
		print ("Enter");
		PlayerController.S.Move (false);
		myRenderer.sprite = storm;
		StartCoroutine(Lightning());
	}

	IEnumerator Lightning() {
		yield return new WaitForSeconds(1);
		myRenderer.sprite = lightning;
		print ("Zap1");
		Instantiate (lightningparticle, transform.position, transform.rotation);
		yield return new WaitForSeconds(1);
		myRenderer.sprite = storm;
		yield return new WaitForSeconds(1);
		myRenderer.sprite = lightning;
		print ("Zap2");
		Instantiate (lightningparticle, transform.position, transform.rotation);
		yield return new WaitForSeconds(1);
		myRenderer.sprite = storm;
		Application.LoadLevel ("overWorld_2_ben");
	}
}
