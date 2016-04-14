using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour {

//	public Sprite pencilSprite;
//
//	// Use this for initialization
//	void Start () {
//		switch (GameManager.S.currentItem) {
//		case "Pencil":
//			print ("Display pencil sprite.");
//			GetComponent<Image> ().sprite = pencilSprite;
//			GetComponent<Image> ().enabled = true;
//			break;
//		default:
//			print ("No item.");
//			GetComponent<Image> ().enabled = false;
//			break;
//		}
//	}
	
	// Update is called once per frame
	void Update () {
		if (GameManager.S.currentItem == "") {
			GetComponent<Image> ().enabled = false;
		} else {
			print ("have item");
			GetComponent<Image> ().enabled = true;
		}

	}
}
