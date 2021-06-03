using System.Collections;
using UnityEngine;

public class WebRequest
{
	private string _baseUrl = "http://zor.lu/games.php?name=boxy&version=1";
	
	public string Text {
		get;
		set;
	}
	
	public string Error {
		get;
		set;
	}
	
	public IEnumerator MakeGET(string prm)
	{
		#if UNITY_EDITOR
		Debug.Log("ProfileNotification is disabled on editor");
		yield break;
		#endif
		
		string url = _baseUrl + GetDefaultPrms() + prm;
		Debug.Log(url);
		
		WWW www = new WWW(url);
		yield return www;
		
		Text = www.text;
		Error = www.error;
	}
	
	string GetDefaultPrms() {
		string prm = string.Empty;
		if(SystemInfo.deviceUniqueIdentifier!=null)
			prm += "&uid=" + WWW.EscapeURL(SystemInfo.deviceUniqueIdentifier);
		
		if(SystemInfo.deviceName != null)
			prm += "&dev=" + WWW.EscapeURL(SystemInfo.deviceName);
		
		if(Social.localUser != null && Social.localUser.authenticated && Social.localUser.userName != null)
			prm += "&user=" + WWW.EscapeURL(Social.localUser.userName);
		
		return prm;
	}
}