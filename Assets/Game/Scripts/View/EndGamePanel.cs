using System.Collections;
using System.Collections.Generic;
using Game.Scripts.Models;
using Game.Scripts.View.Elements;
using Mek.Helpers;
using Mek.Localization;
using Mek.Navigation;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.View
{
    public class EndGamePanel : Panel
    {
        [SerializeField] private CurrencyElement _currencyElement;
        [SerializeField] private Text _rewardText;
        
        private NumberAnimator _coinNumberAnimator;

        private EndGamePanelParameters _params;
        
        public override void Open(ViewParams viewParams)
        {
            base.Open(viewParams);
            
            _params = viewParams as EndGamePanelParameters;
            if(_params == null) return;
            
            _coinNumberAnimator = new NumberAnimator(PlayerData.Instance.Coin, _rewardText);
            
            _currencyElement.Init(PlayerData.Instance.Coin);
        }

        public override void Close()
        {
            base.Close();
        }

        public void OnClaimButtonClicked()
        {
            UpdateCoin();
            _params?.OnClaimed?.Invoke();
        }

        public void UpdateCoin()
        {
            _coinNumberAnimator.UpdateValue(0);
            _currencyElement.UpdateValue(PlayerData.Instance.Coin);
        }
    }
}