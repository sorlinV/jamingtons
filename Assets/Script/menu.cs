using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class menu : MonoBehaviour {

	public void loadScene(string sceneName) {
		SceneManager.LoadScene(sceneName);
	}

	public void exit() {
        Application.Quit();
	}
}
