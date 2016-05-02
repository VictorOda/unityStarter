using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SocialPlatforms;
using Facebook.Unity;


public class SocialController : MonoBehaviour {

	public static SocialController instance;

	// Facebook
	private List<string> permissions = new List<string>() {"publish_actions"};

	// Social
	private string leaderboardId = "";

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

		// Authenticate and register a ProcessAuthentication callback
		// This call needs to be made before we can proceed to other calls in the Social API
		Social.localUser.Authenticate (ProcessAuthentication);

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


	#region GameCenter

	// This function gets called when Authenticate completes
	// Note that if the operation is successful, Social.localUser will contain data from the server. 
	void ProcessAuthentication (bool success) {
		if (success) {
			Debug.Log ("Authenticated, checking achievements");

			// Request loaded achievements, and register a callback for processing them
			Social.LoadAchievements (ProcessLoadedAchievements);

			//Post the highscore that the player already has
			//PostScore(ScoreManager.instance.score);
		}
		else
			Debug.Log ("Failed to authenticate");
	}

	// This function gets called when the LoadAchievement call completes
	void ProcessLoadedAchievements (IAchievement[] achievements) {
		if (achievements.Length == 0)
			Debug.Log ("Error: no achievements found");
		else
			Debug.Log ("Got " + achievements.Length + " achievements");

		// You can also call into the functions like this
		Social.ReportProgress ("Achievement01", 100.0, result => {
			if (result)
				Debug.Log ("Successfully reported achievement progress");
			else
				Debug.Log ("Failed to report achievement");
		});
	}

	public void ShowLeaderboard () {
		Debug.Log("SHOW LEADERBOARD");
		Social.ShowLeaderboardUI();
	}

	public void PostScore (int score) {
		Debug.Log("Posting score...");
		Social.ReportScore((long)score, leaderboardId, HighScoreCheck);
	}

	void HighScoreCheck (bool result) {
		if(result)
			Debug.Log("score submission successful");
		else
			Debug.Log("score submission failed");
	}

	#endregion
}
