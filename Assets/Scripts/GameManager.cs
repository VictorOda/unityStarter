using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public static GameManager instance;

	[Header("UIs")]
	public GameObject gameplayUI;
	public GameObject winUI;
	public GameObject loseUI;

	void Start () {
		if(!instance)
			instance = this;
	}

	public void Menu() {
		SceneController.instance.ChangeScene("Menu");
	}

	public void Replay() {
		SceneController.instance.ChangeScene("Gameplay");
	}

	public void Win() {
		gameplayUI.SetActive(false);
		winUI.SetActive(true);
	}

	public void Lose() {
		gameplayUI.SetActive(false);
		loseUI.SetActive(true);
	}
}
