using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class TutorialBehavior : MonoBehaviour {

	private int _jumpIndex;
	private bool _oneJumpEnabled = false;
	private bool _twoJumpEnabled = false;
	private bool _freeJump = false;
	private bool _finished = false;
	private float oneJumpLastTime;
	private float twoJumpLastTime;
	private CubeControllerBehavior _cubeController;
	private GenerateAndRepeatBlocksBehavior _generateAndRepeatBlocksBehavior;
	private FollowCameraBehavior _followCameraBehavior;
	private Vector3 _blockDefaultPosition;

	public float TimeFactor = 1f;
	public int MaxJumpCount = 10;
	public GameObject Camera;
	public GameObject Cube;
	public GameObject Blocks;
	public GameObject Sound;
	public Image Phone;
	public Image PhoneGroupLeft;
	public Image PhoneGroupRight;
	public Image HandLeft;
	public Image HandRight;
	public Text MainText;
	public RectTransform SkipTutorialPanel;
	
	void Awake () {
		Game.Instance.IsInTutorialMode = true;

		_cubeController = Cube.GetComponent<CubeControllerBehavior>();
		_cubeController.MinJumpSpeedFactor = 0.0001f;
		_cubeController.JumpSpeedFactor = 0.0001f;
		Game.Instance.CameraSpeed = 0.0001f;

		_generateAndRepeatBlocksBehavior = Blocks.GetComponent<GenerateAndRepeatBlocksBehavior>();
		_generateAndRepeatBlocksBehavior.PredefinedFortune = new List<bool>() {
			true, true, true, true, true, false, true, true, false, true, true, true, true, true, true, true, true, true, true, true, true, true, false
		};

		_followCameraBehavior = Camera.GetComponent<FollowCameraBehavior>();
		_followCameraBehavior.FollowXMargin = 0f;

		Phone.color = new Color(0f, 0f, 0f, 0f);
		PhoneGroupLeft.color = new Color(0f, 0f, 0f, 0f);
		PhoneGroupRight.color = new Color(0f, 0f, 0f, 0f);

		HandLeft.color = new Color(0f, 0f, 0f, 0f);
		HandRight.color = new Color(0f, 0f, 0f, 0f);
	}

	void Start() {
		Game.Instance.CameraSpeed = 0.001f;
		_blockDefaultPosition = Blocks.transform.position;
		Blocks.SetActive(false);

		var tweenExt = gameObject.AddComponent<iTweenExt>()
				.AddAction(()=>{
					MainText.color = new Color(0f, 0f, 0f, 0f);
				}, 0.1f)
				.AddAction(()=>{
					MainText.text = Localization.Instance.GetString("TUTORIAL_0"); //Merhaba !\r\nBu küp";
					iTween.ValueTo(this.gameObject, iTween.Hash("from", 0f, "to", 1f, "onupdate", "changeMainTextColor", "time", 3 * TimeFactor, "easetype", iTween.EaseType.easeOutExpo));
				}, 3 * TimeFactor)
				.Delay(0.5f * TimeFactor)
				.AddAction(()=>{
					iTween.ValueTo(this.gameObject, iTween.Hash("from", 1f, "to", 0f, "onupdate", "changeMainTextColor", "time", 0.5 * TimeFactor, "easetype", iTween.EaseType.easeOutExpo));
					iTween.ValueTo(this.gameObject, iTween.Hash("from", 0.1f, "to", 1.5f, "onupdate", "changeCameraSpeed", "time", 4 * TimeFactor, "easetype", iTween.EaseType.easeInExpo));
				}, 0.5f * TimeFactor)
				.AddAction(()=>{
					MainText.text = Localization.Instance.GetString("TUTORIAL_1"); //"... \r\nve bunlar da bloklar";
					iTween.ValueTo(this.gameObject, iTween.Hash("from", 0f, "to", 1f, "onupdate", "changeMainTextColor", "time", 0.5 * TimeFactor, "easetype", iTween.EaseType.easeOutExpo));
					Blocks.transform.position = new Vector3(10f, _blockDefaultPosition.y, _blockDefaultPosition.z);
					Blocks.SetActive(true);
					iTween.MoveTo(Blocks, _blockDefaultPosition, 2.5f);
				}, 3f * TimeFactor)
				.AddAction(()=>{
					iTween.ValueTo(this.gameObject, iTween.Hash("from", 1f, "to", 0f, "onupdate", "changeMainTextColor", "time", 0.5 * TimeFactor, "easetype", iTween.EaseType.easeOutExpo));
				}, 0.5f * TimeFactor)
				.AddAction(()=>{
					MainText.text = Localization.Instance.GetString("TUTORIAL_2");//"Küp tek ve çift zıplayabilir";
					iTween.ValueTo(this.gameObject, iTween.Hash("from", 0f, "to", 1f, "onupdate", "changeMainTextColor", "time", 0.5 * TimeFactor, "easetype", iTween.EaseType.easeOutExpo));

					_oneJumpEnabled = true;
					JumpOne();

					_oneJumpEnabled = true;
					JumpOne();

					_twoJumpEnabled = true;
					JumpTwo();

					_twoJumpEnabled = true;
					JumpTwo();
				}, 5 * TimeFactor)
				.AddAction(()=>{
					iTween.ValueTo(this.gameObject, iTween.Hash("from", 0f, "to", 1f, "onupdate", "changePhoneAlpha", "time", 1 * TimeFactor, "easetype", iTween.EaseType.linear));
				}, 1 * TimeFactor)
				.AddAction(()=>{
					iTween.ValueTo(this.gameObject, iTween.Hash("from", 1f, "to", 0f, "onupdate", "changeMainTextColor", "time", 0.5 * TimeFactor, "easetype", iTween.EaseType.easeOutExpo));
				}, 0.5f * TimeFactor)
				.AddAction(()=>{
					MainText.text = Localization.Instance.GetString("TUTORIAL_3");//"Tek zıplamak için ekranın soluna dokun";
					iTween.ValueTo(this.gameObject, iTween.Hash("from", 0f, "to", 1f, "onupdate", "changeMainTextColor", "time", 0.5 * TimeFactor, "easetype", iTween.EaseType.easeOutExpo));
					iTween.ValueTo(this.gameObject, iTween.Hash("from", 0f, "to", 1f, "onupdate", "changePhoneLeftAlpha", "time", 1 * TimeFactor, "easetype", iTween.EaseType.linear));
					iTween.ValueTo(this.gameObject, iTween.Hash("from", 0f, "to", 1f, "onupdate", "changeMainTextColor", "time", 0.5 * TimeFactor, "easetype", iTween.EaseType.easeOutExpo));

					iTween.ValueTo(this.gameObject, iTween.Hash("from", 0f, "to", 1f, "onupdate", "changeLeftHandAlpha", "time", 1 * TimeFactor, "easetype", iTween.EaseType.linear));
					HandLeft.GetComponentInParent<Animator>().SetBool("Fade", true);
				}, 1 * TimeFactor)
				.Delay(0.5f)
				.AddAction(()=>{
					_oneJumpEnabled = true;
				}, 0.1f).AddCoroutine(WaitForJump(true))
				.Delay(0.5f * TimeFactor)
				.AddAction(()=>{
					iTween.ValueTo(this.gameObject, iTween.Hash("from", 1f, "to", 0f, "onupdate", "changeMainTextColor", "time", 0.5 * TimeFactor, "easetype", iTween.EaseType.easeOutExpo));
				}, 1 * TimeFactor)
				.AddAction(()=>{
					MainText.text = Localization.Instance.GetString("TUTORIAL_4");//"Çift zıplamak için ekranın sağına dokun";
					iTween.ValueTo(this.gameObject, iTween.Hash("from", 0f, "to", 1f, "onupdate", "changeMainTextColor", "time", 0.5 * TimeFactor, "easetype", iTween.EaseType.easeOutExpo));
					iTween.ValueTo(this.gameObject, iTween.Hash("from", 1f, "to", 0f, "onupdate", "changePhoneLeftAlpha", "time", 0.5 * TimeFactor, "easetype", iTween.EaseType.linear));
					iTween.ValueTo(this.gameObject, iTween.Hash("from", 0f, "to", 1f, "onupdate", "changePhoneRightAlpha", "time", 1 * TimeFactor, "easetype", iTween.EaseType.linear));

					iTween.ValueTo(this.gameObject, iTween.Hash("from", 0f, "to", 1f, "onupdate", "changeRightHandAlpha", "time", 1 * TimeFactor, "delay", 0.5f, "easetype", iTween.EaseType.linear));
					HandRight.GetComponentInParent<Animator>().SetBool("Fade", true);

					iTween.ValueTo(this.gameObject, iTween.Hash("from", 1f, "to", 0f, "onupdate", "changeLeftHandAlpha", "time", 1 * TimeFactor, "easetype", iTween.EaseType.linear));
					HandLeft.GetComponentInParent<Animator>().SetBool("Fade", false);
				}, 2 * TimeFactor)
				.Delay(0.5f)
				.AddAction(()=>{
					_twoJumpEnabled = true;
				}, 0.1f).AddCoroutine(WaitForJump(false))
				.AddAction(()=>{
					iTween.ValueTo(this.gameObject, iTween.Hash("from", 1f, "to", 0f, "onupdate", "changeMainTextColor", "time", 0.5 * TimeFactor, "easetype", iTween.EaseType.easeOutExpo));
					iTween.ValueTo(this.gameObject, iTween.Hash("from", 1f, "to", 0f, "onupdate", "changeRightHandAlpha", "time", 1 * TimeFactor, "easetype", iTween.EaseType.linear));
					HandRight.GetComponentInParent<Animator>().SetBool("Fade", false);
				}, 1 * TimeFactor)
				.AddAction(()=>{
					iTween.ValueTo(this.gameObject, iTween.Hash("from", 1f, "to", 0f, "onupdate", "changePhoneRightAlpha", "time", 0.5 * TimeFactor, "easetype", iTween.EaseType.linear));
					iTween.ValueTo(this.gameObject, iTween.Hash("from", 1f, "to", 0f, "onupdate", "changePhoneAlpha", "time", 1 * TimeFactor, "easetype", iTween.EaseType.linear));
					MainText.text = Localization.Instance.GetString("TUTORIAL_5");//"Mükemmel ! \r\n Bir kaç sefer daha zıpla";
					_freeJump = true;
					iTween.ValueTo(this.gameObject, iTween.Hash("from", 0f, "to", 1f, "onupdate", "changeMainTextColor", "time", 0.5 * TimeFactor, "easetype", iTween.EaseType.easeOutExpo));
				}, 2 * TimeFactor)
				;

		StartCoroutine(tweenExt.Run());
	}

	void Update() {
		if(!_finished && MaxJumpCount < _jumpIndex + 1)
		{
			_finished = true;
			_freeJump = false;
			_oneJumpEnabled = false;
			_twoJumpEnabled = false;

			Destroy(GetComponent<iTweenExt>());
			var tweenExt = gameObject.AddComponent<iTweenExt>()
			.AddAction(()=>{
				iTween.ValueTo(this.gameObject, iTween.Hash("from", 1f, "to", 0f, "onupdate", "changeMainTextColor", "time", 1 * TimeFactor, "easetype", iTween.EaseType.easeOutExpo));
			}, 1 * TimeFactor)
			.Delay(0.5f * TimeFactor)
			.AddAction(()=>{
						MainText.text = Localization.Instance.GetString("TUTORIAL_6");//"Artık oynamaya hazırsın !";
				iTween.ValueTo(this.gameObject, iTween.Hash("from", 0f, "to", 1f, "onupdate", "changeMainTextColor", "time", 1 * TimeFactor, "easetype", iTween.EaseType.easeOutExpo));
			}, 1 * TimeFactor)
			.Delay(1f * TimeFactor)
			.AddAction(()=>{
				MainText.text = Localization.Instance.GetString("TUTORIAL_7");//"Oyun yükleniyor";
				iTween.ValueTo(this.gameObject, iTween.Hash("from", 0f, "to", 1f, "onupdate", "changeMainTextColor", "time", 1 * TimeFactor, "easetype", iTween.EaseType.easeOutExpo));
			}, 1 * TimeFactor)
			.Delay(1f * TimeFactor)
			.AddAction(()=>{
				SkipTutorial();
			}, 1);

			StartCoroutine(tweenExt.Run());
		}
		else
		{	
			//Keyboard
			if (Input.GetKeyDown (KeyCode.Space)) {
				JumpOne();
			}
			else if (Input.GetKeyDown (KeyCode.Return)) {
				JumpTwo ();
			}
			
			//Touch
			if (Input.touchCount == 1 && Input.touches [0].phase == TouchPhase.Began) {
				var touch = Input.touches [0];

				if (touch.position.x < Screen.width / 2) {
					JumpOne();
				}
				else if (touch.position.x > Screen.width / 2) {
					if(touch.position.y > Screen.height - 150f)
						return;

					JumpTwo ();
				}
			}
		}
	}

	IEnumerator WaitForJump (bool isOne)
	{
		float time = 100f;

		while(time > 1f)
		{
			time = Time.time - (isOne ? oneJumpLastTime : twoJumpLastTime);
			yield return new WaitForSeconds(0.3f);
		}
	}

	IEnumerator WaitUntilTouchCountAt (int count)
	{
		var current = _jumpIndex;

		while(_jumpIndex < current + count)
		{
			yield return new WaitForSeconds(0.5f);
		}
	}
	
	public void changeLeftHandAlpha(float f){
		HandLeft.color = new Color(1f, 1f, 1f, f);
	}

	public void changeRightHandAlpha(float f){
		HandRight.color = new Color(1f, 1f, 1f, f);
	}

	
	public void changePhoneAlpha(float f){
		Phone.color = new Color(1f, 1f, 1f, f);
	}

	public void changePhoneLeftAlpha(float f){
		PhoneGroupLeft.color = new Color(1f, 1f, 1f, f);
	}
	
	public void changePhoneRightAlpha(float f){
		PhoneGroupRight.color = new Color(1f, 1f, 1f, f);
	}

	public void changeMainTextColor(float f){
		MainText.color = new Color(0f, 0f, 0f, f);
	}

	public void changeCameraSpeed(float f) {
		_cubeController.JumpSpeedFactor = f;
	}

	void JumpOne ()
	{
		if (!_freeJump && !_oneJumpEnabled)
			return;
		_oneJumpEnabled = false;
		_cubeController.JumpCommands.Enqueue (true);

		_jumpIndex += 1;
		oneJumpLastTime = Time.time;
	}

	void JumpTwo ()
	{
		if (!_freeJump && !_twoJumpEnabled)
			return;
		_twoJumpEnabled = false;
		_cubeController.JumpCommands.Enqueue (false);
		_jumpIndex += 2;
		twoJumpLastTime = Time.time;
	}

	public void SkipTutorial() {
		Game.Instance.IsInTutorialMode = false;
		Game.Instance.TutorialIsPlayed = true;
		Application.LoadLevel("Level");
	}
}
