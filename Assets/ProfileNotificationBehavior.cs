using UnityEngine;
using System.Collections;

public class ProfileNotificationBehavior : MonoBehaviour
{

	void Start(){
		StartCoroutine(CheckNewInstall());
	}

	public IEnumerator GameOver(int score, int jumpCount, int bestScore)
	{
		WebRequest request = new WebRequest();
		yield return StartCoroutine(request.MakeGET("&gameover=1&jumpCount=" + jumpCount + "&score=" + score + "&bestScore=" + bestScore));
	}

	public IEnumerator CheckNewInstall()
	{
		if(PlayerPrefs.HasKey("newinstallnotified"))
			yield break;

		WebRequest request = new WebRequest();
		yield return StartCoroutine(request.MakeGET("&new=1"));

		PlayerPrefs.SetInt("newinstallnotified", 1);
	}
}

