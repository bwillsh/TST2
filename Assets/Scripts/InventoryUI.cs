using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour {

	public Sprite keySprite;

	// Use this for initialization
	void Start () {
		switch (GameManager.S.currentItem) {
		case "Key":
			print ("Display key sprite.");
			GetComponent<Image> ().sprite = keySprite;
			break;
		default:
			print ("No item.");
			break;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
