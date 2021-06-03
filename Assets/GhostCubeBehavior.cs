using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public class GhostCubeBehavior : MonoBehaviour {

	private float _gamePlayTime;
	private List<CubeJumpHistoryItem> _previousJumps;
	private Animator _animator;
	private float _cubePositionX;

	public GameObject Parent;

	void Start () {
		_animator = GetComponent<Animator>();

		_previousJumps = new List<CubeJumpHistoryItem>(Game.Instance.CurrentJumps.ToArray());

		if(_previousJumps==null || _previousJumps.Count == 0)
		{
			//Parent.transform.position = new Vector3(-25000,0,0);
			this.gameObject.SetActive(false);
			return;
		}

		Game.Instance.CurrentJumps.Clear();
	}
	
	void Update () {
		if(Game.Instance.IsPaused || Game.Instance.IsDead)
		{
			iTween.Pause(Parent);
			_animator.speed = 0.0000001f;
			return;
		}

		iTween.Resume(Parent);

		_gamePlayTime += Time.deltaTime;

		if(_previousJumps.Count > 0)
		{
			if(_previousJumps[0].Time < _gamePlayTime){
				_animator.speed = _previousJumps[0].Speed;

				if(_previousJumps[0].IsOneJump)
				{
					JumpOne(_previousJumps[0].Speed);
				}
				else
				{
					JumpTwo(_previousJumps[0].Speed);
				}

				_previousJumps.RemoveAt(0);
			}
		}
	}

	public void JumpOne(float JumpSpeedFactor){
		_animator.SetTrigger("IdleToOne");
		MoveTo(1.5f, 0.6f, JumpSpeedFactor);
	}

	public void JumpTwo(float JumpSpeedFactor){
		_animator.SetTrigger("IdleToTwo");
		MoveTo(3f, 0.8f, JumpSpeedFactor);
	}

	public void MoveTo(float xpos, float speed, float factor)
	{
		iTween.Stop(Parent);

		iTween.MoveTo(Parent, iTween.Hash("x", _cubePositionX + xpos, "easeType", "linear", "time", speed / factor));

		_cubePositionX += xpos;
	}
}
