using UnityEngine;
using System.Collections;

public class Portal : MonoBehaviour {


    public int level;
    public Vector2 location = new Vector2(0,2.75f);
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Button()
    {
        print("PORTAL");
        GameManager.S.level = level;
        GameManager.S.backPos = location;
        Application.LoadLevel(level);
    }
}
