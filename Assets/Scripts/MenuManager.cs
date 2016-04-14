using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour {

	public GameObject menuCanvas;
	public GameObject optionsCanvas;
	public Toggle myMute;
	public Toggle myTimer;

	void Start() {
		myMute.isOn = GameManager.S.isMuted;
		myTimer.isOn = GameManager.S.isTimed;
	}

	public void Play() {
        string lvl = PlayerPrefs.GetString("Level");
        if (lvl == "hall1" || lvl == "hall2" ||
            lvl == "hall3" || lvl == "hall4" ||
            lvl == "hall5" || lvl == "hall6")
        {
            Application.LoadLevel(PlayerPrefs.GetString("Level"));
        }
        else
        {
            Application.LoadLevel("OpenCutscene");
        }
            
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
		PlayerPrefs.SetInt ("Mute", mute ? 1 : 0);
		GameManager.S.isMuted = mute;
	}

	public void SetTimer(bool timer) {
		PlayerPrefs.SetInt ("Timer", timer ? 1 : 0);
		GameManager.S.isTimed = timer;
	}

	public void ResetSave() {
		GameManager.S.NukeSaveData();
	}

	public void BackToMenu() {
		menuCanvas.SetActive (true);
		optionsCanvas.SetActive (false);
	}
}
