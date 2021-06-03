using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class GameBehavior : MonoBehaviour {

	private bool BestRecordShowed = false;
	private float BestRecordParticleStopTime = 0f;

	public GameObject GameOverPanel;
	public GameObject BestRecordPanel;
	public GameObject MainCanvasPanel;

	public Text ScoreText;
	public ParticleSystem BestScoreParticleSystem;
	public float BestRecordParticleShowTime = 5f;
	public int BestRecordParticleEmitCount = 20;

	// Use this for initialization
	void Awake () {
		Game.Instance.Initialize();
		Game.Instance.GameOverPanel = this.GameOverPanel;
	}

	void Start() {
		var ads = GameObject.FindGameObjectWithTag("AdsTag");
		if(ads != null)
		{
			ads.GetComponent<GoogleAdsBehavior>().ShowBanner();
			if(PlayerPrefs.GetInt("interstitial", 0) == 1 || Game.Instance.PlayCount % Int32.Parse(Settings.InterstitialFrequency) == 0) {
				PlayerPrefs.SetInt("interstitial", 1);
				ads.GetComponent<GoogleAdsBehavior>().GetReadyForInterstitial();
			}
		}
	}

	void Update(){
		if(Game.Instance.IsDead || Game.Instance.IsPaused)
			return;

		if(ScoreText != null)
		{
			ScoreText.text = Game.Instance.CurrentJumpCount.ToString();
		}

		if(!BestRecordShowed && !Game.Instance.IsFirstPlayMode && Game.Instance.BestScore < Game.Instance.CurrentJumpCount)
		{
			BestRecordShowed = true;
			BestRecordParticleStopTime = Time.time + 3f;

			var bestRecordPanel = Instantiate(BestRecordPanel);
			bestRecordPanel.transform.SetParent(MainCanvasPanel.transform);
			var rectTransform = bestRecordPanel.GetComponent<RectTransform>();
			rectTransform.anchoredPosition = Vector2.zero;
			rectTransform.transform.localScale =Vector3.one;
		}

		if(BestRecordParticleStopTime != 0f && Time.time < BestRecordParticleStopTime)
		{
			BestScoreParticleSystem.Emit(BestRecordParticleEmitCount);
		}
	}
}
