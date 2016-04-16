using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using SmartLocalization;

public class LocalizedText : MonoBehaviour {

	public string keyName;
	private Text text;
	private bool isEn = true;

	void Start () {
		text = GetComponent<Text>();

		//Subscribe to the change language event
		LanguageManager languageManager = LanguageManager.Instance;
		languageManager.OnChangeLanguage += OnChangeLanguage;

		//Run the method one first time
		OnChangeLanguage(languageManager);
	}

	void OnChangeLanguage(LanguageManager languageManager)
	{
		text.text = LanguageManager.Instance.GetTextValue(keyName);
	}
}
