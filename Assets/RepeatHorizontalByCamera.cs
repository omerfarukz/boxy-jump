using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class RepeatHorizontalByCamera : MonoBehaviour {

	private int _count = 0;
	private Queue<GameObject> _instanceQueue = new Queue<GameObject>();
	private Renderer _objectRenderer;
	private Camera _camera;

	public GameObject ObjectToClone;
	public Vector3 Margin = Vector3.zero;
	public Vector3 Padding = Vector3.zero;
	public int FirstGenerationCount = 20;
	public int GenerationXDistance = 5;
	public int SwapCount = 1;

	void Start () {
		_camera = this.GetComponent<Camera>();

		_objectRenderer = ObjectToClone.GetComponent<Renderer>();

		Generate(FirstGenerationCount);
	}
	
	// Update is called once per frame
	void Update () {
		if(Game.Instance.IsPaused || Game.Instance.IsDead)
			return;

		if(_instanceQueue.Count <= 1)
			return;

		var lastInstance = _instanceQueue.Last();

		if(lastInstance.transform.position.x - _camera.transform.position.x < GenerationXDistance){
			SwapToLast(SwapCount);
		}
	}

	void Generate(int count)
	{
		for (int i = _count; i < _count + count; i++) {
			var newObject = Instantiate(ObjectToClone);
			newObject.transform.position = GetPosition(i);
			
			_instanceQueue.Enqueue(newObject);
		}
	}

	void SwapToLast(int count){
		for (int i = 0; i < count; i++) {
			var first = _instanceQueue.Dequeue();
			first.transform.position = GetPosition(++_count);
			_instanceQueue.Enqueue(first);
		}
	}

	Vector3 GetPosition(int i){
		return Padding * i + Margin + new Vector3(
			i*(_objectRenderer.bounds.center.x + (_objectRenderer.bounds.size.x)), 
			ObjectToClone.transform.position.y, 
			ObjectToClone.transform.position.z
		);
	}
}
