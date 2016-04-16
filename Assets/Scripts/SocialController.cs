using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Facebook.Unity;


public class SocialController : MonoBehaviour {

	public static SocialController instance;

	// Facebook
	private List<string> permissions = new List<string>() {"publish_actions"};

	void Awake ()
	{
		if (!FB.IsInitialized) {
			// Initialize the Facebook SDK
			FB.Init(InitCallback, OnHideUnity);
		} else {
			// Already initialized, signal an app activation App Event
			FB.ActivateApp();
		}
	}
	
	void Start () {
		if(!instance)
			instance = this;

		DontDestroyOnLoad(this.gameObject);
	}

	#region Facebook
	private void InitCallback ()
	{
		if (FB.IsInitialized) {
			// Signal an app activation App Event
			FB.ActivateApp();
		} else {
			Debug.Log("Failed to Initialize the Facebook SDK");
		}
	}

	private void OnHideUnity (bool isGameShown)
	{
		if (!isGameShown) {
			// Pause the game - we will need to hide
			Time.timeScale = 0;
		} else {
			// Resume the game - we're getting focus again
			Time.timeScale = 1;
		}
	}

	public void FBLogin () {
		if(FB.IsLoggedIn) {
			ShareScore();
		} else {
			FB.LogInWithPublishPermissions(permissions, AuthCallback);
		}
	}

	void AuthCallback (ILoginResult result) {
		if(FB.IsLoggedIn) {
			ShareScore();
			Debug.Log("FB login worked!");
		} else {
			Debug.Log("FB login failed!");
			Invoke("InvokeLogin", 0.1f);
		}
	}

	void InvokeLogin() {
		if(FB.IsLoggedIn) {
			ShareScore();
		}
	}

	public void ShareScore() {
		FB.ShareLink(
			new System.Uri("http://alphaquestgames.com/games/orc-smasher/"),
			"Olha esse jogo! Eu estou jogando Marvin The Volcano e é muito legal!",
			"Eu consegui " + "ScoreManager.score.ToString()" + " pontos! Você consegue fazer melhor?",
			new System.Uri("http://alphaquestgames.com/wp-content/uploads/2014/12/icone-site-orc.png")
		);
	}
	#endregion
}
