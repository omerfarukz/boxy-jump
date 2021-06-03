using UnityEngine;
using System.Collections;
using GoogleMobileAds.Api;

//TODO: android icin ayarlar yapilmali
//TODO:sinif acilmali
//TODO: android ve ios icin ads kodlari belirlenmeli
public class GoogleAdsBehavior : MonoBehaviour {
		
//	public void ShowInterstitialIfItIsReady() {}
//	
//	public void ShowBanner() {}
//	
//	public void HideBannerIfItsOnTheScene () {}
//	
//	public void GetReadyForInterstitial() {}

	private bool _bannerUsed;
	private bool _gameOverBannerUsed;

	private InterstitialAd _interstitial;
	private BannerView _bannerView;
	private BannerView _gameOverBannerView;

	void Start () {
#if !UNITY_EDITOR
		if (Debug.isDebugBuild)
		{
			Debug.Log("Ads disabled in development build");
			return;
		}
#endif
	}

	public void ShowInterstitialIfItIsReady() {
		if(Settings.AdsEnabled == "0")
		{
			Debug.Log("ads disabled on settings.adenabled");
			return;
		}

		if(_interstitial != null && _interstitial.IsLoaded())
		{
			_interstitial.Show();
		}
		else
		{
			Debug.LogWarning("ShowInterstitialIfItIsReady is not ready");
		}
	}

	private AdRequest CreateRequest ()
	{
		var requestBuilder = new AdRequest.Builder ();
		requestBuilder.AddTestDevice ("3b4f7df7cf890c26a441aac75d0d1c0b");

		AdRequest request = requestBuilder.Build ();
		return request;
	}
	
	public void ShowBanner(){
		if(Settings.AdsEnabled == "0")
		{
			Debug.Log("ads disabled on settings.adenabled");
			return;
		}

		if(_gameOverBannerUsed && _gameOverBannerView != null)
		{
			_gameOverBannerView.Hide();
			_gameOverBannerView.Destroy();
		}

		print ("Settings.ShowBannerOnGamePlay: " + Settings.ShowBannerOnGamePlay);

		if(Settings.ShowBannerOnGamePlay == "0")
			return;

		_bannerView = new BannerView(GetBannerId(), AdSize.Banner, AdPosition.Bottom);
		var request = CreateRequest ();

		_bannerView.LoadAd(request);
		_bannerUsed = true;
	}


	public void ShowGameOverBanner(){
		if(Settings.AdsEnabled == "0")
		{
			Debug.Log("ads disabled on settings.adenabled");
			return;
		}

		if(Settings.ShowBannerOnGameOver == "0")
			return;

		var request = CreateRequest ();

		if(_bannerUsed && _bannerView != null)
		{
			_bannerView.Hide();
			_bannerView.Destroy();
		}

		_gameOverBannerView = new BannerView(GetBannerId(), AdSize.Banner, AdPosition.Bottom);
		_gameOverBannerView.LoadAd(request);
		_gameOverBannerUsed = true;
	}

	public void HideBannerIfItsOnTheScene ()
	{
		if(Settings.AdsEnabled != "1")
		{
			Debug.Log("ads disabled on settings.adenabled");
			return;
		}

		if(_bannerUsed && _bannerView != null)
		{
			_bannerView.Hide();
			_bannerView.Destroy();
		}

		if(_gameOverBannerUsed && _gameOverBannerView != null)
		{
			_gameOverBannerView.Hide();
			_gameOverBannerView.Destroy();
		}
	}

	public void GetReadyForInterstitial() {
		if(Settings.AdsEnabled != "1")
		{
			Debug.Log("ads disabled on settings.adenabled");
			return;
		}

		if(_interstitial != null)
			return;

		Debug.Log("Interstitial ad requested");
		
		var request = CreateRequest ();
		_interstitial = new InterstitialAd(GetIntersitialId());
		_interstitial.LoadAd(request);
		_interstitial.AdOpened += (o, e)=> {
			PlayerPrefs.SetInt("interstitial", 0);
		};
	}

	private string GetBannerId(){
		if(Application.platform == RuntimePlatform.IPhonePlayer)
		{
			return Settings.Admob_IOS_Baner_Id;
		}
		else if (Application.platform == RuntimePlatform.Android)
		{
			return Settings.Admob_Android_Banner_Id;
		}

		return null;
	}

	private string GetIntersitialId(){
		if(Application.platform == RuntimePlatform.IPhonePlayer)
		{
			return Settings.Admob_IOS_Interstitial_Id;
		}
		else if (Application.platform == RuntimePlatform.Android)
		{
			return Settings.Admob_Android_Interstitial_Id;
		}
		
		return null;
	}
}
