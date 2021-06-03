using UnityEngine;
using System.Collections;
using UnityEngine.SocialPlatforms;
//using GoogleMobileAds.Api;
using System;
using System.Runtime.InteropServices;

public class Dev : MonoBehaviour {

	void Start() {
	}

	void OnGUI(){

#if DEBUG
		if(GUI.Button(new Rect(30, 30, 80,80), "Goto Main")){
			Application.LoadLevel("Main");
		}
		
		if(GUI.Button(new Rect(30, 130, 80,80), "Delete All")){
			//Social.Active.ReportScore(0, "BoxyScores", (result) => { print ("report score status is " + result); });
			
			PlayerPrefs.DeleteAll();
		}
#endif
	}

}
