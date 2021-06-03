using UnityEngine;
using UnityEngine.UI;

public class TextLocalizationBehavior : MonoBehaviour
{
	public string LocalizationKey;

	void Awake(){
		string text = Localization.Instance.GetString(LocalizationKey);
		GetComponent<Text>().text = text;
	}
}