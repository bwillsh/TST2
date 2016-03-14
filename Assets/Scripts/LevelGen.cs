using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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

    public List<GameObject> wallList;

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
        for (int i = 0; i < numNinjas; i++)
        {
            x = Random.Range(15f, 17);
            y = Random.Range(min.y, max.y);
            Ninja clone = (Ninja)Instantiate(ninja, new Vector3(x, y, -1), Quaternion.identity);
            GameObject npoint = (GameObject)Instantiate(empty, clone.transform.position, Quaternion.identity);
            clone.jumpPoints.Add(npoint.transform);
            float jx = x;
            float dec = (x - Foot.S.transform.position.x) / numJumps;
            for (int j = 1; j < numJumps; j++)
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
        int tryCount = 0;
        for (int i = 0; i < numWalls; i++)
        {
            x = Random.Range(min.x, max.x);
            y = Random.Range(min.y, max.y);
            bool good = false;
            while(!good)
            {
                x = Random.Range(min.x, max.x);
                y = Random.Range(min.y, max.y);
                good = true;
                foreach(Ninja n in combat.ninjaList)
                {
                    foreach(Transform t in n.jumpPoints)
                    {
                        float x2 = t.position.x;
                        float y2 = t.position.y;
                        float dist = Mathf.Sqrt(Mathf.Pow(x2 - x, 2) + Mathf.Pow(y2 - y, 2));
                        if (dist < 3)
                            good = false;
                    }
                    
                }
                foreach (GameObject n in wallList)
                {
                    float x2 = n.transform.position.x;
                    float y2 = n.transform.position.y;
                    float dist = Mathf.Sqrt(Mathf.Pow(x2 - x, 2) + Mathf.Pow(y2 - y, 2));
                    if (dist < 3)
                        good = false;
                }
                if(!good)
                {
                    print("not good: " + i);
                    tryCount++;
                    if (tryCount > 10)
                        break;
                }
                
            }
            if(good)
            {
                wallList.Add((GameObject)Instantiate(wall, new Vector3(x, y, -1), Quaternion.Euler(0, 0, Random.Range(0, 180))));
                tryCount = 0;
            }
            else
            {
                i = numWalls + 10;
            }
        }
        
    }
}
