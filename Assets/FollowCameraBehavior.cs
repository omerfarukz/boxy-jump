	using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

public class FollowCameraBehavior : MonoBehaviour {

	private float _gamePlayTime;
	private float _xVelocity = 0f;
	private float _cameraHiddenX = 0f;
	private Dictionary<float, float> _timeSpeedRanges = new Dictionary<float, float>();
	private Queue<int> _levelPoints = new Queue<int>();
	private Queue<string> _levelArchivements = new Queue<string>();

	private int _currentLevelPoint = 0;

	public Transform Target;
	public float DefaultFollowSpeed;
	public float DefaultCameraXAxisSpeed;
	public string TimeSpeedExpression;
	public float FollowXMargin = 2f;
	public float CameraSpeed = 0f;

	public GameObject[] LevelNumbers;

	void Start () {
		UnityEngine.Application.targetFrameRate = 30;

		_levelPoints.Enqueue(10); // 1
		_levelArchivements.Enqueue(GooglePlayServicesConstants.achievement_level_1); // 1

		_levelPoints.Enqueue(64); // 2
		_levelArchivements.Enqueue(GooglePlayServicesConstants.achievement_level_2); // 2

		_levelPoints.Enqueue(128); // 3
		_levelArchivements.Enqueue(GooglePlayServicesConstants.achievement_level_3); // 3

		_levelPoints.Enqueue(256); // 4
		_levelArchivements.Enqueue(GooglePlayServicesConstants.achievement_level_4); // 4

		_levelPoints.Enqueue(512); // 5
		_levelArchivements.Enqueue(GooglePlayServicesConstants.achievement_level_5); // 5

		_levelPoints.Enqueue(1024); // 6
		_levelArchivements.Enqueue(GooglePlayServicesConstants.achievement_level_6); // 6

		_levelPoints.Enqueue(1600); // 7
		_levelArchivements.Enqueue(GooglePlayServicesConstants.achievement_level_7); // 7

		_levelPoints.Enqueue(2500); // 8
		_levelArchivements.Enqueue(GooglePlayServicesConstants.achievement_level_8); // 8

		_levelPoints.Enqueue(4000); // 9
		_levelArchivements.Enqueue(GooglePlayServicesConstants.achievement_level_9); // 9

		_levelPoints.Enqueue(5000); // 10
		_levelArchivements.Enqueue(GooglePlayServicesConstants.achievement_level_10); // 10


		CameraSpeed = DefaultCameraXAxisSpeed;

		var currentPosition = this.transform.position;
		currentPosition.x = FollowXMargin;
		this.transform.position = currentPosition;

		InitTimeSpeedExpression ();
	}

	void Update(){
		if(Game.Instance.IsPaused || Game.Instance.IsDead)
			return;

		_gamePlayTime += Time.deltaTime;

		if(!Game.Instance.IsInTutorialMode)
		{
			SetFollowSpeedByTimeSpeedRanges();

			if(_levelPoints.First() < Game.Instance.CurrentJumpCount)
			{
				_levelPoints.Dequeue();
				var currentArchivementId = _levelArchivements.Dequeue();

				GoogleHelper.Instance.UnlockArchivement(currentArchivementId);

				AddCurrentLevelNumber(_currentLevelPoint++);
			}
		}
	}

	void LateUpdate () {
		if(Game.Instance.IsPaused || Game.Instance.IsDead)
			return;

		UpdatePosition ();
	}

	void InitTimeSpeedExpression ()
	{
		if (string.IsNullOrEmpty (TimeSpeedExpression))
			return;
		
		var timeSpeedRanges = TimeSpeedExpression.Split (",".ToCharArray (), StringSplitOptions.RemoveEmptyEntries);
		foreach (var current in timeSpeedRanges) {
			var timeSpeed = current.Split (":".ToCharArray (), StringSplitOptions.RemoveEmptyEntries);
			_timeSpeedRanges.Add (float.Parse (timeSpeed [0]), float.Parse (timeSpeed [1]));
		}
	}

	void AddCurrentLevelNumber(int id){
		var instance = GameObject.Instantiate(LevelNumbers[id]);
		instance.transform.position = new Vector3(this.transform.position.x + 20f, 0f, 4f);
	}

	void SetFollowSpeedByTimeSpeedRanges ()
	{
		if(_timeSpeedRanges.Count == 0)
			return;


		var firstRange = _timeSpeedRanges.First();

		if(firstRange.Key < _gamePlayTime)
		{

			CameraSpeed = firstRange.Value;
			_timeSpeedRanges.Remove(firstRange.Key);
		}

		Game.Instance.CameraSpeed = CameraSpeed;
	}

	void UpdatePosition ()
	{
		_cameraHiddenX += Time.deltaTime * CameraSpeed;
		float newXTarget = Mathf.Max (Target.position.x + FollowXMargin, _cameraHiddenX);
		float newX = Mathf.SmoothDamp (this.transform.position.x, newXTarget, ref _xVelocity, DefaultFollowSpeed * Time.deltaTime);
		this.transform.position = new Vector3 (newX, this.transform.position.y, this.transform.position.z);

		if (Target.position.x > _cameraHiddenX) {
			_cameraHiddenX = Target.position.x;
		}
	}
}