using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Mek.Localization
{
    public class Loc : MonoBehaviour
    {
        [SerializeField, ValueDropdown(nameof(GetLocalizationKeys)), OnValueChanged(nameof(SetText))] private string _key;
        
        [SerializeField, HideInInspector] private Text _text;

        private void Awake()
        {
            SetText();

            LocalizationManager.LanguageChanged += OnLanguageChanged;
        }

        private void OnDestroy()
        {
            LocalizationManager.LanguageChanged -= OnLanguageChanged;
        }

        private void OnLanguageChanged()
        {
            SetText();
        }

        private void SetText()
        {
            if (LocalizationManager.TryGetTranslation(_key, out string translation))
            {
                if (_text == null)
                {
                    _text = GetComponent<Text>();
                }
                
                _text.text = translation;
            }
        }
        
        private IEnumerable<string> GetLocalizationKeys()
        {
            return LocalizationManager.GetLocalizationKeys();
        }
    }
}
