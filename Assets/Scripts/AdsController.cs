using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;
using System.Collections;
using GoogleMobileAds.Api;

public class AdsController : MonoBehaviour {

	public static AdsController instance;

	[Header ("UnityAds")]
	public string unityAdsZoneId;
	public string unityAdsZoneIdRewarded;

	[Header ("AdMob")]
	public string iOSAdMobInterstitialId;
	public string androidAdMobInterstitialId;
	public string iOSAdMobBannerId, androidAdMobBannerId;
	string adMobInterstitialId, adMobBannerId;
	[HideInInspector]
	public InterstitialAd interstitial;


	void Start () {
		if(!instance)
			instance = this;
		
		DontDestroyOnLoad(this.gameObject);

		#if UNITY_ANDROID
		adMobInterstitialId = androidAdMobInterstitialId;
		adMobBannerId = androidAdMobBannerId;
		#elif UNITY_IPHONE
		adMobInterstitialId = iOSAdMobInterstitialId;
		adMobBannerId = iOSAdMobBannerId;
		#else
		adMobUnityId = "unexpected_platform";
		adMobBannerId = "unexpected_platform";
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
		interstitial = new InterstitialAd(adMobInterstitialId);
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

	/// <summary>
	/// Shows the ad mob banner at a certain position.
	/// Positions available: Top, TopLeft, TopRight, Bottom, BottomLeft, BottomRight;
	/// Bottom is the default position.
	/// </summary>
	/// <param name="position">Position.</param>
	public void ShowAdMobBanner (string position) {

		AdPosition adPosition;

		switch(position) {
		case "Top": adPosition = AdPosition.Top;
			break;
		case "TopLeft": adPosition = AdPosition.TopLeft;
			break;
		case "TopRight": adPosition = AdPosition.TopRight;
			break;
		case "Bottom": adPosition = AdPosition.Bottom;
			break;
		case "BottomLeft": adPosition = AdPosition.BottomLeft;
			break;
		case "BottomRight": adPosition = AdPosition.BottomRight;
			break;
		default: adPosition = AdPosition.Bottom;
			break;
		}

		// Create a 320x50 banner at the top of the screen.
		BannerView bannerView = new BannerView(adMobBannerId, AdSize.Banner, adPosition);
		// Create an empty ad request.
		AdRequest request = new AdRequest.Builder().Build();
		// Load the banner with the request.
		bannerView.LoadAd(request);
	}
	#endregion


}
