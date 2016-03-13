using UnityEngine;
using System.Collections;

public class LevelGen : MonoBehaviour {

    CombatController combat;

    public int numWalls = 1;
    public int numNinjas = 1;
    public int numJumps = 4;

    public GameObject wall;
    public Ninja ninja;
    public GameObject empty;

    public Vector2 max = new Vector2(18.05f,13.4f);
    public Vector2 min = new Vector2(-8f, -3.1f);
    Vector2 FootPos;

    public static LevelGen S;
    void Awake()
    {
        S = this;
    }
	// Use this for initialization
	void Start () {
        combat = GameObject.Find("CombatController").GetComponent<CombatController>();
        FootPos = Foot.S.transform.position;
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
        float x;
        float y;
        print("Generating Level");
        for(int i = 0; i < numWalls; i++)
        {
            x = Random.Range(min.x, max.x);
            y = Random.Range(min.y, max.y);
            Instantiate(wall, new Vector3(x, y, -1), Quaternion.Euler(0, 0, Random.Range(0,180)));
        }
        for (int i = 0; i < numNinjas; i++)
        {
            x = Random.Range(15f, 17);
            y = Random.Range(min.y, max.y);
            Ninja clone = (Ninja)Instantiate(ninja, new Vector3(x, y, -1), Quaternion.identity);
            GameObject npoint = (GameObject)Instantiate(empty, clone.transform.position, Quaternion.identity);
            clone.jumpPoints.Add(npoint.transform);
            float jx = x;
            float dec = (x - Foot.S.transform.position.x) / numJumps;
            for(int j = 1; j < numJumps; j++)
            {
                jx -= dec;
                float jy = Random.Range(min.y, max.y);
                GameObject tpoint = (GameObject)Instantiate(empty, new Vector3(jx, jy, -1), Quaternion.identity);
                clone.jumpPoints.Add(tpoint.transform);
            }
            GameObject ttpoint = (GameObject)Instantiate(empty, Foot.S.transform.position, Quaternion.identity);
            clone.jumpPoints.Add(ttpoint.transform);
            clone.jumpPoints.Reverse();
            combat.ninjaList.Add(clone);

        }
    }
}
