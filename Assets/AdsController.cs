using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;
using System.Collections;
using System.Collections.Generic;
using GoogleMobileAds.Api;
using Facebook.Unity;

public class AdsController : MonoBehaviour {

	public static AdsController instance;

	// UnityAds
	public string unityAdsZoneId, unityAdsZoneIdRewarded;
	public string iOSAdMobId, androidAdMobId;

	// AdMob
	string adMobUnityId;
	[HideInInspector]
	public InterstitialAd interstitial;

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

		#if UNITY_ANDROID
		adMobUnityId = androidAdMobId;
		#elif UNITY_IPHONE
		adMobUnityId = iOSAdMobId;
		#else
		adMobUnityId = "unexpected_platform";
		#endif

		RequestInterstitial();
	}

	#region UnitAds
	public void ShowUnityAds (bool rewarded)
	{
		if(rewarded)
		{
			if (string.IsNullOrEmpty(unityAdsZoneIdRewarded)) 
				unityAdsZoneIdRewarded = null;

			if(!Advertisement.IsReady(unityAdsZoneIdRewarded))
			{
				Debug.Log("UnityAds rewarded zone not ready");
				return;
			}

			ShowOptions options = new ShowOptions();
			options.resultCallback = HandleShowResult;

			Advertisement.Show(unityAdsZoneIdRewarded, options);
		}
		else
		{
			if (string.IsNullOrEmpty(unityAdsZoneId)) 
				instance.unityAdsZoneId = null;

			if(!Advertisement.IsReady(unityAdsZoneId))
			{
				Debug.Log("UnityAds zone not ready");
				return;
			}

			Advertisement.Show(unityAdsZoneId);
		}
	}


	/// <summary>
	/// Handles the show result for the non rewarded ad.
	/// If the last video played was a rewarded video, give player the reward.
	/// </summary>
	private void HandleShowResult (ShowResult result)
	{
		switch (result)
		{
		case ShowResult.Finished:
			Debug.Log ("Video rewarded completed.");
			break;
		case ShowResult.Skipped:
			Debug.LogWarning ("Video was skipped.");
			break;
		case ShowResult.Failed:
			Debug.LogError ("Video failed to show.");
			break;
		}
	}
	#endregion

	#region AdMob
	private void RequestInterstitial () {
		// Initialize an InterstitialAd.
		interstitial = new InterstitialAd(adMobUnityId);
		// Create an empty ad request.
		AdRequest request = new AdRequest.Builder().Build();
		// Load the interstitial with the request.
		interstitial.LoadAd(request);
	}

	public void ShowAdMobInterstitial () {
		if(interstitial.IsLoaded()) {
			interstitial.Show();
		}
	}
	#endregion

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
