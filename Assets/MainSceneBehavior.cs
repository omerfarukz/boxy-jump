using UnityEngine;
using System.Collections;

public class MainSceneBehavior : MonoBehaviour {

	public GameObject SoundSpriteObject;
	public Sprite SoundOnSprite;
	public Sprite SoundOffSprite;

	void Start(){
		UnityEngine.Application.targetFrameRate = 30;

		if(PlayerPrefs.GetInt("GameCenterIsOpened", 0) == 1)
		{
			StartCoroutine(GoogleHelper.Instance.InitAndAuthenticate(4f));
		}
	}

	void Update() {
		CheckSoundState();

		if (Input.touchCount == 1 && Input.touches[0].phase == TouchPhase.Began)
		{
			var ray = Camera.main.ScreenPointToRay (Input.GetTouch(0).position);
			RaycastHit hit;
			if (Physics.Raycast (ray, out hit)) 
			{
				Open(hit.collider.name);
			}
		}
		else if(Input.GetMouseButtonDown(0)) {
			var ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast (ray, out hit)) 
			{
				Open(hit.collider.name);
			}
		}
	}

	void CheckSoundState ()
	{
		if(Game.Instance.SoundIsOn)
		{
			AudioListener.volume = 1;
			SoundSpriteObject.GetComponent<SpriteRenderer>().sprite = SoundOnSprite;
		}
		else
		{
			AudioListener.volume = 0;
			SoundSpriteObject.GetComponent<SpriteRenderer>().sprite = SoundOffSprite;
		}
	}

	void Open(string name)
	{
		if(name == "playblock" || name == "GoodBlock" || name == "Cube")
		{
			if(Game.Instance.TutorialIsPlayed)
			{
				Application.LoadLevel("Level");
			}
			else
			{
				Application.LoadLevel("Tutorial");
			}
		}
		else if(name == "starblock")
		{
			StartCoroutine(LoginAndShowLeaderBoards());
		}
		else if(name == "soundblock")
		{
			Game.Instance.SoundIsOn = !Game.Instance.SoundIsOn;
		}
	}

	IEnumerator LoginAndShowLeaderBoards() {
		PlayerPrefs.SetInt("GameCenterIsOpened", 1);

		yield return StartCoroutine(GoogleHelper.Instance.InitAndAuthenticate(0f));

		GoogleHelper.Instance.ReportScore(GooglePlayServicesConstants.leaderboard_leadership, Game.Instance.BestScore);

		yield return new WaitForSeconds(0.3f);

		GoogleHelper.Instance.ShowLeadersBoard(GooglePlayServicesConstants.leaderboard_leadership);
	}
}