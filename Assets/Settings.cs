using UnityEngine;
using System.Collections;
using System.Linq;

public class Settings : MonoBehaviour
{
	public static bool IsAmazonBuild = false;

	public static string InterstitialFrequency = "10";
	public static string ShowBannerOnGamePlay = "0";
	public static string ShowBannerOnGameOver = "1";
	public static string AdsEnabled = "1";

	public static string RateUsFrequency = "15";

	public static string APP_IOS_URL = "https://itunes.apple.com/app/boxy-jump/id1059221156?ls=1&mt=8";
	public static string APP_GOOGLE_URL = "https://play.google.com/store/apps/details?id=lu.zor.boxy5";
	public static string APP_AMAZON_URL = "http://www.amazon.com/Omer-ZORLU-Boxy-Jump/dp/B017XQ3QZ4";
	public static string APP_REDIRECT_URL = "http://goo.gl/Q3TGKW";

	public static string Admob_IOS_Baner_Id = "ca-app-pub-8848560437945355/7889193505";
	public static string Admob_IOS_Interstitial_Id = "ca-app-pub-8848560437945355/9365926701";
	public static string Admob_Android_Banner_Id = "ca-app-pub-8848560437945355/9505527508";
	public static string Admob_Android_Interstitial_Id = "ca-app-pub-8848560437945355/1982260708";
	
	void Start() {
		try {
			StartCoroutine(TryUpdateFromWeb());
		} catch (System.Exception ex) {
			Debug.LogError("TryLoadFromWeb Error: " + ex.ToString());
		}
	}

	public IEnumerator TryUpdateFromWeb()
	{
		WebRequest request = new WebRequest();
		yield return StartCoroutine(request.MakeGET("&echo=settings"));

		if(string.IsNullOrEmpty(request.Error) && !string.IsNullOrEmpty(request.Text))
		{
			var pairs = request.Text.Split(",".ToCharArray(), System.StringSplitOptions.RemoveEmptyEntries);
			if(pairs != null)
			{
				foreach (var keyvalue in pairs) {
					var keyvaluePair = keyvalue.Split(":".ToCharArray(), System.StringSplitOptions.RemoveEmptyEntries);
					var currentkey = keyvaluePair[0];
					var currentvalue = keyvaluePair[1].Replace("'", "");

					var t = typeof(Settings);
					var property = t.GetFields().SingleOrDefault(f=>f.Name == currentkey);

					if(property != null)
					{
						property.SetValue(currentkey, currentvalue);
					}
				}
			}
		}
	}


}