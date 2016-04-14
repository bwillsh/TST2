using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour {

	TextMesh tm;
	public string loadWhichLevel;
	public GameObject star;

	// Use this for initialization
	void Start () {
		tm = GetComponentInChildren<TextMesh>(true);
	}

	// Update is called once per frame
	void Update () {
		if (GameManager.S.levelsBeaten.Contains (loadWhichLevel))
			star.SetActive (true);
		else
			star.SetActive (false);
	}


    public void Button()
    {
		MovementController.S.disableControls = true;
		tm.gameObject.SetActive (true);
        print("Loading Level " + loadWhichLevel);
		GameManager.S.currentHall = loadWhichLevel[1] - '0';
		GameManager.S.level = Application.loadedLevel;
		Application.LoadLevel (loadWhichLevel);
    }



}



