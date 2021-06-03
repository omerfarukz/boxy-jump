using System.Collections.Generic;
using UnityEngine;

public class Localization
{
	private static bool _isInitialized;
	private static Localization _instance;

	private Dictionary<string, string> _dictionary;

	public static Localization Instance {
		get {
			if(_instance==null)
			{
				_instance = new Localization();
				_instance.Initialize();
			}

			return _instance;
		}
	}

	public Localization ()
	{
		_dictionary = new Dictionary<string, string>();
	}

	public void Initialize(){
//		if(_isInitialized)
//			return;
//
		//TR
		_dictionary.Add("TR_TUTORIAL_0", "Merhaba !\r\nBu küp");
		_dictionary.Add("TR_TUTORIAL_1", "... \r\nve bunlar da bloklar");
		_dictionary.Add("TR_TUTORIAL_2", "Küp tek ve çift zıplayabilir");
		_dictionary.Add("TR_TUTORIAL_3", "Tek zıplamak için ekranın soluna dokun");
		_dictionary.Add("TR_TUTORIAL_4", "Çift zıplamak için ekranın sağına dokun");
		_dictionary.Add("TR_TUTORIAL_5", "Mükemmel ! \r\n Bir kaç sefer daha zıpla");
		_dictionary.Add("TR_TUTORIAL_6", "Artık oynamaya hazırsın !");
		_dictionary.Add("TR_TUTORIAL_7", "Oyun yükleniyor");
		_dictionary.Add("TR_TUTORIAL_SKIP", "Geç");


		_dictionary.Add("TR_GAMEOVER_GAMEOVER", "OYUN BİTTİ");
		_dictionary.Add("TR_GAMEOVER_YOUSCORED", "PUAN");
		_dictionary.Add("TR_GAMEOVER_BESTSCORE", "EN YÜKSEK");
		_dictionary.Add("TR_RESTART", "BAŞLAT");
		_dictionary.Add("TR_RATEUS", "BEĞEN");
		_dictionary.Add("TR_SHARE", "PAYLAŞ");

		_dictionary.Add("TR_RATEUS_TITLE", "Boxy Jump'a Puan Ver");
		_dictionary.Add("TR_RATEUS_MESSAGE", "Lütfen bu oyuna puan verin");
		_dictionary.Add("TR_RATEUS_YES", "Şimdi Puan Ver");
		_dictionary.Add("TR_RATEUS_NO", "Daha Sonra");

		_dictionary.Add("TR_SHARE_TEXT", "Boxy Jump oyunundan {0} puan kazandım. {1}");

		_dictionary.Add("TR_GAME_TITLE", "Boxy Jump");


		// EN
		_dictionary.Add("EN_TUTORIAL_0", "Hello !\r\nThis is Boxy");
		_dictionary.Add("EN_TUTORIAL_1", "... \r\nand these are blocks");
		_dictionary.Add("EN_TUTORIAL_2", "Cube can jump single or double");
		_dictionary.Add("EN_TUTORIAL_3", "For single jump, touch left side");
		_dictionary.Add("EN_TUTORIAL_4", "For double jump - touch right side");
		_dictionary.Add("EN_TUTORIAL_5", "Perfect ! \r\nJump more");
		_dictionary.Add("EN_TUTORIAL_6", "And you are ready to play !");
		_dictionary.Add("EN_TUTORIAL_7", "Loading");
		_dictionary.Add("EN_TUTORIAL_SKIP", "Skip");

		_dictionary.Add("EN_GAMEOVER_GAMEOVER", "Game Over");
		_dictionary.Add("EN_GAMEOVER_YOUSCORED", "YOU SCORED");
		_dictionary.Add("EN_GAMEOVER_BESTSCORE", "BEST SCORE");
		_dictionary.Add("EN_RESTART", "RESTART");
		_dictionary.Add("EN_RATEUS", "LIKE");
		_dictionary.Add("EN_SHARE", "SHARE");

		_dictionary.Add("EN_RATEUS_TITLE", "Boxy Jump");
		_dictionary.Add("EN_RATEUS_MESSAGE", "Please rate this game.");
		_dictionary.Add("EN_RATEUS_YES", "Rate Now");
		_dictionary.Add("EN_RATEUS_NO", "Later");

		_dictionary.Add("EN_SHARE_TEXT", "I got {0} point on Boxy jump game. {1}");

		_dictionary.Add("EN_GAME_TITLE", "Boxy Jump");

		_isInitialized = true;
	}

	public string GetString(string key){
		if(Application.systemLanguage == SystemLanguage.Turkish)
		{
			var tr = GetString("TR", key);
			if(tr != null)
				return tr;
			else
				return GetString("EN", key);
		}
		else
		{
			return GetString("EN", key);
		}
	}

	public string GetString(string language, string key){
		string fullKey = string.Format("{0}_{1}", language, key);
		if(_dictionary.ContainsKey(fullKey)){
			return _dictionary[fullKey];
		}
		return null;
	}
}