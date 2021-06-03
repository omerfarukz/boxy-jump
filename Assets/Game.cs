using UnityEngine;
using System;
using System.Collections.Generic;

public class Game : UnityEngine.Object
{
	public static Game Instance = new Game();

	public bool IsPaused {
		get;
		private set;
	}

	public bool IsDead
	{
		get;
		set;
	}
	
	public int CurrentJumpCount
	{
		get;
		private set;
	}

	public float CurrentScore
	{
		get;
		private set;
	}

	public GameObject GameOverPanel {
		get;
		set;
	}

	public GameObject ScoreText {
		get;
		set;
	}

	public float CameraSpeed {
		get;
		set;
	}

	public float JumpSpeed {
		get{
			return 2 + CameraSpeed / 2f;
		}
	}

	public bool IsInTutorialMode {
		get;
		set;
	}

	public int BestScore {
		get;
		set;
	}

	public int PlayCount {
		get {
			return PlayerPrefs.GetInt("PlayCount", 0);
		}
		set {
			PlayerPrefs.SetInt("PlayCount", value);
			PlayerPrefs.Save();
		}
	}

	public bool SoundIsOn {
		get{
			return PlayerPrefs.GetInt("SoundIsOn", 1) == 1;
		}
		set
		{
			PlayerPrefs.SetInt("SoundIsOn", value ? 1 : 0);
			PlayerPrefs.Save();
		}
	}

	public bool IsFirstPlayMode {
		get{
			if(BestScore<10)
				return true;

			return PlayerPrefs.GetInt("FirstPlay", 1) == 1;
		}
		set {
			PlayerPrefs.SetInt("FirstPlay", value ? 1 : 0);
			PlayerPrefs.Save();
		}
	}

	public bool TutorialIsPlayed {
		get{
			return PlayerPrefs.GetInt("TutorialIsPlayed", 0) == 1;
		}
		set
		{
			PlayerPrefs.SetInt("TutorialIsPlayed", value ? 1 : 0);
			PlayerPrefs.Save();
		}
	}

	public bool IsUserRated {
		get{
			return PlayerPrefs.GetInt("IsUserRated", 0) == 1;
		}
		set
		{
			PlayerPrefs.SetInt("IsUserRated", value ? 1 : 0);
			PlayerPrefs.Save();
		}
	}

	public DateTime NextRateUsDateTime {
		get{
			return OmrTime.GetAsDate(PlayerPrefs.GetInt("NextRateUsOmrTime", 0));
		}
		set
		{
			var oTime = OmrTime.GetAsOmrTime(value);
			PlayerPrefs.SetInt("NextRateUsOmrTime", oTime);
			PlayerPrefs.Save();
		}
	}

	public Queue<CubeJumpHistoryItem> CurrentJumps = new Queue<CubeJumpHistoryItem>();

	public void Initialize() {
		IsPaused = false;
		IsDead = false;
		CurrentJumpCount = 0;
		CurrentScore = 0f;
		BestScore = PlayerPrefs.GetInt("BestScore", 0);
		PlayCount = PlayCount + 1;

		var gameOverAudioSourceObject = GameObject.FindGameObjectWithTag("GameOverAudioSource");
		var gameOverAudioSource = gameOverAudioSourceObject.GetComponent<AudioSource>();
		gameOverAudioSource.volume = 0f;
		gameOverAudioSource.Play();

		PlayerPrefs.Save();
	}

	public void Resume(){
		IsPaused = false;
	}

	public void Pause()
	{
		IsPaused = true;
	}

	public void AddJump(bool isOne){
		++CurrentJumpCount;

		CurrentScore += Mathf.Max(CameraSpeed,1);
	}

	public void GameOver ()
	{
		IsDead = true;
		IsPaused = true;

		var gameOverAudioSource = GameObject.FindGameObjectWithTag("GameOverAudioSource").GetComponent<AudioSource>();
		gameOverAudioSource.volume = 1f;
		gameOverAudioSource.time = 0f;
		gameOverAudioSource.Play();

		if(IsFirstPlayMode)
			IsFirstPlayMode = false;

		var mainCanvas = GameObject.FindGameObjectWithTag("MainCanvasTag");
		var gameOverPanel = Instantiate(GameOverPanel);
		gameOverPanel.transform.SetParent(mainCanvas.transform);
		var rectTransform = gameOverPanel.GetComponent<RectTransform>();
		rectTransform.anchoredPosition = Vector2.zero;
		rectTransform.transform.localScale =Vector3.one;

		if(Game.Instance.CurrentJumpCount > BestScore){
			BestScore = Game.Instance.CurrentJumpCount;
			PlayerPrefs.SetInt("BestScore", BestScore);

			GoogleHelper.Instance.ReportScore(GooglePlayServicesConstants.leaderboard_leadership, BestScore);
		}
	}
}