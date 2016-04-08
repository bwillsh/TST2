using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

    //what the background position is
    //Updated each frame you move
    public Vector2 backPos = new Vector2(0, 2.75f);
    public int level = 2;
    public string levelName = "null";
    public string loader = "null";
    public string realLevelName = "null";
	public string currentItem;
	public List<string> levelsBeaten;
    int numberOfLevelsBeaten = 0;
	public string currentHall;

    public static GameManager S;
    void Awake()
    {
        S = this;
        DontDestroyOnLoad(this.gameObject);
        Input.multiTouchEnabled = false;

        if (Application.loadedLevel != null)
        {
            loader = Application.loadedLevelName;
        }
        
        print("Saved Level: " + PlayerPrefs.GetString("Level"));
        backPos.x = PlayerPrefs.GetFloat("BackX");
        backPos.y = PlayerPrefs.GetFloat("BackY");
        currentItem = PlayerPrefs.GetString("Inv");

        levelsBeaten = new List<string>();
        for(int i = 1; i <= 8; i++)
        {
            string s = PlayerPrefs.GetString("Door" + i);
            if (s != null && s != "" && s != "null")
            {
                numberOfLevelsBeaten = i;
                levelsBeaten.Add(s);
            }
            else
            {
                i = 100;
            }
        }

        if(PlayerPrefs.GetString("Level") == null)
        {
            print("Uh Oh");
            backPos.x = 0;
            backPos.y = 2.75f;
            PlayerPrefs.SetFloat("BackX", 0);
            PlayerPrefs.SetFloat("BackY", 2.75f);
            PlayerPrefs.Save();
        }

        if (PlayerPrefs.GetString("Level") != "Menu" && Application.loadedLevelName == "Menu")
        {
            print("Loading new level");
            Application.LoadLevel(PlayerPrefs.GetString("Level"));
        }
    }
	// Use this for initialization
	void Start () {
        //vital for keeping the GameManager in each scene
       
        

    }

    // Update is called once per frame
    void Update()
    {

        if(Application.loadedLevelName != "" && Application.loadedLevelName != null )
        {
            PlayerPrefs.SetString("Level", Application.loadedLevelName);
            PlayerPrefs.SetFloat("BackX", backPos.x);
            PlayerPrefs.SetFloat("BackY", backPos.y);
            PlayerPrefs.SetString("Inv", currentItem);
            PlayerPrefs.Save();
        }
       
        
        if(Input.GetKeyDown(KeyCode.M))
        {
            NukeSaveData();
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            PlayerPrefs.SetString("Level", loader);
            PlayerPrefs.SetFloat("BackX", 0);
            PlayerPrefs.SetFloat("BackY", 2.75f);
            backPos = new Vector2(0, 2.75f);
            PlayerPrefs.Save();
            Application.LoadLevel(loader);
        }
        realLevelName = Application.loadedLevelName;
        levelName = PlayerPrefs.GetString("Level");
    }

	public bool IsHallBeaten() {
		switch (currentHall) {
		case "1":
			return levelsBeaten.Contains ("combat_Tutorial");
		case "2":
			return levelsBeaten.Contains ("combat_Beta_1") && levelsBeaten.Contains ("combat_Beta_2") && levelsBeaten.Contains ("combat_Beta_3");
		default:
			return false;
		}
	}

    public void BeatLevel(string s)
    {
        levelsBeaten.Add(s);
        numberOfLevelsBeaten++;
        PlayerPrefs.SetString("Door" + numberOfLevelsBeaten, s);
    }
		
    public void NukeSaveData()
    {
        PlayerPrefs.SetString("Level", "Menu");
        PlayerPrefs.SetFloat("BackX", 0);
        PlayerPrefs.SetFloat("BackY", 2.75f);
        currentItem = "";
        PlayerPrefs.SetString("Inv", "");
        levelsBeaten = new List<string>();
        numberOfLevelsBeaten = 0;
        for (int i = 1; i <= 8; i++)
        {
            PlayerPrefs.SetString("Door" + i, "null");
        }
        backPos = new Vector2(0, 2.75f);
        PlayerPrefs.Save();
        Application.LoadLevel("Menu");
    }


}
