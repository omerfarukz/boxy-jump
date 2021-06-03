using UnityEngine;
using System.IO;
using System.Collections;
using System.Runtime.InteropServices;
//using NativeAlert;

public class ShareBehavior : MonoBehaviour
{
	private string LastScreenShotPath;

	#if UNITY_IOS

	[System.Runtime.InteropServices.DllImport("__Internal")]
	extern static public void _showShareDialog(string title, string url, string imagepath);
	
	[System.Runtime.InteropServices.DllImport("__Internal")]
	extern static public void _showMessage(string title, string message);

	[System.Runtime.InteropServices.DllImport("__Internal")]
	extern static public void _showRateUs(string url, string title, string message, string yesTitle, string noTitle);
	
	public delegate void UnityCallbackDelegate(string objectName, string commandName, string commandData);
	
	[System.Runtime.InteropServices.DllImport("__Internal")]
	public static extern void _connectCallback([MarshalAsAttribute(UnmanagedType.FunctionPtr)]UnityCallbackDelegate callbackMethod);

	#endif
//
//	void OnEnable()
//	{
//		NativeAlertListener.onFinish += OnAlertFinish;
//	}
//	
//	void OnDisable()
//	{
//		NativeAlertListener.onFinish -= OnAlertFinish;
//	}
	
	void OnAlertFinish(string clickedBtn)
	{
		if (clickedBtn == "Yes" || clickedBtn == "Rate Now") {

			//Application.OpenURL(Settings.APP_REDIRECT_URL);

			if(Settings.IsAmazonBuild)
			{
				Application.OpenURL(Settings.APP_AMAZON_URL);
			}
			else
			{
				Application.OpenURL(Settings.APP_GOOGLE_URL);
			}
		}
	}

	void Start() {
		#if UNITY_IOS
		
		ShareBehavior._connectCallback(RealCallback);
		
		#endif
	}

	#if UNITY_IOS
	[MonoPInvokeCallback(typeof(UnityCallbackDelegate))]
	public static void RealCallback(string objectName, string commandName, string commandData){
		GameObject go = GameObject.Find(objectName);
		if(go != null)
		{
			go.SendMessage(commandName, commandData);
		}
	}
#endif

	public void ShowRateUs(){
#if UNITY_IOS
		_showRateUs(Settings.APP_IOS_URL, Localization.Instance.GetString("RATEUS_TITLE"), Localization.Instance.GetString("RATEUS_MESSAGE"), Localization.Instance.GetString("RATEUS_YES"), Localization.Instance.GetString("RATEUS_NO"));
#endif
#if UNITY_ANDROID
		showAndroidDialog(Localization.Instance.GetString("RATEUS_TITLE"), Localization.Instance.GetString("RATEUS_MESSAGE"), Localization.Instance.GetString("RATEUS_YES"), Localization.Instance.GetString("RATEUS_NO"));
#endif
		Game.Instance.NextRateUsDateTime = System.DateTime.Now.AddDays(3);
	}

	public IEnumerator ShareScreenshot(string text)
	{
		if(Application.isEditor)
		{
			Debug.Log("Screenshot is not supported on editor");
			yield break;
		}

		Debug.Log("Taking screen shot");

		yield return StartCoroutine(TakeScreenShot());

		Debug.Log("ScreenShot took");

		if(Application.platform == RuntimePlatform.Android)
		{
			Debug.Log("Sharing screenshot on android");
#if UNITY_ANDROID
			AndroidJavaClass intentClass = new AndroidJavaClass("android.content.Intent");
			AndroidJavaObject intentObject = new AndroidJavaObject("android.content.Intent");
			intentObject.Call<AndroidJavaObject>("setAction", intentClass.GetStatic<string>("ACTION_SEND"));
			AndroidJavaClass uriClass = new AndroidJavaClass("android.net.Uri");
			AndroidJavaObject uriObject = uriClass.CallStatic<AndroidJavaObject>("parse","file://" + LastScreenShotPath);
			intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_STREAM"), uriObject);
			intentObject.Call<AndroidJavaObject>("setType", "image/jpeg");
			if(!string.IsNullOrEmpty(text)) {
				intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_TEXT"), text);
			}
			AndroidJavaClass unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
			AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject>("currentActivity");
			
			currentActivity.Call("startActivity", intentObject);
#endif
		}
		else if(Application.platform == RuntimePlatform.IPhonePlayer)
		{
#if UNITY_IOS
			_showShareDialog (text, string.Empty, LastScreenShotPath);
#endif
		}
	}

	public IEnumerator TakeScreenShot() {
		yield return new WaitForEndOfFrame();

		Texture2D screenTexture = new Texture2D(Screen.width, Screen.height,TextureFormat.RGB24,true);
		screenTexture.ReadPixels(new Rect(0f, 0f, Screen.width, Screen.height),0,0);
		screenTexture.Apply();
		
		byte[] dataToSave = screenTexture.EncodeToPNG();
		LastScreenShotPath = Path.Combine(Application.persistentDataPath,System.DateTime.Now.ToString("yyyy-MM-dd-HHmmss") + ".png");

		File.WriteAllBytes(LastScreenShotPath, dataToSave);
	}

	
	#if UNITY_ANDROID

	private class PositiveButtonListner : AndroidJavaProxy {
		private ShareBehavior mDialog;
		
		public PositiveButtonListner(ShareBehavior d)
		: base("android.content.DialogInterface$OnClickListener") {
			mDialog = d;
		}
		
		public void onClick(AndroidJavaObject obj, int value ) {
			//Application.OpenURL(Settings.APP_REDIRECT_URL);

			if(Settings.IsAmazonBuild)
			{
				Application.OpenURL(Settings.APP_GOOGLE_URL);
			}
			else
			{
				Application.OpenURL(Settings.APP_GOOGLE_URL);
			}
		}
	}

	private class NegativeButtonListner : AndroidJavaProxy {
		private ShareBehavior mDialog;
		
		public NegativeButtonListner(ShareBehavior d)
		: base("android.content.DialogInterface$OnClickListener") {
			mDialog = d;
		}
		
		public void onClick(AndroidJavaObject obj, int value ) {
		}
	}
	
	#endif
		
	private void showAndroidDialog(string title, string message, string yesTitle, string noTitle) {
		
		#if UNITY_ANDROID
		AndroidJavaClass unityPlayer = new AndroidJavaClass ("com.unity3d.player.UnityPlayer");
		AndroidJavaObject activity = unityPlayer.GetStatic< AndroidJavaObject>  ("currentActivity");
		
		activity.Call("runOnUiThread", new AndroidJavaRunnable(() =>  {
			AndroidJavaObject alertDialogBuilder = new AndroidJavaObject("android/app/AlertDialog$Builder", activity);
			alertDialogBuilder.Call< AndroidJavaObject> ("setTitle", title);
			alertDialogBuilder.Call< AndroidJavaObject> ("setMessage", message);
			alertDialogBuilder.Call< AndroidJavaObject> ("setCancelable", true);
			alertDialogBuilder.Call< AndroidJavaObject> ("setPositiveButton", yesTitle, new PositiveButtonListner(this));
			alertDialogBuilder.Call< AndroidJavaObject> ("setNegativeButton", noTitle, new NegativeButtonListner(this));


			AndroidJavaObject dialog = alertDialogBuilder.Call< AndroidJavaObject> ("create");
			dialog.Call("show");
		}));
		#endif
		
	}

}