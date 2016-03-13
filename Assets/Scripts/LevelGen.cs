using UnityEngine;
using System.Collections;

public class LevelGen : MonoBehaviour {

    CombatController combat;

    public int numWalls = 1;

    public GameObject wall;
    public GameObject ninja;

    public Vector2 max = new Vector2(18.05f,13.4f);
    public Vector2 min = new Vector2(-8f, -3.1f);

    public static LevelGen S;
    void Awake()
    {
        S = this;
    }
	// Use this for initialization
	void Start () {
        combat = GameObject.Find("CombatController").GetComponent<CombatController>();
    }
	
	// Update is called once per frame
	void Update () {
	    if(Input.GetKeyDown(KeyCode.R))
        {
            Application.LoadLevel(Application.loadedLevel);
        }
	}

    public void GenLevel()
    {
        print("Generating Level");
        for(int i = 0; i < numWalls; i++)
        {
            float x = Random.Range(min.x, max.x);
            float y = Random.Range(min.y, max.y);
            Instantiate(wall, new Vector3(x, y, -1), Quaternion.Euler(0, 0, Random.Range(0,180)));
            print("Hi " + i);
        }
    }
}
