using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AnimatedSpriteBehavior : MonoBehaviour {

	private float _nextTime = 0f;
	private int _spriteIndex = 0;
	private SpriteRenderer _renderer;

	public float TimeForDelay = 0.3f;
	public Sprite[] Sprites;

	void Start () {
		_renderer = GetComponent<SpriteRenderer>();
	}
	
	void Update () {
		if(_nextTime<Time.time)
		{
			_renderer.sprite = Sprites[_spriteIndex];
			_spriteIndex = (++_spriteIndex) % (Sprites.Length - 1);

			_nextTime += TimeForDelay;
		}
	}
}