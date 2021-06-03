using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;

public class JumpCommandPanelBehavior : MonoBehaviour {

	private Queue<bool> _queue = new Queue<bool>();
	private List<RectTransform> _commandPanelInstances = new List<RectTransform>();

	public GameObject Cube;
	public RectTransform CommandPanel;
	public int Count = 5;
	public Color One;
	public Color Two;
	public Color Empty;

	// Use this for initialization
	void Start () {
		var cubeController = Cube.GetComponent<CubeControllerBehavior>();
		_queue = cubeController.JumpCommands;
		
		for (int i = 0; i < 10; i++) {
			var newCommandPanelInstance = Instantiate(CommandPanel);

			newCommandPanelInstance.SetParent(this.transform);
			newCommandPanelInstance.gameObject.SetActive(false);
			_commandPanelInstances.Add(newCommandPanelInstance);
		}

		var gridLayoutSystem = this.gameObject.AddComponent<GridLayoutGroup>();
		gridLayoutSystem.childAlignment = TextAnchor.MiddleLeft;
		gridLayoutSystem.cellSize = new Vector2(10f, 10f);
		gridLayoutSystem.spacing = new Vector2(20f, 0f);
		gridLayoutSystem.padding = new RectOffset(20,40,20,20);
		StartCoroutine(InitPanels());
	}
	
	// Update is called once per frame
	void Update () {
		if(Game.Instance.IsPaused || Game.Instance.IsDead)
			return;

		if(_queue == null)
		{
			Debug.LogWarning("Queue is null");
		}

		foreach (var item in _commandPanelInstances) {
			item.gameObject.SetActive(false);
		}

		var queueAsArray = _queue.Take(Count).ToArray();
		for (int i = 0; i < Count; i++) {
			if(i<queueAsArray.Length)
			{
				if(queueAsArray[i]) // one jump
				{
					_commandPanelInstances[i].GetComponent<Image>().color = One;
				}
				else
				{
					_commandPanelInstances[i].GetComponent<Image>().color = Two;
				}
			}
			else
			{
				_commandPanelInstances[i].GetComponent<Image>().color = Empty;
			}

			_commandPanelInstances[i].gameObject.SetActive(true);
		}
	}

	IEnumerator InitPanels(){


		yield return new WaitForEndOfFrame();


	}
}
