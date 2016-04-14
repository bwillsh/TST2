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
    public List<string> puzzlesBeaten;
    int numberOfLevelsBeaten = 0;

	public int currentHall;

    int numberOfPuzzlesBeaten = 0;
	public bool isMuted = false;
	public bool isTimed = true;


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
		isMuted = (PlayerPrefs.GetInt ("Mute") == 1);
		isTimed = (PlayerPrefs.GetInt ("Timer") == 1);

        levelsBeaten = new List<string>();
        for (int i = 1; i <= 36; i++)
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

        puzzlesBeaten = new List<string>();
        for (int i = 1; i <= 18; i++)
        {
            string s = PlayerPrefs.GetString("Puzzle" + i);
            if (s != null && s != "" && s != "null")
            {
                numberOfPuzzlesBeaten = i;
                puzzlesBeaten.Add(s);
            }
            else
            {
                i = 100;
            }
        }

        if (PlayerPrefs.GetString("Level") == null)
        {
            print("Uh Oh");
            backPos.x = 0;
            backPos.y = 2.75f;
            PlayerPrefs.SetFloat("BackX", 0);
            PlayerPrefs.SetFloat("BackY", 2.75f);
            PlayerPrefs.Save();
        }

       /* if (PlayerPrefs.GetString("Level") != "Menu" && Application.loadedLevelName == "Menu")
        {
            print("Loading new level");
            Application.LoadLevel(PlayerPrefs.GetString("Level"));
        } */
    }
	// Use this for initialization
	void Start () {
        //vital for keeping the GameManager in each scene
       
        

    }

    // Update is called once per frame
    void Update()
    {

        if(Application.loadedLevelName == "hall1" || Application.loadedLevelName == "hall2" || 
            Application.loadedLevelName == "hall3" || Application.loadedLevelName == "hall4" || 
            Application.loadedLevelName == "hall5" || Application.loadedLevelName == "hall6" )
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
        /*switch (currentHall) {
		case "1":
			return levelsBeaten.Contains ("combat_Tutorial");
		case "2":
			return levelsBeaten.Contains ("combat_Beta_1") && levelsBeaten.Contains ("combat_Beta_2") && levelsBeaten.Contains ("combat_Beta_3");
		default:
			return false;
		}*/
        bool good = true;
        for(int i = 1; i <=6; i++)
        {
            if(!levelsBeaten.Contains("h" + currentHall + "d" + i))
            {
                good = false;
            }
        }
        return good;
	}

    public void BeatLevel(string s)
    {
        levelsBeaten.Add(s);
        numberOfLevelsBeaten++;
        PlayerPrefs.SetString("Door" + numberOfLevelsBeaten, s);
    }

    public void BeatPuzzle(string s)
    {
        puzzlesBeaten.Add(s);
        numberOfPuzzlesBeaten++;
        PlayerPrefs.SetString("Puzzle" + numberOfPuzzlesBeaten, s);
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
        for (int i = 1; i <= 36; i++)
        {
            PlayerPrefs.SetString("Door" + i, "null");
        }
		puzzlesBeaten = new List<string>();
        numberOfPuzzlesBeaten = 0;
        for (int i = 1; i <= 20; i++)
        {
            PlayerPrefs.SetString("Puzzle" + i, "null");
        }
        backPos = new Vector2(0, 2.75f);
        PlayerPrefs.Save();
        Application.LoadLevel("Menu");
    }

	public void addPuzzleLevel(string level)
	{
		puzzlesBeaten.Add(level);
		for (int i = 0 ; i < puzzlesBeaten.Count; ++i)
		{
			print (puzzlesBeaten[i]);
		}
        numberOfPuzzlesBeaten++;
        PlayerPrefs.SetString("Puzzle" + numberOfPuzzlesBeaten, level);
    }
}
