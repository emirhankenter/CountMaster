using System.Collections;
using System.Collections.Generic;
using Game.Scripts.Models;
using Game.Scripts.View.Elements;
using Mek.Coroutines;
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
        [SerializeField] private Button _claimButton;
        [SerializeField] private Text _rewardText;
        
        private NumberAnimator _coinRewardNumberAnimator;

        private EndGamePanelParameters _params;
        
        public override void Open(ViewParams viewParams)
        {
            base.Open(viewParams);
            
            _params = viewParams as EndGamePanelParameters;
            if(_params == null) return;
            
            _coinRewardNumberAnimator = new NumberAnimator(_params.Reward, _rewardText);
            
            _currencyElement.Init(PlayerData.Instance.Coin);
            _claimButton.interactable = true;
        }

        public override void Close()
        {
            base.Close();
        }

        public void OnClaimButtonClicked()
        {
            _claimButton.interactable = false;
            UpdateCoin();
            
            CoroutineController.DoAfterGivenTime(2f, () =>
            {
                _params?.OnClaimed?.Invoke();
            });
        }

        private void UpdateCoin()
        {
            _coinRewardNumberAnimator.UpdateValue(0, roundToInt: true);
            _currencyElement.UpdateValue(PlayerData.Instance.Coin);
        }
    }
}