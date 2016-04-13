using UnityEngine;
using System.Collections;

public class GameManagerManager : MonoBehaviour {


    public GameObject GM;
	// Use this for initialization
	void Awake () {
        GameObject clone = GameObject.Find("GameManager(Clone)");
        if (clone == null)
        {
            Instantiate(GM);
        }
        
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
