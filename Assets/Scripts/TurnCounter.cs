using UnityEngine;
using System.Collections;

public class TurnCounter : MonoBehaviour {

	public Color onColor;
	public Color offColor;
	private Material	mat;

	// Use this for initialization
	void Awake () {
		mat = GetComponent<Renderer>().material;
		mat.color = offColor;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void TurnOn()
	{
		mat.color = onColor;
	}

	public void TurnOff()
	{
		mat.color = offColor;
	}
}
