using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelGen : MonoBehaviour {

    CombatController combat;
    
    //Variables that control what spawns
    public bool progressing = true;
    public int numWalls = 1;
    public int futureWalls = 1;
    public int numNinjas = 1;
    public int futureNinjas = 1;
    public int numJumps = 4;

    int blah = 0;

    //GameObjects for prefabs
    public GameObject wall;
    public Ninja ninja;
    public GameObject empty;

    //Level size
    public Vector2 max = new Vector2(18.05f,13.4f);
    public Vector2 min = new Vector2(-8f, -3.1f);
    Vector2 FootPos;

    public List<GameObject> wallList;
    public List<GameObject> collList;


    public static LevelGen S;
    void Awake()
    {
        S = this;
    }
	// Use this for initialization
	void Start () {
        blah = 0;
        DontDestroyOnLoad(this.gameObject);
        futureNinjas = numNinjas;
        futureWalls = numWalls;
        print(Application.loadedLevelName);
        if(Application.loadedLevelName != "endlessStart")
        {
            combat = GameObject.Find("CombatController").GetComponent<CombatController>();
            FootPos = Foot.S.transform.position;
        }
        
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
        //If you want the levels to get harder, fetch the newest numbers
        if(progressing)
        {
            blah++;
            numWalls = blah%10 + 1;
            if (blah < 10)
                numNinjas = 1;
            else if (blah < 20)
                numNinjas = 2;
            else
                numNinjas = 3;
        }
       
        //clear old walls
        wallList = new List<GameObject>();

        combat = GameObject.Find("CombatController").GetComponent<CombatController>();
        FootPos = Foot.S.transform.position;
        float x = 0;
        float y = 0;

        print("Generating Level");
        for (int i = 0; i < numNinjas; i++)
        {
            //spawns a ninja on edge of screen
            x = Random.Range(-7f, -5);
            y = Random.Range(min.y, max.y);
            Ninja clone = (Ninja)Instantiate(ninja, new Vector3(x, y, -1), Quaternion.identity);


            //initial jump point is equal to ninja position
            clone.jumpPoints = new List<Transform>();
            GameObject npoint = (GameObject)Instantiate(empty, clone.transform.position, Quaternion.identity);
            clone.jumpPoints.Add(npoint.transform);

            //set up jump points at equal intervals
            float jx = x;
            float inc = (Foot.S.transform.position.x - x) / numJumps;

            //spawn number of jump points
            for (int j = 1; j < numJumps; j++)
            {
                print("Jump point " + j);
                jx += inc;
                float jy = Random.Range(min.y, max.y);
                GameObject tpoint = (GameObject)Instantiate(empty, new Vector3(jx, jy, -1), Quaternion.identity);
                clone.jumpPoints.Add(tpoint.transform);
            }

            //last jump point = to tommy
            GameObject ttpoint = (GameObject)Instantiate(empty, Foot.S.transform.position, Quaternion.identity);
            clone.jumpPoints.Add(ttpoint.transform);

            //points get added in reverse order, so need to swap around
            clone.jumpPoints.Reverse();
            combat.ninjaList.Add(clone);

        }
        int tryCount = 0;

        //spawn walls
        for (int i = 0; i < numWalls; i++)
        {
            //set a random point for the wall
            x = Random.Range(min.x, max.x);
            y = Random.Range(min.y, max.y);

            //test to see if point is ok
            bool good = false;
            while(!good)
            {
                //creates a new test location
                x = Random.Range(min.x, max.x);
                y = Random.Range(min.y, max.y);

                //only need one bad point to throw out cur wall
                good = true;

                //make sure you dont overlap any of the ninja jump points
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

                //make sure walls dont overlap, not necessary but it looks better
                foreach (GameObject n in wallList)
                {
                    float x2 = n.transform.position.x;
                    float y2 = n.transform.position.y;
                    float dist = Mathf.Sqrt(Mathf.Pow(x2 - x, 2) + Mathf.Pow(y2 - y, 2));
                    if (dist < 5)
                        good = false;
                }

                //if bad, try again, unless youve tried too many times
                if(!good)
                {
                    print("not good: " + i);
                    tryCount++;
                    if (tryCount > 10)
                        break;
                }
                
            }

            //If all good, add wall to the world
            if(good)
            {
                int num = Random.Range(0, collList.Count);
                GameObject go = collList[num];
                float f = Random.Range(0, 360);
                switch (num)
                {
                    
                    case 1: //Swing seat
                        f = 0f;
                        break;
                    case 8: //Light Fixture
                        f = Random.Range(45, 135);
                        break;
                    case 11: //Stool
                        f = Random.Range(-180, 0);
                        break;
                }
                
                wallList.Add((GameObject)Instantiate(go, new Vector3(x, y, -1), Quaternion.Euler(0, 0, f)));
                tryCount = 0;
            }

            //if not good, stop adding walls. even if you are not at numWalls
        
            
        }//End wall placement
        
    }
}
