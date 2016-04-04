using UnityEngine;
using System.Collections;

public class SelectLevel : MonoBehaviour {
	public void LoadRoom(string room) {
		Application.LoadLevel (room);
	}
}
