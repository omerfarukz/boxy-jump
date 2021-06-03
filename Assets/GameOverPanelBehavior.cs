using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameOverPanelBehavior : MonoBehaviour {

	public Text ScoreText;
	public Text BestScoreText;

	void Start(){
		iTween.ValueTo(this.gameObject, iTween.Hash("from", Game.Instance.CurrentJumpCount * 0.6f , "to", (float)Game.Instance.CurrentJumpCount, "onupdate", "UpdateJumpCount", "time", 0.5, "easetype", iTween.EaseType.easeInOutQuad));
		BestScoreText.text = Game.Instance.BestScore.ToString();
	}

	public void UpdateJumpCount(float f) {
		ScoreText.text = ((int)f).ToString();
	}

	public void Replay()
	{
		var ads = GameObject.FindGameObjectWithTag("AdsTag").GetComponent<GoogleAdsBehavior>();
		ads.HideBannerIfItsOnTheScene();

		Application.LoadLevel("Level");
	}

	public void Share() {
		string url = Settings.APP_REDIRECT_URL;
//		if(Application.platform == RuntimePlatform.Android)
//		{
//			if(Settings.IsAmazonBuild)
//			{
//				url = Settings.APP_AMAZON_URL;
//			}
//			else
//			{
//				url = Settings.APP_GOOGLE_URL;
//			}
//		}

		string text = string.Format(Localization.Instance.GetString("SHARE_TEXT"), Game.Instance.BestScore, url);

		var shareComponent = GetComponent<ShareBehavior>();
		if(shareComponent == null)
			shareComponent = gameObject.AddComponent<ShareBehavior>();
		
		StartCoroutine(shareComponent.ShareScreenshot(text));
	}

	public void RateUs() {
		Debug.Log("Rating...");
		//Application.OpenURL(Settings.APP_REDIRECT_URL);

		if(Application.platform == RuntimePlatform.Android)
		{
			if(Settings.IsAmazonBuild)
			{
				Application.OpenURL(Settings.APP_AMAZON_URL);
			}
			else
			{
				Application.OpenURL(Settings.APP_GOOGLE_URL);
			}
		}
		else if(Application.platform == RuntimePlatform.IPhonePlayer)
		{
			Application.OpenURL(Settings.APP_IOS_URL);
		}
		else
		{
			Debug.LogWarning("Rate us not supported on " + Application.platform);
		}
	}

	public void Continue()
	{

	}
}
