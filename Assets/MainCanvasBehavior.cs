using UnityEngine;
using System.Collections;

public class MainCanvasBehavior : MonoBehaviour {

	public GameObject TutorialPanel;

	void Awake(){
		GetComponent<Canvas>().overrideSorting = true;
	}

	void Start () {
		if(Game.Instance.IsFirstPlayMode)
		{
//			var tutorialPanel = Instantiate(TutorialPanel);
//			tutorialPanel.transform.SetParent(this.transform);
//			var rectTransform = tutorialPanel.GetComponent<RectTransform>();
//			//rectTransform.anchoredPosition = Vector2.zero;
//			rectTransform.transform.localScale = Vector3.one;
		}

	}
}
