using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LevelSelectButton : MonoBehaviour {
	public string levelToLoad;
	public Image star;

	void OnLevelWasLoaded(int level) {
		print ("level was loaded");
		if (level == Application.loadedLevel) {
			print ("this level was loaded");
			if (star != null) {
				if (GameManager.S.levelsBeaten.Contains (levelToLoad)) {
					print ("enable star");
					star.enabled = true;
				} else {
					print (levelToLoad + " not beaten");
					star.enabled = false;
				}
			} else {
				print ("no star to show");
			}
		}
	}

	public void LoadRoom() {
		GameManager.S.level = Application.loadedLevel;
		Application.LoadLevel (levelToLoad);
	}
}
