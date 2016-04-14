using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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

		List<string> halls = new List<string>{ "hall1", "hall2", "hall3", "hall4", "hall5", "hall6" };
		if (halls.Contains (loadWhichLevel)) {
			GameManager.S.backPos.x = 0f;
			PlayerPrefs.SetFloat("BackX", 0);
			GameManager.S.currentItem = "";
		}

		Application.LoadLevel (loadWhichLevel);
    }



}



