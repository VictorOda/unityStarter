using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;
using System.Collections;
using GoogleMobileAds.Api;

public class AdsController : MonoBehaviour {

	public static AdsController instance;

	public string unityAdsZoneId, unityAdsZoneIdRewarded;
	public string iOSAdMobId, androidAdMobId;

	string adMobUnityId;



	[HideInInspector]
	public InterstitialAd interstitial;

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
	public static void ShowUnityAds (bool rewarded)
	{
		if(rewarded)
		{
			if (string.IsNullOrEmpty (instance.unityAdsZoneIdRewarded)) 
				instance.unityAdsZoneIdRewarded = null;

			if(!Advertisement.IsReady(instance.unityAdsZoneIdRewarded))
			{
				Debug.Log("UnityAds rewarded zone not ready");
				return;
			}

			ShowOptions options = new ShowOptions();
			options.resultCallback = instance.HandleShowResult;

			Advertisement.Show (instance.unityAdsZoneIdRewarded, options);
		}
		else
		{
			if (string.IsNullOrEmpty (instance.unityAdsZoneId)) 
				instance.unityAdsZoneId = null;

			if(!Advertisement.IsReady(instance.unityAdsZoneId))
			{
				Debug.Log("UnityAds zone not ready");
				return;
			}

			Advertisement.Show (instance.unityAdsZoneId);
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
}
