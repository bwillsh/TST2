using UnityEngine;
using System.Collections;

public class MenuManager : MonoBehaviour {

	public GameObject menuCanvas;
	public GameObject optionsCanvas;

	public void Play() {
		Application.LoadLevel ("OpenCutscene");
	}

	public void Endless() {
		Application.LoadLevel ("endlessStart");
	}

	public void Select() {
		Application.LoadLevel ("Select_Hall1");
	}

	public void Options() {
		menuCanvas.SetActive (false);
		optionsCanvas.SetActive (true);
	}

	public void SetMute(bool mute) {
		print ("setting ismuted to " + mute);
		GameManager.S.isMuted = mute;
	}

	public void ResetSave() {
		GameManager.S.NukeSaveData();
	}

	public void BackToMenu() {
		menuCanvas.SetActive (true);
		optionsCanvas.SetActive (false);
	}
}
