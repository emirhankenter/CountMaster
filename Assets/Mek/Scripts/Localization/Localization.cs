using System;
using System.Collections.Generic;
using System.Linq;
using Mek.Utilities;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Mek.Localization
{

    public class Localization : ScriptableObject
    {
        [Serializable] public class TranslationDictionary : SerializableDictionary<string, LanguageDictionary> { }
        [Serializable] public class LanguageDictionary : SerializableDictionary<SystemLanguage, string> { }

        [ReadOnly] public TranslationDictionary Dictionary = new TranslationDictionary();

        [SerializeField, HideInInspector] private List<SystemLanguage> _languages; 

        public void ReadCSV(string content)
        {
            Dictionary.Clear();
            // TextAsset csv = Resources.Load<TextAsset>("LocalizationDictionary");

            string[] data = content.Split(new char[] { '\n' });

            string[] firstRow = data[0].Split(new char[] {','});

            _languages = new List<SystemLanguage>();

            for (int i = 1; i < firstRow.Length; i++)
            {
                if (Enum.TryParse(firstRow[i], out SystemLanguage language))
                {
                    _languages.Add(language);
                }
            }

            for (int i = 1; i < data.Length; i++)
            {
                string[] row = data[i].Split(new char[] { ',' });
                if (data.Length == 0) continue;
                var key = row[0];
                
                if (Dictionary.TryGetValue(key, out LanguageDictionary dict))
                {
                }
                else
                {
                    if (key == string.Empty) continue;
                    dict = new LanguageDictionary();
                    Dictionary.Add(key, dict);
                }
                for (int j = 1; j < row.Length; j++)
                {
                    try
                    {
                        if (Enum.TryParse(firstRow[j], out SystemLanguage language))
                        {
                            if (dict.TryGetValue(language, out string translation))
                            {
                                dict[language] = row[j].ToUpper();
                            }
                            else
                            {
                                dict.Add(language, row[j].ToUpper());
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.LogException(e); //somethings wrong
                    }
                }
            }
        }

        public bool IsLanguageValid(SystemLanguage language)
        {
            return _languages.Contains(language);
        }
    }
}
