using UnityEngine;
using System.Collections;

public class ArrowBehavior : MonoBehaviour {

	private const float MaxValue = -270;
	private bool _shaking = false;

	public float Speed = 4;
	public int MaxSpeed = 10;

	void Update () {
		if(Game.Instance.IsDead)
		{
			GetComponent<Animator>().SetBool("Shaking", false);
			_shaking = false;
			return;
		}
		else if(Game.Instance.IsPaused)
		{
			GetComponent<Animator>().SetBool("Shaking", false);
			this.transform.localRotation = Quaternion.Euler(Vector3.zero);
			_shaking = false;
			return;
		}
		else if(_shaking == false)
		{
			GetComponent<Animator>().SetBool("Shaking", true);
			_shaking = true;
		}

		this.Speed = Game.Instance.JumpSpeed;

		float currentAngle = (MaxValue / MaxSpeed)  * Mathf.Min(Mathf.Max(0, this.Speed), MaxSpeed);
		this.transform.localRotation = Quaternion.Euler(new Vector3(0f, 0f, currentAngle));
	}

	void OnGUI(){
		if(GUI.Button(new Rect(10f, 10f, 150f , 150f), "Back"))
		{
			Application.LoadLevel("Level");
		}
	}
}
