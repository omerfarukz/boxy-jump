using UnityEngine;
using System.Collections;

public class ChangeColorOnCollision : MonoBehaviour {

	private Color _startColor;
	private Renderer _renderer;
	private bool _isStarted;
	private float _lastHit;

	public Color TargetColor;
	public float Speed = 0.5f;

	void Start () {
		_renderer = GetComponent<Renderer>();
		_startColor = _renderer.material.color;

		_isStarted = true;
	}

	void Update(){
		if(Game.Instance.IsDead || Game.Instance.IsPaused)
			return;
	}

	void OnTriggerEnter(Collider collider){
		if(!_isStarted)
			return;

		iTween.Stop(this.gameObject);
		iTween.ColorTo(this.gameObject, TargetColor, Speed); //new Color32(255,43,91,255), 0.5f);
		GetComponent<iTween>().easeType = iTween.EaseType.easeInBounce;
	}

	void OnTriggerExit(Collider collider){
		if(!_isStarted)
			return;

		iTween.Stop(this.gameObject);
		iTween.ColorTo(this.gameObject, _startColor, 2f);
		GetComponent<iTween>().easeType = iTween.EaseType.easeOutBounce;
	}
}
