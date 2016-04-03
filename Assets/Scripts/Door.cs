using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour {

	TextMesh tm;
	public string loadWhichLevel;
	// Use this for initialization
	void Start () {
		tm = GetComponentInChildren<TextMesh>(true);
	}

	// Update is called once per frame
	void Update () {
	}


    public void Button()
    {
		MovementController.S.disableControls = true;
		tm.gameObject.SetActive (true);
        print("Loading Level " + loadWhichLevel);
		GameManager.S.level = Application.loadedLevel;
		Application.LoadLevel (loadWhichLevel);
    }



}



