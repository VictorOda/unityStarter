using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour {

	public static SceneController instance;

	void Start() {
		if(!instance)
			instance = this;
	}

	public void ChangeScene(string sceneName) {
		SceneManager.LoadScene(sceneName);
	}

	public string CurrentSceneName() {
		return SceneManager.GetActiveScene().name;
	}
}
