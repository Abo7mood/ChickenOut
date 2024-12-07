using UnityEngine;
using UnityEngine.Localization.Settings;
using TMPro;

namespace ChickenOut
{
    public enum Languages { Arabic, English }
    public class LanguageManager : MonoBehaviour
    {
        [SerializeField] GameObject[] bookTitles;
        [SerializeField] string bookName = "BookButton";

        const string LANGUAGE = "Language", ENGLISH = "English (en)", ARABIC = "Arabic (ar)";
        string language;

        TMP_Text[] allTexts;
        [SerializeField] TMP_FontAsset arabicFont, englishFont;

        void Awake()
        {
            allTexts = FindObjectsOfType<TMP_Text>(true);
            language = PlayerPrefs.GetString(LANGUAGE, ENGLISH);
        }
        void Start()
        {
            Invoke(nameof(UpdateLanguage), .1f);
            Changefonts();
        }

        public void ChangeLanguage(string languageName)
        {
            language = languageName;
            PlayerPrefs.SetString(LANGUAGE, language);

            UpdateLanguage();
            Changefonts();
        }

        void UpdateLanguage()
        {
            switch (language)
            {
                case ARABIC:
                    SetLanguage(Languages.Arabic);
                    ChangeBookTitles(Languages.Arabic);
                    break;

                case ENGLISH:
                    SetLanguage(Languages.English);
                    ChangeBookTitles(Languages.English);
                    break;
            }
        }
        void ChangeBookTitles(Languages language)
        {
            if (bookTitles != null && bookTitles.Length == LocalizationSettings.AvailableLocales.Locales.Count)
                for (int i = 0; i < bookTitles.Length; i++)
                    bookTitles[i].SetActive(i == (int)language);
        }

        void Changefonts()
        {
            foreach (TMP_Text text in allTexts)
                if (text.transform.parent.parent.name != bookName)
                    text.font = language == ARABIC ? arabicFont : englishFont;
        }

        void SetLanguage(Languages language) => LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[(int)language];
    }
}