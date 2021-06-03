using System.Collections;
using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;

public class GoogleHelper
{
	public static GoogleHelper Instance = new GoogleHelper();

	private bool _isAuthenticating;
	private bool _initialized;

	public bool IsAuthenticated {
		get {
			return PlayGamesPlatform.Instance.IsAuthenticated() && PlayGamesPlatform.Instance.localUser.authenticated;
		}
	}

	public bool IsSocialAvailable {
		get{
			return (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer);
		}
	}

	#region "Social"

	public IEnumerator InitAndAuthenticate(float delay) {
		Debug.Log("a");

		if(!IsSocialAvailable)
			yield break;

		Debug.Log("b");

		yield return new WaitForSeconds(delay);

		_isAuthenticating = false;

		Debug.Log("c");

		if(!_initialized)
		{
			PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().Build();
			PlayGamesPlatform.InitializeInstance(config);
			if(Debug.isDebugBuild)
			{
				PlayGamesPlatform.DebugLogEnabled = true;
			}
		}

		if(!IsAuthenticated)
		{
			_isAuthenticating = true;

			PlayGamesPlatform.Instance.Authenticate((result) => {
				_isAuthenticating = false;
			});

			while(_isAuthenticating)
			{
				yield return new WaitForSeconds(0.5f);
			}
		}
	}

	public void ReportScore(string board, int score) {
		if(!IsSocialAvailable)
		{
			Debug.Log("Social not available");
			return;
		}

		if(!IsAuthenticated)
		{
			Debug.Log("Player is not authenticated");
			return;
		}

		PlayGamesPlatform.Instance.ReportScore(score, board, (result)=>{
			Debug.Log ("Reported " + score + " score to " + board + " board. Result is " + result);
		});
	}

	public void UnlockArchivement(string archivementId) {
		if(!IsSocialAvailable || !IsAuthenticated)
		{
			Debug.Log("Social not available");
			return;
		}

		PlayGamesPlatform.Instance.ReportProgress(archivementId, 100f, (result)=>{
			Debug.Log ("Reported " + archivementId + " archivement is unlocked. Result is " + result);
		});
	}

	public void ShowLeadersBoard(string board) {
		if(!IsSocialAvailable || !IsAuthenticated)
		{
			Debug.Log("player not authenticated or social is unavailable");
			return;
		}

		PlayGamesPlatform.Instance.ShowLeaderboardUI(board, (result)=>{
			Debug.Log("ShowLeaderboardUI status is " + result);
		});
	}

	#endregion
}