using UnityEngine;
using System.Collections;

public class CutsceneController : MonoBehaviour {

	public Transform target;
	public float movespeed = 0.5f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		float step = movespeed * Time.deltaTime;
		transform.position = Vector3.MoveTowards(transform.position, target.position, step);
	}
}
