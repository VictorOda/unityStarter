using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using SmartLocalization;

public class LanguageController : MonoBehaviour {

	public static LanguageController instance;

	void Start () {
		if(!instance)
			instance = this;

		DontDestroyOnLoad(this.gameObject);

		LanguageManager languageManager = LanguageManager.Instance;
	}
	
	public void SetLangPortuguese () {
		LanguageManager.Instance.ChangeLanguage("pt-BR");
	}

	public void SetLangEnglish () {
		LanguageManager.Instance.ChangeLanguage("en");
	}

	public void SetLangItalian () {
		LanguageManager.Instance.ChangeLanguage("it");
	}

	public void SetLangGerman () {
		LanguageManager.Instance.ChangeLanguage("de");
	}

	public void SetLangFrench () {
		LanguageManager.Instance.ChangeLanguage("fr");
	}

	public void SetLangSpanish () {
		LanguageManager.Instance.ChangeLanguage("es");
	}

}
