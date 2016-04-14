using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour {

	public GameObject menuCanvas;
	public GameObject optionsCanvas;
	public Toggle mute;

	public AudioSource music;

	void Start() {
		mute.isOn = GameManager.S.isMuted;
	}

	void Update(){
		if (GameManager.S.isMuted) {
			music.mute = true;
		} else {
			music.mute = false;
		}
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

	public void ResetSave() {
		GameManager.S.NukeSaveData();
	}

	public void BackToMenu() {
		menuCanvas.SetActive (true);
		optionsCanvas.SetActive (false);
	}
}
