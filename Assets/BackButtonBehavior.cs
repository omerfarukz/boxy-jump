using UnityEngine;

public class BackButtonBehavior : MonoBehaviour {

	public string GotoLevel;

	void Update() {

		if (Input.GetKeyDown(KeyCode.Escape)) 
		{
			if(!string.IsNullOrEmpty(GotoLevel))
			{
				Application.LoadLevel(GotoLevel);
				return;
			}
			else
			{
				Application.Quit(); 
			}
		}
			
	}

	public void GotoMain() {
		Application.LoadLevel("Main");

		return;
	}
}
