using System;
using System.Collections.Generic;
using Mek.Models;
using UnityEngine;

namespace Mek.Localization
{
    public static class LocalizationManager
    {
        public static event Action LanguageChanged; 

        private static Localization _localization;

        private static SystemLanguage? _language;

        public static SystemLanguage Language
        {
            get
            {
                if (_language == null)
                {
                    return Application.systemLanguage;
                }

                return _language.Value;
            }
            private set
            {
                _language = value;
                LanguageChanged?.Invoke();
            } 
        }

        private static MekLog Log = new MekLog(nameof(LocalizationManager), DebugLevel.Debug);

        public static void Init(SystemLanguage defaultLanguage = SystemLanguage.English)
        {
            _localization = Resources.Load<Localization>("Localization");
            SetLanguage(defaultLanguage);
        }

        public static void SetLanguage(SystemLanguage language)
        {
            if (!_localization.IsLanguageValid(language)) return;
            if (Language == language) return;
            
            Language = language;
        }

        public static bool TryGetTranslation(string key, out string translation)
        {
            if (_localization == null)
            {
                _localization = Resources.Load<Localization>("Localization");
            }
            translation = "";
            if (_localization.Dictionary.TryGetValue(key, out Localization.LanguageDictionary langDict))
            {
                if (langDict.TryGetValue(Language, out translation))
                {
                    return true;
                }
                else
                {
                    Log.Error($"Translation does not exists!");
                    return false;
                }
            }
            else
            {
                Log.Error($"Translation does not exists!");
                return false;
            }
        }

        public static bool TryGetTranslationWithParameter(string key, string parameter, string value, out string loc)
        {
            loc = "";
            if (TryGetTranslation(key, out var translation))
            {
                loc = translation.Replace("{" + parameter + "}", value);

                return true;
            }

            return false;
        }

        public static IEnumerable<string> GetLocalizationKeys()
        {
            if (_localization == null)
            {
                _localization = Resources.Load<Localization>("Localization");
            }

            if (_localization)
            {
                return _localization.Dictionary.Keys;
            }
            else
            {
                Debug.LogError($"Could not found the localization asset!");
                return null;
            }
        }
    }
}
