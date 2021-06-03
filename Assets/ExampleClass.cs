using UnityEngine;
using System.Collections;

public class ExampleClass : MonoBehaviour {
	public float timeScale = 1f;
	public float playScheduled = 1f;

	void Update() {
		Time.timeScale = timeScale;
		//var audiosource = GetComponent<AudioSource>();
	}

}
