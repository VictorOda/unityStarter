using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;
using System.Collections;

public class AdsController : MonoBehaviour {

	public static AdsController instance;

	public string unityAdsZoneId, unityAdsZoneIdRewarded;

	void Start () {
		if(!instance)
			instance = this;
		DontDestroyOnLoad(this.gameObject);
	}

	#region UnitAds
	public void ShowUnityAds (bool rewarded)
	{
		if(rewarded)
		{
			if (string.IsNullOrEmpty (unityAdsZoneIdRewarded)) 
				unityAdsZoneIdRewarded = null;

			if(!Advertisement.IsReady(unityAdsZoneIdRewarded))
			{
				Debug.Log("UnityAds rewarded zone not ready");
				return;
			}

			ShowOptions options = new ShowOptions();
			options.resultCallback = HandleShowResult;

			Advertisement.Show (unityAdsZoneIdRewarded, options);
		}
		else
		{
			if (string.IsNullOrEmpty (unityAdsZoneId)) 
				unityAdsZoneId = null;

			if(!Advertisement.IsReady(unityAdsZoneId))
			{
				Debug.Log("UnityAds zone not ready");
				return;
			}

			Advertisement.Show (unityAdsZoneId);
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
}
