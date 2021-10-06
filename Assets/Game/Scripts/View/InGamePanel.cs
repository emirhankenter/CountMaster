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
    public class InGamePanel : Panel
    {
        [SerializeField] private CurrencyElement _currencyElement;
        
        public override void Open(ViewParams viewParams)
        {
            base.Open(viewParams);
            
            _currencyElement.Init(PlayerData.Instance.Coin);
        }

        public override void Close()
        {
            base.Close();
        }
    }
}