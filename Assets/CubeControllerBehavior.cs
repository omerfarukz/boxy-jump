using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CubeControllerBehavior : MonoBehaviour {

	private Animator _animator;
	private float _cubePositionX;
	private bool _isJumpCommandsInProgress = false;
	//private float _startTime = -1f;
	private float _startJumpSpeedFactor = 1f;
	private AudioSource _gamePlayAudioSource;
	private AudioSource _jumpAudioSource;
	private float _gamePlayTime = 0f;

	public float JumpSpeedFactor = 1f;
	public float MinJumpSpeedFactor = 3f;
	public GameObject Parent;
	public int MaxCommandCountInQueue = 5;
	public string JumpEasingType = "linear";
	public AudioClip[] JumpOneAudioClips;
	public AudioClip[] JumpTwoAudioClips;

	private Queue<bool> _jumpCommands = new Queue<bool>();
	public Queue<bool> JumpCommands {
		get { return _jumpCommands; }
		set { _jumpCommands = value; }
	}

	public bool IsCubeWaitingForJump = false;

	void Start () {
		_animator = this.GetComponent<Animator>();
		_cubePositionX = transform.position.x;
		_startJumpSpeedFactor = JumpSpeedFactor;

		_gamePlayAudioSource = GameObject.FindGameObjectWithTag("GamePlayAudioSource").GetComponent<AudioSource>();
		_jumpAudioSource = GetComponent<AudioSource>();
	}

	void Update () {
		if(Game.Instance.IsDead)
		{
			_gamePlayAudioSource.volume = 0f;
			return;
		}

		if(Game.Instance.IsPaused)
		{
			_gamePlayAudioSource.volume = 0f;
			return;
		}

		_gamePlayTime += Time.deltaTime;
		_gamePlayAudioSource.volume = 1f;

		if(JumpCommands.Count < MaxCommandCountInQueue)
		{
			WaitForNextCommand ();
		}

		if(!_isJumpCommandsInProgress){
			_isJumpCommandsInProgress = true;
			StartCoroutine(JumpCoroutine());
		}

		if(Game.Instance.IsInTutorialMode)
		{
			//JumpSpeedFactor = Game.Instance.JumpSpeed;
			_gamePlayAudioSource.pitch = JumpSpeedFactor;
			_jumpAudioSource.pitch = _gamePlayAudioSource.pitch;
		}
		else
		{
			JumpSpeedFactor = Mathf.Max(MinJumpSpeedFactor, Game.Instance.JumpSpeed);
			_gamePlayAudioSource.pitch = JumpSpeedFactor / _startJumpSpeedFactor;
			_jumpAudioSource.pitch = _gamePlayAudioSource.pitch;
		}


		AudioListener.volume = Mathf.Min(JumpSpeedFactor, 1f);
	}

	void FixedUpdate(){
		if(Game.Instance.IsDead || Game.Instance.IsPaused)
		{
			iTween.Stop(Parent);
		}
	}

	void LateUpdate() {
		if(Game.Instance.IsPaused || Game.Instance.IsDead)
		{
			_animator.speed = 0.0000001f;
			return;
		}

		_animator.speed = JumpSpeedFactor;
	}

	IEnumerator JumpCoroutine()
	{
		//print (IsCubeWaitingForJump);

		while(JumpCommands.Count > 0 && IsCubeWaitingForJump && !Game.Instance.IsDead)
		{
			if(Game.Instance.IsDead)
			{
				JumpCommands.Clear();
				yield break;
			}

			var isFirstCommandJumpOne = JumpCommands.Dequeue();
			if(isFirstCommandJumpOne)
			{
				JumpOne();
			}
			else
			{
				JumpTwo ();
			}
			IsCubeWaitingForJump = false;

			yield return new WaitForFixedUpdate();
		}

		_isJumpCommandsInProgress = false;
	}

	void WaitForNextCommand ()
	{
		if(Game.Instance.IsInTutorialMode)
			return;

		//Keyboard
		if (Input.GetKeyDown (KeyCode.Space)) {
			JumpCommands.Enqueue(true);
		}
		else if (Input.GetKeyDown (KeyCode.Return)) {
			JumpCommands.Enqueue(false);
		}

		//Touch
		if (Input.touchCount == 1 && Input.touches [0].phase == TouchPhase.Began) {
			var touch = Input.touches [0];
			if(touch.position.y > Screen.height - 150f)
				return;

			if (touch.position.x < Screen.width / 2) {
				JumpCommands.Enqueue(true);
			}
			else if (touch.position.x > Screen.width / 2) {
				JumpCommands.Enqueue(false);
			}
		}
	}

	public void JumpOne(){
		_animator.SetTrigger("IdleToOne");
		MoveTo(1.5f, 0.6f);

		_jumpAudioSource.PlayOneShot(JumpOneAudioClips[Random.Range(0, JumpOneAudioClips.Length)]);

		//mix together
		Game.Instance.AddJump(true);
		Game.Instance.CurrentJumps.Enqueue(new CubeJumpHistoryItem(_gamePlayTime, JumpSpeedFactor, true));
	}
	
	public void JumpTwo(){
		_animator.SetTrigger("IdleToTwo");
		MoveTo(3f, 0.8f);

		_jumpAudioSource.PlayOneShot(JumpTwoAudioClips[Random.Range(0, JumpOneAudioClips.Length)]);

		//mix together
		Game.Instance.AddJump(false);
		Game.Instance.CurrentJumps.Enqueue(new CubeJumpHistoryItem(_gamePlayTime, JumpSpeedFactor, false));
	}

	public void MoveTo(float xpos, float speed)
	{
		iTween.Stop(Parent);
		if(Game.Instance.IsDead)
			return;

		iTween.MoveTo(Parent, iTween.Hash("x", _cubePositionX + xpos, "easeType", JumpEasingType, "time", speed / JumpSpeedFactor));

		_cubePositionX += xpos;
	}
}
