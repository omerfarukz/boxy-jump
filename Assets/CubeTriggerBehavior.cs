using UnityEngine;
using System.Collections;
using System;

public class CubeTriggerBehavior : MonoBehaviour {
	
	public GameObject Cube;
	public string NameOfGoodCube;
	public string NameOfBadCube;

	void OnTriggerEnter(Collider collider)
	{
		if(Game.Instance.IsDead || Game.Instance.IsPaused)
			return;

		if(collider.name.StartsWith(NameOfBadCube))
		{
			CallGameOverWithNotification();

			iTween.Stop(Cube.GetComponent<CubeControllerBehavior>().Parent);
			Cube.GetComponent<Animator>().SetTrigger("AnyToDie");
			iTween.Stop(Cube);
		}
	}

	void Update(){
		if(Game.Instance.IsDead || Game.Instance.IsPaused)
			return;

		var cubeScreenPosition = Camera.main.WorldToScreenPoint(Cube.transform.position);
		if(cubeScreenPosition.x < -50f)
		{
			iTween.Stop(Cube.GetComponent<CubeControllerBehavior>().Parent);
			CallGameOverWithNotification();
			iTween.Stop(Cube);
		}
	}

	void CallGameOverWithNotification() {
		Game.Instance.GameOver();

		if(
			Game.Instance.NextRateUsDateTime < DateTime.Now && 
			Game.Instance.PlayCount > int.Parse(Settings.RateUsFrequency)
		)
		{
			var shareComponent = GetComponent<ShareBehavior>();
			if(shareComponent==null)
				shareComponent = gameObject.AddComponent<ShareBehavior>();
			
			shareComponent.ShowRateUs();
		}

		var ads = GameObject.FindGameObjectWithTag("AdsTag").GetComponent<GoogleAdsBehavior>();
		ads.HideBannerIfItsOnTheScene();
		ads.ShowInterstitialIfItIsReady();
		ads.ShowGameOverBanner();

		var pn = GameObject.FindGameObjectWithTag("ProfileNotifierTag");
		var behavior = pn.GetComponent<ProfileNotificationBehavior>();
		StartCoroutine(behavior.GameOver((int)Game.Instance.CurrentScore, Game.Instance.CurrentJumpCount, Game.Instance.BestScore));
	}
}