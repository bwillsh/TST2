using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour {


	public int loadWhichLevel;
	// Use this for initialization
	void Start () {
	}

	// Update is called once per frame
	void Update () {
	}


    public void Button()
    {
		GameManager.S.level = Application.loadedLevel;
		Application.LoadLevel (loadWhichLevel);
    }



}



