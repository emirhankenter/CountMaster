using System.Collections;
using System.Collections.Generic;
using Mek.Helpers;
using Mek.Localization;
using Mek.Navigation;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.View
{
    public class MyPanel : Panel
    {
        [SerializeField] private Text _moneyText;
        [SerializeField] private Text _localizedText;

        private static NumberAnimator _moneyAnimator;
        public override void Open(ViewParams viewParams)
        {
            base.Open(viewParams);

            if (_moneyAnimator == null)
            {
                _moneyAnimator = new NumberAnimator(100, _moneyText);
            }
            else
            {
                _moneyAnimator.SetCurrent(500);
            }

            if (LocalizationManager.TryGetTranslationWithParameter("TIME LEFT", "time", "40", out string loc))
            {
                _localizedText.text = loc;
            }
        }

        public override void Close()
        {
            base.Close();
        }

        [Button]
        public void UpdateMoneyTo(float to = 1000)
        {
            _moneyAnimator.UpdateValue(to);
        }
    }
}